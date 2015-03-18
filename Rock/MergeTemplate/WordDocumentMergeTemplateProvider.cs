﻿// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlPowerTools;
using Rock.Data;
using Rock.Model;

namespace Rock.MergeTemplate
{
    /// <summary>
    /// 
    /// </summary>
    public class WordDocumentMergeTemplateProvider : MergeTemplateProvider
    {
        /// <summary>
        /// Creates the document.
        /// </summary>
        /// <param name="mergeTemplate">The merge template.</param>
        /// <param name="mergeObjectsList">The merge objects list.</param>
        /// <returns></returns>
        public override BinaryFile CreateDocument( Rock.Model.MergeTemplate mergeTemplate, List<Dictionary<string, object>> mergeObjectsList )
        {
            var rockContext = new RockContext();
            var binaryFileService = new BinaryFileService( rockContext );
            MemoryStream outputDocStream = new MemoryStream();
            var templateBinaryFile = binaryFileService.Get( mergeTemplate.TemplateBinaryFileId );
            if ( templateBinaryFile == null )
            {
                return null;
            }

            var sourceTemplateStream = templateBinaryFile.ContentStream;

            // Start by creating a new document with the contents of the Template (so that Styles, etc get included)
            sourceTemplateStream.CopyTo( outputDocStream );
            outputDocStream.Seek( 0, SeekOrigin.Begin );

            using ( WordprocessingDocument outputDoc = WordprocessingDocument.Open( outputDocStream, true ) )
            {
                var xdoc = outputDoc.MainDocumentPart.GetXDocument();
                var outputBodyNode = xdoc.DescendantNodes().OfType<XElement>().FirstOrDefault( a => a.Name.LocalName.Equals( "body" ) );
                outputBodyNode.RemoveNodes();

                int recordIndex = 0;
                int recordCount = mergeObjectsList.Count();
                while ( recordIndex < recordCount )
                {
                    var tempMergeTemplateStream = new MemoryStream();
                    sourceTemplateStream.Position = 0;
                    sourceTemplateStream.CopyTo( tempMergeTemplateStream );
                    tempMergeTemplateStream.Position = 0;
                    var tempMergeWordDoc = WordprocessingDocument.Open( tempMergeTemplateStream, true );

                    MarkupSimplifier.SimplifyMarkup( tempMergeWordDoc, this.simplifyMarkupSettingsAll );
                    var tempMergeTemplateX = tempMergeWordDoc.MainDocumentPart.GetXDocument();
                    var tempMergeTemplateBodyNode = tempMergeTemplateX.DescendantNodes().OfType<XElement>().FirstOrDefault( a => a.Name.LocalName.Equals( "body" ) );

                    // find all the Nodes that have a {& next &}.  
                    List<XElement> nextIndicatorNodes = new List<XElement>();

                    OpenXmlRegex.Match(
                        tempMergeTemplateX.Elements(),
                        this.nextRecordRegEx,
                        ( x, m ) =>
                        {
                            nextIndicatorNodes.Add( x );
                        } );

                    var allSameParent = nextIndicatorNodes.Count > 1 && nextIndicatorNodes.Select( a => a.Parent ).Distinct().Count() == 1;

                    foreach ( var nextIndicatorNode in nextIndicatorNodes )
                    {
                        // Each of the nextIndicatorNodes will get a record until we run out of nodes or records.  
                        // If we have more records than nodes, we'll jump out to the outer "while" and append another template and keep going
                        XContainer recordContainerNode = null;
                        if ( nextIndicatorNode != null && nextIndicatorNode.Parent != null )
                        {
                            recordContainerNode = nextIndicatorNode.Parent;
                        }
                        else
                        {
                            // shouldn't happen
                            continue;
                        }

                        XContainer mergedXRecord;

                        if ( recordIndex >= recordCount )
                        {
                            // out of records, so clear out any remaining template nodes that haven't been merged
                            string xml = recordContainerNode.ToString().ReplaceWordChars();
                            mergedXRecord = XElement.Parse( xml ) as XContainer;
                            OpenXmlRegex.Replace( mergedXRecord.Nodes().OfType<XElement>(), this.regExDot, string.Empty, ( a, b ) => { return true; } );

                            recordIndex++;
                        }
                        else
                        {
                            List<string> xmlChunks = new List<string>();

                            if ( allSameParent )
                            {
                                // if all the nextRecord nodes have the same parent, just split the XML for each record and reassemble it when done
                                xmlChunks.AddRange( this.nextRecordEncodedRegEx.Split( recordContainerNode.ToString().ReplaceWordChars() ) );
                            }
                            else
                            {
                                xmlChunks.Add( recordContainerNode.ToString().ReplaceWordChars() );
                            }

                            string mergedXml = string.Empty;

                            foreach ( var xml in xmlChunks )
                            {
                                if ( recordIndex < recordCount )
                                {
                                    if ( xml.HasMergeFields() )
                                    {
                                        DotLiquid.Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention();
                                        DotLiquid.Template template = DotLiquid.Template.Parse( xml );
                                        mergedXml += template.Render( DotLiquid.Hash.FromDictionary( mergeObjectsList[recordIndex] ) );
                                        recordIndex++;
                                    }
                                    else
                                    {
                                        mergedXml += xml;
                                    }
                                }
                                else
                                {
                                    mergedXml += xml;
                                }
                            }

                            mergedXRecord = XElement.Parse( mergedXml ) as XContainer;
                        }

                        // remove the orig nodes and replace with merged nodes
                        recordContainerNode.RemoveNodes();
                        foreach ( var childNode in mergedXRecord.Nodes() )
                        {
                            var xchildNode = childNode as XElement;
                            recordContainerNode.Add( xchildNode );
                        }

                        var mergedRecordContainer = XElement.Parse( recordContainerNode.ToString() );
                        var nextNode = recordContainerNode.NextNode;
                        if ( recordContainerNode.Parent != null )
                        {
                            // the recordContainerNode is some child/descendent of <body>
                            recordContainerNode.ReplaceWith( mergedRecordContainer );
                        }
                        else
                        {
                            // the recordContainerNode is the <body>
                            recordContainerNode.RemoveNodes();
                            foreach ( var node in mergedRecordContainer.Nodes() )
                            {
                                recordContainerNode.Add( node );
                            }

                            if ( recordIndex < recordCount )
                            {
                                // add page break
                                var pageBreakXml = new DocumentFormat.OpenXml.Wordprocessing.Break() { Type = BreakValues.Page }.OuterXml;
                                var pageBreak = XElement.Parse( pageBreakXml );
                                recordContainerNode.Add( pageBreak );
                            }
                        }
                    }

                    foreach ( var childNode in tempMergeTemplateBodyNode.Nodes() )
                    {
                        outputBodyNode.Add( childNode );
                    }
                }

                // remove all the 'next' delimiters
                OpenXmlRegex.Replace( outputBodyNode.Nodes().OfType<XElement>(), this.nextRecordRegEx, string.Empty, ( xx, mm ) => { return true; } );

                // remove the last pagebreak if there is nothing after it
                var lastBodyElement = outputBodyNode.Nodes().OfType<XElement>().LastOrDefault();
                if ( lastBodyElement != null && lastBodyElement.Name.LocalName == "br" )
                {
                    if ( lastBodyElement.Parent != null )
                    {
                        lastBodyElement.Remove();
                    }
                }

                // pop the xdoc back
                outputDoc.MainDocumentPart.PutXDocument();
            }

            var outputBinaryFile = new BinaryFile();
            outputBinaryFile.IsTemporary = false;
            outputBinaryFile.ContentStream = outputDocStream;
            // TODO



            binaryFileService.Add( outputBinaryFile );
            rockContext.SaveChanges();

            return outputBinaryFile;
        }

        /// <summary>
        /// The simplify markup settings all
        /// </summary>
        private SimplifyMarkupSettings simplifyMarkupSettingsAll = new SimplifyMarkupSettings
        {
            NormalizeXml = true,
            RemoveWebHidden = true,
            RemoveBookmarks = true,
            RemoveGoBackBookmark = true,
            RemoveMarkupForDocumentComparison = true,
            RemoveComments = true,
            RemoveContentControls = true,
            RemoveEndAndFootNotes = true,
            RemoveFieldCodes = false,
            RemoveLastRenderedPageBreak = true,
            RemovePermissions = true,
            RemoveProof = true,
            RemoveRsidInfo = true,
            RemoveSmartTags = true,
            RemoveSoftHyphens = true,
            ReplaceTabsWithSpaces = true
        };

        /// <summary>
        /// The RegEx for finding the "next" delimiter/indicator
        /// </summary>
        private Regex nextRecordRegEx = new Regex( @"{&\s*\bnext\b\s*&}", RegexOptions.IgnoreCase );

        /// <summary>
        /// The RegEx for finding the "next" delimiter/indicator (for Xml Encoded strings)
        /// </summary>
        private Regex nextRecordEncodedRegEx = new Regex( @"{&amp;\s*\bnext\b\s*&amp;}", RegexOptions.IgnoreCase );

        /// <summary>
        /// The RegEx of "." that matches anything
        /// </summary>
        private Regex regExDot = new Regex( "." );
    }
}
