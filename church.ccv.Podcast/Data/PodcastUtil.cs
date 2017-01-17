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
using Rock.Web.Cache;
using System.Data.Entity;

namespace church.ccv.Podcast
{
    public static class PodcastUtil
    {
        // This is rock's core podcast category and will never change.
        const int RootPodcast_CategoryId = 451;

        // This is Rock's Weekend Videos podcast category, and will also never change.
        // This is defined for external systems (like the block that displays our Weekend Series)
        public const int WeekendVideos_CategoryId = 452;

        const int ContentChannelTypeId_PodcastSeries = 8;

        // Returns podcasts by category (or by the root if 0 is provided).
        // Can return NULL if the user provided isn't allowed to view the category of categoryId
        public static PodcastCategory GetPodcastsByCategory( int categoryId, bool keepCategoryHierarchy = true, int maxSeries = int.MaxValue, int numMessages = int.MaxValue, int personAliasId = 0 )
        {
            if( maxSeries != int.MaxValue && numMessages != int.MaxValue )
            {
                throw new Exception( "Cannot provide both a maxSeries and maxMessage. They conflict. Use one or the other." );
            }

            // if they pass in 0, accept that as the Root
            if ( categoryId == 0 )
            {
                categoryId = RootPodcast_CategoryId;
            }

            // get the root category that's parent to all categories and podcasts they care about
            RockContext rockContext = new RockContext( );
            var category = CategoryCache.Read( categoryId );

            // check the security on this category to see if it should be viewable.
            // a NULL person is considered an "All Users" person, so if no ID was passed,
            // we'll treat the requester as an "All User"
            Person requestingPerson = null;
            if( personAliasId > 0 )
            {
                // take the person, if they're found. If somehow they're null, we'll treat them like
                // an anonymous user
                PersonAlias personAlias = new PersonAliasService( rockContext ).Get( personAliasId );
                if( personAlias != null )
                {
                    requestingPerson = personAlias.Person;
                }
            }
            bool viewAllowed = category.IsAuthorized( Rock.Security.Authorization.VIEW, requestingPerson );

            // if this user (anonymous, or specific) is allowed to view the category, load it.
            // otherwise, we'll just pass back an empty category
            PodcastCategory rootPodcast = null;
            if ( viewAllowed )
            {
                // create a query that'll get all of the "Category" attributes for all the content channels
                var categoryAttribValList = new AttributeValueService( rockContext ).Queryable( ).Where( av => av.Attribute.Guid == new Guid( church.ccv.Utility.SystemGuids.Attribute.CONTENT_CHANNEL_CATEGORY_ATTRIBUTE  ) );

                // now get ALL content channels with their parent category(s) attributes as a joined object
                ContentChannelService contentChannelService = new ContentChannelService( rockContext );
                var categoryContentChannelItems = contentChannelService.Queryable( ).Join( categoryAttribValList, 
                                                                                           cc => cc.Id, cav => cav.EntityId, ( cc, cav ) => new ContentChannelWithAttrib { ContentChannel = cc, AttribValue = cav.Value } );
            
                // create our root podcast object
                rootPodcast = new PodcastCategory( category.Name, category.Id );

                // see if this category has any podcasts to add.
                Internal_GetPodcastsByCategory( category, rootPodcast, categoryContentChannelItems, ref maxSeries, ref numMessages, keepCategoryHierarchy, requestingPerson, rockContext );
            }
    
            return rootPodcast;
        }
        
        static void Internal_GetPodcastsByCategory( CategoryCache category, PodcastCategory rootPodcast, IQueryable<ContentChannelWithAttrib> categoryContentChannelItems, ref int numSeriesToAdd, ref int numMessagesToAdd, bool keepCategoryHierarchy, Person requestingPerson, RockContext rockContext )
        {
            // first get ALL podcasts for this category (sorted by creation date)
            var podcastsForCategoryFull = categoryContentChannelItems.Where( cci => cci.AttribValue.Contains( category.Guid.ToString( ) ) )
                                                                 .Select( cci => cci.ContentChannel )
                                                                 .OrderByDescending( cc => cc.CreatedDateTime )
                                                                 .Include( cc => cc.Items )
                                                                 .AsNoTracking( );

            var podcastsForCategory = SortPodcastSeriesByPriority( podcastsForCategoryFull, rockContext );
            podcastsForCategory = podcastsForCategory.Take( numSeriesToAdd );
            
            // Convert all the content channel items into PodcastSeries and add them as children.
            // (We're safe to do this because we KNOW we've only added PodcastSeries at this point)
            foreach ( ContentChannel contentChannel in podcastsForCategory )
            {
                // convert the series, which may return null if it doesn't match the requested viewState
                PodcastSeries podcastSeries = ContentChannelToPodcastSeries( contentChannel, numMessagesToAdd );
                rootPodcast.Children.Add( podcastSeries );

                // now see if we've hit our max messages.
                numMessagesToAdd -= podcastSeries.Messages.Count;
                if ( numMessagesToAdd <= 0 ) break;
            }

            // subtract the number of podcasts added
            numSeriesToAdd -= podcastsForCategory.Count( );
            
            // now recursively handle all child categories
            foreach ( CategoryCache childCategory in category.Categories )
            {
                if ( childCategory.IsAuthorized( Rock.Security.Authorization.VIEW, requestingPerson ) )
                {
                    PodcastCategory podcastCategory = null;

                    // if true, we'll maintain the category hierarchy.
                    if ( keepCategoryHierarchy )
                    {
                        // so create a new child category object
                        PodcastCategory childPodcastCategory = new PodcastCategory( childCategory.Name, childCategory.Id );

                        // add it to this root podcast
                        rootPodcast.Children.Add( childPodcastCategory );

                        // and set it as the next category to use
                        podcastCategory = childPodcastCategory;
                    }
                    else
                    {
                        // false, so we should use the initial root category, so that all series / messages go into this category's
                        // Children list as a big flat list
                        podcastCategory = rootPodcast;
                    }


                    // if there are more podcasts to add
                    if ( numSeriesToAdd > 0 && numMessagesToAdd > 0 )
                    {
                        // recursively call this so it can add its children
                        Internal_GetPodcastsByCategory( childCategory, podcastCategory, categoryContentChannelItems, ref numSeriesToAdd, ref numMessagesToAdd, keepCategoryHierarchy, requestingPerson, rockContext );
                    }
                }
            }
        }

        public static IQueryable<ContentChannel> SortPodcastSeriesByPriority( IQueryable<ContentChannel> seriesQuery, RockContext rockContext )
        {
            // Now we want to join any podcast that has a "Priority" attribute with that attribute, which controls what order they should be in.
            // Lower number means higher on the list.
            // Note - Not all Content Channels have the Priority Attribute, as it was added recently. For those, assume a priority of 0.
            var attribValList = new AttributeValueService( rockContext ).Queryable( ).Where( av => av.Attribute.Guid == new Guid( church.ccv.Utility.SystemGuids.Attribute.CONTENT_CHANNEL_PRIORITY_ATTRIBUTE ) );

            // select all Content Channels joined with their Priority Attribute. For those with no Attribute, we'll use a blank default one.
            var podcastsForCategoryWithAttrib = seriesQuery.AsQueryable( ).GroupJoin( attribValList,
                                                                                                  x => x.Id, 
                                                                                                  y => y.EntityId, 
                                                                                                  ( x, y ) => new { ContentChannel = x, AttribValue = y } )

                                                                                      .SelectMany( xy => xy.AttribValue.DefaultIfEmpty(),
                                                                                                   ( x, y ) => new { ContentChannelContainer = x, AttribValue = y } )

                                                                                      .Select( xy => new { ContentChannel = xy.ContentChannelContainer.ContentChannel, AttribValue = xy.AttribValue } )
                                                                                      .ToList( );

            // Finally, order the items by descending (Lower priority goes on top), and then take the number requested.
            var podcastsForCategory = podcastsForCategoryWithAttrib.OrderByDescending( cc => cc.ContentChannel.CreatedDateTime )
                                                                   .OrderBy( cc => cc.AttribValue != null ? int.Parse( cc.AttribValue.Value ) : 0 )
                                                                   .Select( cc => cc.ContentChannel );
            
            return podcastsForCategory.AsQueryable( );
        }

        public static DateTime? LatestModifiedDateTime( )
        {
            //JHM 6-21-2016: cc.ModifiedDateTime< DateTime.Now is a hack to fix an issue when porting from Arena. We used the Series StartDateTime as the Created/Modified DateTime, which
            // causes a ModifiedDateTime in the future. This can go away after 7-3-2016.

            // this will gather the series, messages, and all attribute values, and see what the date/time of the most recent change was
            RockContext rockContext = new RockContext( );

            DateTime? latestDate = null;

            // get all podcast series sorted by ModifiedDateTime
            ContentChannelService contentChannelService = new ContentChannelService( rockContext );
            var podcastSeries = contentChannelService.Queryable( ).Where( cc => cc.ContentChannelTypeId == ContentChannelTypeId_PodcastSeries && cc.ModifiedDateTime < DateTime.Now )
                                                                  .Select( ps => new { ps.Id, ps.ModifiedDateTime } ).AsNoTracking( );
            if( podcastSeries.Count( ) > 0 )
            {
                // take the latest modified date/time and assume that's the latest change
                var seriesIds = podcastSeries.Select( ps => ps.Id );
                latestDate = podcastSeries.Max( ps => ps.ModifiedDateTime );
                
                // get all messages for all the podcast series
                ContentChannelItemService contentChannelItemService = new ContentChannelItemService( rockContext );
                var podcastMessages = contentChannelItemService.Queryable( ).Where( cci => seriesIds.Contains( cci.ContentChannelId ) && cci.ModifiedDateTime< DateTime.Now )
                                                                            .Select( cci => new { cci.Id, cci.ModifiedDateTime } ).AsNoTracking( );
                if ( podcastMessages.Count() > 0 )
                {
                    // take the message IDs and the most recent modifiedDateTime
                    var messageIds = podcastMessages.Select( pm => pm.Id );
                    var mostRecentDate = podcastMessages.Max( pm => pm.ModifiedDateTime );

                    // if there's a more recent change in the messages, take that date/time
                    if ( latestDate < mostRecentDate )
                    {
                        latestDate = mostRecentDate;
                    }

                    // NOW, get the most recent series OR messages' attribValue modifiedDateTime
                    mostRecentDate = new AttributeValueService( rockContext ).Queryable()
                                                .Where( av => av.Attribute.EntityTypeQualifierColumn == "ContentChannelTypeId" && 
                                                av.ModifiedDateTime < DateTime.Now && 
                                                ( seriesIds.Contains( av.EntityId.Value ) || messageIds.Contains( av.EntityId.Value ) ) )
                                                .AsNoTracking( )
                                                .Max( av => av.ModifiedDateTime );

                    // and see if it's newer
                    if ( latestDate < mostRecentDate )
                    {
                        latestDate = mostRecentDate;
                    }
                    
                }
            }

            // now return the latestDate, which is the most recent change to ANYTHING related to the podcast system
            return latestDate;
        }

        static PodcastSeries ContentChannelToPodcastSeries( ContentChannel contentChannel, int numMessages )
        {
            // Given a content channel in the database, this will convert it into our Podcast model.
            RockContext rockContext = new RockContext( );
            IQueryable<AttributeValue> attribValQuery = new AttributeValueService( rockContext ).Queryable( );

            // get the list of attributes for this content channel
            var contentChannelAttribValList = attribValQuery.Where( av => av.EntityId == contentChannel.Id && av.Attribute.EntityTypeQualifierColumn == "ContentChannelTypeId" )
                                                            .Select( av => new { AttributeKey = av.Attribute.Key, av.Value } )
                                                            .AsNoTracking( )
                                                            .ToList( );

            // setup the series
            PodcastSeries series = new PodcastSeries( );
            series.Id = contentChannel.Id;
            series.Name = contentChannel.Name;
            series.Description = contentChannel.Description;

            // add all the attributes
            series.Attributes = new Dictionary<string, string>( );
            foreach( var attribValue in contentChannelAttribValList )
            {
                series.Attributes.Add( attribValue.AttributeKey, attribValue.Value );
            }

            // use the created dateTime as the series' date.
            series.Date = contentChannel.CreatedDateTime;
            
            // now add all the messages
            series.Messages = new List<PodcastMessage>( );

            // sort the messages by date
            var orderedContentChannelItems = contentChannel.Items.OrderByDescending( it => it.StartDateTime );
            
            // and then priority (lowest priority goes to top)
            orderedContentChannelItems = orderedContentChannelItems.OrderBy( it => it.Priority );

            // now take whatever's less. the number of messages in the series, or numMessages requested.
            var orderedContentChannelItemList = orderedContentChannelItems.ToList( );
            
            int i;
            for( i = 0; i < Math.Min( numMessages, orderedContentChannelItems.Count( ) ); i++ )
            {
                ContentChannelItem contentChannelItem = orderedContentChannelItemList[ i ];

                // convert each contentChannelItem into a PodcastMessage, and add it to our list
                PodcastMessage message = ContentChannelItemToPodcastMessage( contentChannelItem );
                series.Messages.Add( message );
            }

            return series;
        }

        static PodcastMessage ContentChannelItemToPodcastMessage( ContentChannelItem contentChannelItem )
        {
            // Given a content channel item, convert it into a PodcastMessage and return it
            
            RockContext rockContext = new RockContext( );
            IQueryable<AttributeValue> attribValQuery = new AttributeValueService( rockContext ).Queryable( );

            // get this message's attributes
            var itemAttribValList = attribValQuery.Where( av => av.EntityId == contentChannelItem.Id && av.Attribute.EntityTypeQualifierColumn == "ContentChannelTypeId" )
                                                    .Select( av => new { AttributeKey = av.Attribute.Key, av.Value } )
                                                    .AsNoTracking( )
                                                    .ToList( );
                                    
            PodcastMessage message = new PodcastMessage( );
            message.Id = contentChannelItem.Id;
            message.Name = contentChannelItem.Title;
            message.Description = contentChannelItem.Content;
            message.Date = contentChannelItem.StartDateTime;
            
            //JHM TODO: Remove this once Roku and Apple TV are both updated to use the new "Active" attribute.
            message.Approved = true;

            // add all the attributes
            message.Attributes = new Dictionary<string, string>( );
            foreach ( var attribValue in itemAttribValList )
            {
                message.Attributes.Add( attribValue.AttributeKey, attribValue.Value );
            }
            return message;
        }

        public static PodcastCategory PodcastsAsModel( int podcastCategory, bool keepHierarchy = false, int numSeries = int.MaxValue )
        {
            return GetPodcastsByCategory( podcastCategory, keepHierarchy, numSeries );
        }

        public static PodcastSeries GetSeries( int seriesId )
        {
            // try to get the content channel that represents this series
            RockContext rockContext = new RockContext( );
            ContentChannelService contentChannelService = new ContentChannelService( rockContext );

            ContentChannel seriesContentChannel = contentChannelService.Queryable( ).Where( cc => cc.Id == seriesId ).Include( cc => cc.Items ).SingleOrDefault( );
            if( seriesContentChannel != null )
            {
                // convert it to a PodcastSeries and return it
                PodcastSeries series = ContentChannelToPodcastSeries( seriesContentChannel, int.MaxValue );
                return series;
            }
            
            // couldn't find it? return null
            return null;
        }

        public static PodcastMessage GetMessage( int messageId )
        {
            // try to get the content channel item that represents this message
            RockContext rockContext = new RockContext( );
            ContentChannelItemService contentChannelItemService = new ContentChannelItemService( rockContext );

            ContentChannelItem messageContentChannelItem = contentChannelItemService.Queryable( ).Where( cc => cc.Id == messageId ).SingleOrDefault( );
            if( messageContentChannelItem != null )
            {
                // convert it to a PodcastMessage and return it
                PodcastMessage message = ContentChannelItemToPodcastMessage( messageContentChannelItem );
                return message;
            }
            
            // couldn't find it? return null
            return null;
        }
        
        // Helper class for storing a Content Channel with an associated Attribute Value
        public class ContentChannelWithAttrib
        {
            public ContentChannel ContentChannel { get; set; }
            public string AttribValue { get; set; }
        }

        // Interface so that PodcastCategories can have either Series or Categories as children.
        public interface IPodcastNode : Rock.Lava.ILiquidizable
        {
            DateTime? Date { get; set; }
        }

        public class PodcastCategory : IPodcastNode
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<IPodcastNode> Children { get; set; }
            public DateTime? Date { get; set; }


            public PodcastCategory( string name, int id )
            {
                Name = name;
                Id = id;
                Children = new List<IPodcastNode>( );
            }



            // Liquid Methods
            [JsonIgnore]
            [Rock.Data.LavaIgnore]
            public List<string> AvailableKeys
            {
                get
                {
                    var availableKeys = new List<string> { "Id", "Name", "Children" };
                                            
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
                       case "Id": return Id;
                       case "Name": return Name;
                       case "Children": return Children;
                   }

                    return null;
                }
            }
            
            public bool ContainsKey( object key )
            {
                var additionalKeys = new List<string> { "Id", "Name", "Children" };
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
            public int Id { get; set; }
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
                    var availableKeys = new List<string> { "Id", "Name", "Description", "Date", "Attributes", "Messages" };

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
                       case "Id": return Id;
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
                var additionalKeys = new List<string> { "Id", "Name", "Description", "Date", "Attributes", "Messages" };
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

        public class PodcastMessage : IPodcastNode
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime? Date { get; set; }
            public Dictionary<string, string> Attributes { get; set; }
            public bool Approved { get; set; }

            // Liquid Methods
            [JsonIgnore]
            [Rock.Data.LavaIgnore]
            public List<string> AvailableKeys
            {
                get
                {
                    var availableKeys = new List<string> { "Id", "Name", "Description", "Date", "Attributes", "Approved" };
                    
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
                       case "Id": return Id;
                       case "Name": return Name;
                       case "Description": return Description;
                       case "Date": return Date;
                       case "Attributes": return Attributes;
                       case "Approved": return Approved;
                   }

                    return null;
                }
            }
            
            public bool ContainsKey( object key )
            {
                var additionalKeys = new List<string> { "Id", "Name", "Description", "Date", "Attributes", "Approved" };
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
