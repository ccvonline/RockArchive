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
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rock;
using Rock.Data;
using Rock.Model;

namespace church.ccv.Podcast
{
    public static class PodcastUtil
    {
        //todo: This should be moved into our CCV Guids, but not until we first create it in Production ON the Content Channel
        public const string ContentChannel_CategoryAttributeGuid = "DEA4ACCE-82F6-43E3-B381-6959FBF66E74";
        public const int RootPodcast_CategoryId = 450;
        public const int WeekendVideos_CategoryId = 451;

        static PodcastCategory GetPodcastsByCategory( int categoryId )
        {
            RockContext rockContext = new RockContext( );

            // get the root category that's parent to all categories and podcasts they care about
            var category = new CategoryService( rockContext ).Queryable( ).Where( c => c.Id == categoryId ).SingleOrDefault( );
            
            // create a query that'll get all of the "Category" attributes for all the content channels
            var categoryAttribValList = new AttributeValueService( rockContext ).Queryable( ).Where( av => av.Attribute.Guid == new Guid( ContentChannel_CategoryAttributeGuid  ) );

            // now get ALL content channels with their parent category(s) attributes as a joined object
            ContentChannelService contentChannelService = new ContentChannelService( rockContext );
            var categoryContentChannelItems = contentChannelService.Queryable( ).Join( categoryAttribValList, 
                                                                                       cc => cc.Id, cav => cav.EntityId, ( cc, cav ) => new ContentChannelWithAttribs { ContentChannel = cc, CategoryAttribValue = cav } );
            
            // create our root podcast object
            PodcastCategory rootPodcast = new PodcastCategory( );
                        
            // see if this category has any podcasts to add.
            Internal_GetPodcastsByCategory( category, rootPodcast, categoryContentChannelItems );
    
            return rootPodcast;
        }

        static void Internal_GetPodcastsByCategory( Category category, PodcastCategory rootPodcast, IQueryable<ContentChannelWithAttribs> categoryContentChannelItems )
        {
            rootPodcast.Name = category.Name;
            rootPodcast.Children = new List<IPodcastNode>( );

            // Get all Content Channels that are immediate children of the provided category.
            var podcastsForCategory = categoryContentChannelItems.Where( cci => cci.CategoryAttribValue.Value.Contains( category.Guid.ToString( ) ) ).Select( cci => cci.ContentChannel );

            // Convert all the content channel items into PodcastSeries and add them as children.
            foreach( ContentChannel contentChannel in podcastsForCategory )
            {
                PodcastSeries podcastSeries = ContentChannelToPodcastSeries( contentChannel );

                rootPodcast.Children.Add( podcastSeries );
            }
            
            // now sort all series based on their first message's date.
            // (We're safe to do this because we KNOW we've only added PodcastSeries at this point)
            rootPodcast.Children.Sort( delegate( IPodcastNode a, IPodcastNode b )
            {
                PodcastSeries podcastSeriesA = a as PodcastSeries;
                PodcastSeries podcastSeriesB = b as PodcastSeries;
                
                if( podcastSeriesA.Date > podcastSeriesB.Date )
                {
                    return -1;
                }
                return 1;
            });

            // now recursively handle all child categories
            foreach( Category childCategory in category.ChildCategories )
            {
                // create the child podcast category
                PodcastCategory childPodcastCategory = new PodcastCategory( );

                // add it to this root podcast
                rootPodcast.Children.Add( childPodcastCategory );

                // now recursively call this so it can add its children
                Internal_GetPodcastsByCategory( childCategory, childPodcastCategory, categoryContentChannelItems );
            }
        }

        static PodcastSeries ContentChannelToPodcastSeries( ContentChannel contentChannel )
        {
            // Given a content channel in the database, this will convert it into our Podcast model.

            RockContext rockContext = new RockContext( );
            IQueryable<AttributeValue> attribValQuery = new AttributeValueService( rockContext ).Queryable( );

            // get the list of attributes for this content channel
            List<AttributeValue> contentChannelAttribValList = attribValQuery.Where( av => av.EntityId == contentChannel.Id && 
                                                                                            av.Attribute.EntityTypeQualifierColumn == "ContentChannelTypeId" )
                                                                             .ToList( );

            // setup the series
            PodcastSeries series = new PodcastSeries( );
            series.Name = contentChannel.Name;
            series.Description = contentChannel.Description;
                
            // add all the attributes
            series.Attributes = new Dictionary<string, string>( );
            foreach( AttributeValue attribValue in contentChannelAttribValList )
            {
                series.Attributes.Add( attribValue.AttributeKey, attribValue.Value );
            }
                
            series.Messages = new List<PodcastMessage>( );
            foreach( ContentChannelItem contentChannelItem in contentChannel.Items )
            {
                // get this message's attributes
                List<AttributeValue> itemAttribValList = attribValQuery.Where( av => av.EntityId == contentChannelItem.Id && 
                                                                                        av.Attribute.EntityTypeQualifierColumn == "ContentChannelTypeId" )
                                                                       .ToList( );

                PodcastMessage message = new PodcastMessage( );
                    
                message.Name = contentChannelItem.Title;
                message.Description = contentChannelItem.Content;
                message.Date = contentChannelItem.StartDateTime;

                // add all the attributes
                message.Attributes = new Dictionary<string, string>( );
                foreach( AttributeValue attribValue in itemAttribValList )
                {
                    message.Attributes.Add( attribValue.AttributeKey, attribValue.Value );
                }

                series.Messages.Add( message );
            }

            // if there are messages, sort them by date
            if( series.Messages.Count > 0 )
            {
                // sort the messages by date
                series.Messages.Sort( delegate( PodcastMessage a, PodcastMessage b )
                {
                    if( a.Date > b.Date )
                    {
                        return -1;
                    }
                    return 1;
                });

                // then set the series' date to the first message's (earliest)
                series.Date = series.Messages[0].Date;
            }
            else
            {
                series.Date = null;
            }

            return series;
        }

        public static PodcastCategory PodcastsAsModel( int podcastCategory )
        {
            return GetPodcastsByCategory( podcastCategory );
        }
        
        public static string PodcastsAsJson( int podcastCategory )
        {
            PodcastCategory requestedPodcast = GetPodcastsByCategory( podcastCategory );
            
            return JsonConvert.SerializeObject( requestedPodcast );
        }

        // Helper class for storing a Content Channel with its Category Attribute Value
        public class ContentChannelWithAttribs
        {
            public ContentChannel ContentChannel { get; set; }
            public AttributeValue CategoryAttribValue { get; set; }
        }

        // Interface so that PodcastCategories can have either Series or Categories as children.
        public interface IPodcastNode : Rock.Lava.ILiquidizable
        {
        }

        public class PodcastCategory : IPodcastNode
        {
            public string Name { get; set; }
            public List<IPodcastNode> Children { get; set; }

            // Liquid Methods
            [JsonIgnore]
            [Rock.Data.LavaIgnore]
            public List<string> AvailableKeys
            {
                get
                {
                    var availableKeys = new List<string> { "Name", "Children" };
                                            
                    foreach ( IPodcastNode child in Children )
                    {
                        availableKeys.AddRange( child.AvailableKeys );
                    }

                    return availableKeys;
                }
            }
            
            public object ToLiquid()
            {
                return this;
            }

            [JsonIgnore]
            [Rock.Data.LavaIgnore]
            public object this[object key]
            {
               get
                {
                   switch( key.ToStringSafe() )
                   {
                       case "Name": return Name;
                       case "Children": return Children;
                   }

                    return null;
                }
            }
            
            public bool ContainsKey( object key )
            {
                var additionalKeys = new List<string> { "Name", "Children" };
                if ( additionalKeys.Contains( key.ToStringSafe() ) )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //
        }

        public class PodcastSeries : IPodcastNode
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime? Date { get; set; }
            public Dictionary<string, string> Attributes { get; set; }
            public List<PodcastMessage> Messages { get; set; }

            // Liquid Methods
            [JsonIgnore]
            [Rock.Data.LavaIgnore]
            public List<string> AvailableKeys
            {
                get
                {
                    var availableKeys = new List<string> { "Name", "Description", "Date", "Attributes", "Messages" };

                    foreach ( IPodcastNode child in Messages )
                    {
                        availableKeys.AddRange( child.AvailableKeys );
                    }

                    return availableKeys;
                }
            }
            
            public object ToLiquid()
            {
                return this;
            }

            [JsonIgnore]
            [Rock.Data.LavaIgnore]
            public object this[object key]
            {
               get
                {
                   switch( key.ToStringSafe() )
                   {
                       case "Name": return Name;
                       case "Description": return Description;
                       case "Date": return Date;
                       case "Attributes": return Attributes;
                       case "Messages": return Messages;
                   }

                    return null;
                }
            }
            
            public bool ContainsKey( object key )
            {
                var additionalKeys = new List<string> { "Name", "Description", "Date", "Attributes", "Messages" };
                if ( additionalKeys.Contains( key.ToStringSafe() ) )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //
        }

        public class PodcastMessage
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }
            public Dictionary<string, string> Attributes { get; set; }

            // Liquid Methods
            [JsonIgnore]
            [Rock.Data.LavaIgnore]
            public List<string> AvailableKeys
            {
                get
                {
                    var availableKeys = new List<string> { "Name", "Description", "Date", "Attributes" };
                    
                    return availableKeys;
                }
            }
            
            public object ToLiquid()
            {
                return this;
            }

            [JsonIgnore]
            [Rock.Data.LavaIgnore]
            public object this[object key]
            {
               get
                {
                   switch( key.ToStringSafe() )
                   {
                       case "Name": return Name;
                       case "Description": return Description;
                       case "Date": return Date;
                       case "Attributes": return Attributes;
                   }

                    return null;
                }
            }
            
            public bool ContainsKey( object key )
            {
                var additionalKeys = new List<string> { "Name", "Description", "Date", "Attributes" };
                if ( additionalKeys.Contains( key.ToStringSafe() ) )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //
        }
    }
}
