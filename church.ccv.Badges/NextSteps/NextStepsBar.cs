﻿using Rock;
using Rock.Attribute;
using Rock.Web.Cache;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace church.ccv.Badges.NextSteps
{
    [Description( "Displays the NextSteps Bar for the specific person." )]
    [Export( typeof( Rock.PersonProfile.BadgeComponent ) )]
    [ExportMetadata( "ComponentName", "NextSteps Bar" )]

    [LinkedPage("Baptism Registration Page", "The page to link to for registering for baptism.")]
    [IntegerField("Baptism Event Id", "The event id to use for pulling upcoming baptisms.")]
    [IntegerField("Shared Story WorkflowType Id", "The workflow type used for sharing a story.")]
    [LinkedPage( "Connection Group Registration Page", "The page to link to for registering for connection group." )]
    [LinkedPage( "Next Step Group Registration Page", "The page to link to for registering for a next step group." )]
    [LinkedPage("Group List Page", "The page to list all of the groups of a certain type")]
    [LinkedPage("Group Details Page", "The group details page.")]
    [LinkedPage("Serving Connection Page", "The page to use for creating new serving connections.")]
    [LinkedPage( "Young Adult Group Registration Page", "The page to link to for registering for a young adult group." )]
    [LinkedPage( "Next Gen Group Registration Page", "The page to link to for registering for a next gen group." )]
    [LinkedPage( "Shared Story Review Page", "The page used for reviewing a Shared Story." )]
    public class NextStepsBar : Rock.PersonProfile.BadgeComponent
    {
        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="badge">The badge.</param>
        /// <param name="writer">The writer.</param>
        public override void Render( PersonBadgeCache badge, System.Web.UI.HtmlTextWriter writer )
        {
            int baptismRegistrationPageId = GetPageIdFromLinkedPageAttribute( "BaptismRegistrationPage", badge);
            int connectionGroupRegistrationPageId = GetPageIdFromLinkedPageAttribute( "ConnectionGroupRegistrationPage", badge );
            int nextGenGroupRegistrationPageId = GetPageIdFromLinkedPageAttribute( "NextGenGroupRegistrationPage", badge );
            int nextStepGroupRegistrationPageId = GetPageIdFromLinkedPageAttribute( "NextStepGroupRegistrationPage", badge );
            int servingConnectionPageId = GetPageIdFromLinkedPageAttribute( "ServingConnectionPage", badge );
            int groupDetailsPageId = GetPageIdFromLinkedPageAttribute( "GroupDetailsPage", badge );
            int groupListPageId = GetPageIdFromLinkedPageAttribute( "GroupListPage", badge );
            int youngAdultGroupRegistrationPageId = GetPageIdFromLinkedPageAttribute( "YoungAdultGroupRegistrationPage", badge );
            int sharedStoryReviewPage = GetPageIdFromLinkedPageAttribute( "SharedStoryReviewPage", badge );//329;
            string sharedStoryWorkflowTypeId = GetAttributeValue(badge, "SharedStoryWorkflowTypeId");//163;

            writer.Write( string.Format( @"<div class='badge-group badge-group-steps js-badge-group-steps badge-id-{0}'>
                <a class='badge badge-baptism badge-icon step-nottaken' data-toggle='tooltip' data-original-title='{1} is not baptized' data-container='body' href='/page/{4}?PersonGuid={2}&EventItemId={3}'>
                    <i class='icon ccv-baptism'></i>
                </a>
                <div class='badge badge-worship badge-icon step-nottaken' data-toggle='tooltip' data-original-title='{1} is not an eRA' data-container='body'>
                    <i class='icon ccv-worship'></i>
                </div>
                <a class='badge badge-connect badge-icon step-nottaken' data-toggle='tooltip' data-original-title='{1} is not connected to a neighborhood group' data-container='body' href='/page/{5}?PersonGuid={2}'>
                    <i class='icon ccv-connect'></i>
                </a>
                <a class='badge badge-serve badge-icon step-nottaken' data-toggle='tooltip' data-original-title='{1} is not serving (Click for more options)' data-container='body' href='/page/{6}?PersonGuid={2}'>
                    <i class='icon ccv-serve'></i>
                </a>
                <div class='badge badge-tithe badge-icon step-nottaken' data-toggle='tooltip' data-original-title='{1} is not giving' data-container='body'>
                    <i class='icon ccv-tithe'></i>
                </div>
                <a class='badge badge-coach badge-icon step-nottaken' data-toggle='tooltip' data-original-title='{1} is not coaching' data-container='body' href='/page/{7}?PersonGuid={2}'>
                    <i class='icon ccv-coach'></i>
                </a>
                <a class='badge badge-share badge-icon step-nottaken' data-toggle='tooltip' data-original-title='{1} has not shared a story' data-container='body' href='/WorkflowEntry/{8}?PersonId={9}&Internal=True'>
                    <i class='icon ccv-share'></i>
                </a>
            </div>", 
                badge.Id // 0
                , Person.NickName.EncodeHtml() // 1
                , Person.Guid // 2
                , GetAttributeValue(badge, "BaptismEventId") // 3
                , baptismRegistrationPageId // 4
                , connectionGroupRegistrationPageId // 5
                , servingConnectionPageId // 6
                , nextStepGroupRegistrationPageId // 7
                , sharedStoryWorkflowTypeId // 8
                , Person.Id // 9
            ) );
            
            writer.Write( string.Format(
                @"
                    <script>
                    Sys.Application.add_load(function () {{

    var firstName = '{0}';

    var servingConnectionPageId = '{4}';

    var connectionGroupRegistrationPage = '{5}';

    var groupDetailPageId = '{6}';
    var groupListPageId = '{7}';
    var nextStepGroupRegistrationPage = '{8}';
    var youngAdultGroupRegistrationPage = '{9}';
    var nextGenGroupRegistrationPage = '{10}';

    $.ajax({{
        type: 'GET',
        url: Rock.settings.get('baseUrl') + 'api/CCV/Badges/StepsBar/{1}',
        statusCode: {{
            200: function (data, status, xhr) {{

                var $badge = $('.js-badge-group-steps.badge-id-{3}');

                // baptism
                if (data.BaptismResult.BaptismStatus == 2) {{
                    var baptismRegistrationDate = new Date(data.BaptismResult.BaptismRegistrationDate);
                    var baptismRegistrationDateFormatted = (baptismRegistrationDate.getMonth() + 1) + '/' + baptismRegistrationDate.getDate() + '/' + baptismRegistrationDate.getFullYear();

                    $badge.find('.badge-baptism').attr('data-original-title', firstName + ' is registered to be baptized on ' + baptismRegistrationDateFormatted );
                    $badge.find('.badge-baptism').removeClass('step-nottaken');
                    $badge.find('.badge-baptism').addClass('step-partial');
                }} else if (data.BaptismResult.BaptismStatus == 1) {{
                    var baptismDateFormatted = data.BaptismResult.BaptismDateFormatted;

                    $badge.find('.badge-baptism').removeClass('step-nottaken');
                    $badge.find('.badge-baptism').attr('data-original-title', firstName + ' was baptized on ' + baptismDateFormatted + '');
                }}

                // worship
                if (data.IsWorshipper) {{
                    $badge.find('.badge-worship').removeClass('step-nottaken');
                    $badge.find('.badge-worship').attr('data-toggle', 'tooltip');
                    $badge.find('.badge-worship').attr('data-original-title', firstName + ' is an eRA');
                    $badge.find('.badge-worship').attr('data-container', 'body');
                    $badge.find('.badge-worship').tooltip();
                }}

                // connect
                if (data.ConnectionResult.ConnectionStatus != 1) {{
                    $badge.find( '.badge-connect' ).removeClass( 'step-nottaken' );
                }}

                // create content for popover
                var popoverContent = firstName;
                if (data.ConnectionResult.Groups.length > 0) {{
                    popoverContent = popoverContent + "" is in the following connection groups: <ul styling='padding-left: 20px;' > "";

                    $.each( data.ConnectionResult.Groups, function( index, group ) {{

                        popoverContent = popoverContent + ""<li><a href='/page/"" + groupDetailPageId + ""?GroupId="" + group.GroupId + ""'>"" + group.GroupName + ""</a> <small>"" + group.Role + ""</small></li>"";

                        // only display 2
                        if ( index == 1 )
                        {{
                            return false;
                        }}
                    }});

                    var popoverContent = popoverContent + ""</ul>""
                }} else {{
                    popoverContent = popoverContent + "" is not in any connection groups."";
                }}

                // check for more than two groups
                if ( data.ConnectionResult.Groups.length > 2 )
                {{
                    var moreCount = data.ConnectionResult.Groups.length - 2;

                    if ( moreCount == 1 )
                    {{
                        popoverContent = popoverContent + ""<p>and <a href='/page/"" + groupListPageId + ""?PersonId="" + ""{2}'> "" + moreCount + "" other</a></p>"";
                    }}
                    else {{
                        popoverContent = popoverContent + ""<p>and <a href='/page/"" + groupListPageId + ""?PersonId="" + ""{2}'> "" + moreCount + "" others</a></p>"";
                    }}
                }}

                if ( data.IsAdult == true )
                {{
                    var popoverContent = popoverContent + ""<p class='margin-b-none'><a href='/page/"" + connectionGroupRegistrationPage + ""?PersonGuid={1}' class='btn btn-primary btn-block btn-xs'>Find NH Group</a></p>"";
                    var popoverContent = popoverContent + ""<p class='margin-b-none margin-t-sm'><a href='/page/"" + youngAdultGroupRegistrationPage + ""?PersonGuid={1}' class='btn btn-primary btn-block btn-xs'>Find YA Group</a></p>"";
                }}
                else
                {{
                    var popoverContent = popoverContent + ""<p class='margin-b-none margin-t-sm'><a href='/page/"" + nextGenGroupRegistrationPage + ""?PersonGuid={1}' class='btn btn-primary btn-block btn-xs'>Find Students Group</a></p>"";
                }}

                if (data.ConnectionResult.ConnectionStatus == 2) {{
                    $badge.find('.badge-connect').addClass('step-partial');
                }}

                $badge.find( '.badge-connect' ).attr( 'data-toggle', 'popover' );
                $badge.find( '.badge-connect' ).attr( 'data-container', 'body' );
                $badge.find( '.badge-connect' ).attr( 'data-content', popoverContent );
                
                if (data.ConnectionResult.ConnectionStatus != 1) {{                
                    var connectSinceDate = new Date(data.ConnectionResult.ConnectedSince);
                    var connectSinceDateFormatted = (connectSinceDate.getMonth() + 1) + '/' + connectSinceDate.getDate() + '/' + connectSinceDate.getFullYear();

                    $badge.find( '.badge-connect' ).attr( 'data-original-title', firstName + ' is in a connection group (earliest active group ' + connectSinceDateFormatted + ') &nbsp;&nbsp;<i class=""fa fa-mouse-pointer""></i>' );
                }}

                var connectPopoverIsOpen = false;
                var connectPopoverTitle = $badge.find( '.badge-connect' ).attr( 'data-original-title');

                $badge.find( '.badge-connect' ).popover({{
                    html: true,
                    placement: 'top',
                    trigger: 'manual'
                }});

                // disable the anchor tag
                $badge.find( '.badge-connect' ).on( ""click"", function( e ) {{
                    e.preventDefault();
                }});

                // fancy pants to allow the tooltip and popover to work on the same control
                $badge.find( '.badge-connect' ).on( 'click', function() {{
                    if ( connectPopoverIsOpen )
                    {{
                        $badge.find( '.badge-connect' ).popover( 'hide' );
                        connectPopoverIsOpen = false;
                        $badge.find( '.badge-connect' ).attr( 'data-original-title', connectPopoverTitle );
                    }}
                    else {{
                        $badge.find( '.badge-connect' ).attr( 'data-original-title', '' );
                        $badge.find( '.badge-connect' ).popover( 'show' );
                        connectPopoverIsOpen = true;
                        $badge.find( '.badge-connect' ).tooltip( 'hide' );
                    }}
                }});

                // tithing
                if (data.IsTithing) {{
                    $badge.find('.badge-tithe').removeClass('step-nottaken');
                    $badge.find( '.badge-tithe' ).attr( 'data-original-title', firstName + ' is giving' );
                }}

                // serving
                if (data.ServingResult.ServingStatus != 0) {{

                    // create content for popover
                    
                    // the text will either be affirmative or 'pending' depending on their status.
                    var servingPopupText = "" is on the following serving teams: ""
                    var servingTooltipText = "" is serving ""

                    if (data.ServingResult.ServingStatus == 1) {{
                        servingPopupText = "" is pending on the following serving teams: ""
                        servingTooltipText = "" is pending serving ""
                        $badge.find('.badge-serve').addClass('step-partial');
                    }}

                    var popoverContent = firstName + servingPopupText + ""<ul styling='padding-left: 20px;'>"";

                    // disable the anchor tag
                    $badge.find( '.badge-serve' ).on( ""click"", function( e ) {{
                        e.preventDefault();
                    }});

                    $.each( data.ServingResult.Groups, function (index, group)
                    {{
                        popoverContent = popoverContent + ""<li><a href='/page/"" + groupDetailPageId + ""?GroupId="" + group.GroupId + ""'>"" + group.GroupName + ""</a></li>"";

                        // only display 2
                        if ( index == 1 )
                        {{
                            return false;
                        }}
                    }});

                    var popoverContent = popoverContent + ""</ul>""

                    // check for more than two groups
                    if (data.ServingResult.Groups.length > 2) {{
                        var moreCount = data.ServingResult.Groups.length - 2;

                        if (moreCount == 1) {{
                            popoverContent = popoverContent + ""<p>and <a href='/page/"" + groupListPageId + ""?PersonId="" + ""{2}'> "" + moreCount + "" other</a></p>"";
                        }}
                        else {{
                            popoverContent = popoverContent + ""<p>and <a href='/page/"" + groupListPageId + ""?PersonId="" + ""{2}'> "" + moreCount + "" others</a></p>"";
                        }}
                    }}

                    var popoverContent = popoverContent + ""<p class='margin-b-none'><a href='/page/"" + servingConnectionPageId + ""?PersonGuid={1}' class='btn btn-primary btn-block btn-xs'>Connect</a></p>"";

                    $badge.find('.badge-serve').removeClass('step-nottaken');
                    $badge.find('.badge-serve').attr('data-toggle', 'popover');
                    $badge.find('.badge-serve').attr('data-container', 'body');
                    $badge.find('.badge-serve').attr('data-content', popoverContent);

                    var servingSinceDate = new Date(data.ServingResult.ServingSince);
                    var servingSinceDateFormatted = (servingSinceDate.getMonth() + 1) + '/' + servingSinceDate.getDate() + '/' + servingSinceDate.getFullYear();

                    
                    $badge.find('.badge-serve').attr('data-original-title', firstName + servingTooltipText + ' (earliest active group ' + servingSinceDateFormatted + ')&nbsp;&nbsp;<i class=""fa fa-mouse-pointer""></i>');

                    var servingPopoverIsOpen = false;

                    $badge.find('.badge-serve').popover({{
                        html: true,
                        placement: 'top',
                        trigger: 'manual'
                    }});
                    
                    // fancy pants to allow the tooltip and popover to work on the same control
                    $badge.find('.badge-serve').on('click', function ()
                    {{
                        if ( servingPopoverIsOpen )
                        {{
                            $badge.find( '.badge-serve' ).popover( 'hide' );
                            servingPopoverIsOpen = false;
                            $badge.find('.badge-serve').attr('data-original-title', firstName + servingTooltipText + ' (earliest active group ' + servingSinceDateFormatted + ')&nbsp;&nbsp;<i class=""fa fa-mouse-pointer""></i>');
                        }}
                        else {{
                            $badge.find( '.badge-serve' ).attr( 'data-original-title', '' );
                            $badge.find( '.badge-serve' ).popover( 'show' );
                            servingPopoverIsOpen = true;
                            $badge.find( '.badge-serve' ).tooltip( 'hide' );
                        }}
                    }});
                }}

                // sharing
                if (data.SharedStoryResult.SharedStory) {{

                    // create content for popover
                    var popoverContent = firstName + "" has shared the following stories: <ul styling='padding-left: 20px;'>"";

                    // disable the anchor tag
                    $badge.find( '.badge-share' ).on( ""click"", function( e ) {{
                        e.preventDefault();
                    }});

                    $.each( data.SharedStoryResult.SharedStoryIds, function (index, storyId)
                    {{
                        popoverContent = popoverContent + ""<li><a href='/page/"" + {11} + ""?workflowId="" + storyId + ""'>"" + ""Story "" + (index + 1) + ""</a></li>"";

                        // display 3
                        if ( index == 2 )
                        {{
                            return false;
                        }}
                    }});

                    var popoverContent = popoverContent + ""</ul>"";

                    // check for more than three stories
                    if (data.SharedStoryResult.SharedStoryIds.length > 3) {{
                        popoverContent = popoverContent + ""<p>For more stories, see the Extended Attributes tab.</p>"";
                    }}

                    var popoverContent = popoverContent + ""<p class='margin-b-none margin-t-sm'><a href='/WorkflowEntry/"" + {12} + ""?PersonId={2}&Internal=True' class='btn btn-primary btn-block btn-xs'>Submit Story</a></p>"";

                    $badge.find('.badge-share').removeClass('step-nottaken');
                    $badge.find('.badge-share').attr('data-toggle', 'popover');
                    $badge.find('.badge-share').attr('data-container', 'body');
                    $badge.find('.badge-share').attr('data-content', popoverContent);

                    $badge.find('.badge-share').attr('data-original-title', firstName + ' has shared ' + data.SharedStoryResult.SharedStoryIds.length + ' stories. &nbsp;&nbsp;<i class=""fa fa-mouse-pointer""></i>');

                    var sharePopoverIsOpen = false;

                    $badge.find('.badge-share').popover({{
                        html: true,
                        placement: 'top',
                        trigger: 'manual'
                    }});

                    // fancy pants to allow the tooltip and popover to work on the same control
                    $badge.find('.badge-share').on('click', function ()
                    {{
                        if ( sharePopoverIsOpen )
                        {{
                            $badge.find( '.badge-share' ).popover( 'hide' );
                            sharePopoverIsOpen = false;
                            $badge.find('.badge-share').attr('data-original-title', firstName + ' has shared ' + data.SharedStoryResult.SharedStoryIds.length + ' stories. &nbsp;&nbsp;<i class=""fa fa-mouse-pointer""></i>');
                        }}
                        else {{
                            $badge.find( '.badge-share' ).attr( 'data-original-title', '' );
                            $badge.find( '.badge-share' ).popover( 'show' );
                            sharePopoverIsOpen = true;
                            $badge.find( '.badge-share' ).tooltip( 'hide' );
                        }}
                    }});
                }}
                else {{
                    $badge.find( '.badge-share' ).attr( 'data-original-title', firstName + ' has not shared a story.' );
                }}

                // coaching
                if (data.CoachingResult.IsCoaching) {{

                    // create content for popover
                    var popoverContent = firstName + "" is in the following coaching groups: <ul styling='padding-left: 20px;'>"";

                    // disable the anchor tag
                    $badge.find( '.badge-coach' ).on( ""click"", function( e ) {{
                        e.preventDefault();
                    }});

                    $.each( data.CoachingResult.Groups, function (index, group)
                    {{
                        popoverContent = popoverContent + ""<li><a href='/page/"" + groupDetailPageId + ""?GroupId="" + group.GroupId + ""'>"" + group.GroupName + ""</a></li>"";

                        // only display 2
                        if ( index == 1 )
                        {{
                            return false;
                        }}
                    }});

                    var popoverContent = popoverContent + ""</ul>"";

                    // check for more than two groups
                    if (data.CoachingResult.Groups.length > 2) {{
                        var moreCount = data.CoachingResult.Groups.length - 2;

                        if (moreCount == 1) {{
                            popoverContent = popoverContent + ""<p>and "" + moreCount + "" other (see groups tab for details)</p>"";
                        }}
                        else {{
                            popoverContent = popoverContent + ""<p>and "" + moreCount + "" others (see groups tab for details)</p>"";
                        }}
                    }}

                    var popoverContent = popoverContent + ""<p class='margin-b-none margin-t-sm'><a href='/page/"" + nextStepGroupRegistrationPage + ""?PersonGuid={1}' class='btn btn-primary btn-block btn-xs'>Find NS Group</a></p>"";

                    $badge.find('.badge-coach').removeClass('step-nottaken');
                    $badge.find('.badge-coach').attr('data-toggle', 'popover');
                    $badge.find('.badge-coach').attr('data-container', 'body');
                    $badge.find('.badge-coach').attr('data-content', popoverContent);

                    var coachingSinceDate = new Date(data.CoachingResult.CoachingSince);
                    var coachingSinceDateFormatted = (coachingSinceDate.getMonth() + 1) + '/' + coachingSinceDate.getDate() + '/' + coachingSinceDate.getFullYear();

                    $badge.find('.badge-coach').attr('data-original-title', firstName + ' is in a coaching group (earliest active group  ' + coachingSinceDateFormatted + ') &nbsp;&nbsp;<i class=""fa fa-mouse-pointer""></i>');

                    var coachPopoverIsOpen = false;

                    $badge.find('.badge-coach').popover({{
                        html: true,
                        placement: 'top',
                        trigger: 'manual'
                    }});

                    // fancy pants to allow the tooltip and popover to work on the same control
                    $badge.find('.badge-coach').on('click', function ()
                    {{
                        if ( coachPopoverIsOpen )
                        {{
                            $badge.find( '.badge-coach' ).popover( 'hide' );
                            coachPopoverIsOpen = false;
                            $badge.find('.badge-coach').attr('data-original-title', firstName + ' is in a coaching group (earliest active group  ' + coachingSinceDateFormatted + ') &nbsp;&nbsp;<i class=""fa fa-mouse-pointer""></i>');
                        }}
                        else {{
                            $badge.find( '.badge-coach' ).attr( 'data-original-title', '' );
                            $badge.find( '.badge-coach' ).popover( 'show' );
                            coachPopoverIsOpen = true;
                            $badge.find( '.badge-coach' ).tooltip( 'hide' );
                        }}
                    }});
                }}
                else {{
                    $badge.find( '.badge-coach' ).attr( 'data-original-title', firstName + ' is not coaching.' );
                }}
            }}
        }},
    }});
}});
                    </script>",
                 Person.NickName.EncodeHtml(), // 0
                 Person.Guid.ToString(), // 1
                 Person.Id, // 2
                 badge.Id, // 3  
                 servingConnectionPageId, // 4
                 connectionGroupRegistrationPageId, // 5
                 groupDetailsPageId, // 6
                 groupListPageId, // 7
                 nextStepGroupRegistrationPageId, // 8
                 youngAdultGroupRegistrationPageId, // 9
                 nextGenGroupRegistrationPageId, //10
                 sharedStoryReviewPage, //11
                 sharedStoryWorkflowTypeId //12
            ));
        }

        private int GetPageIdFromLinkedPageAttribute(string attributeKey, PersonBadgeCache badge )
        {
            int pageId = -1;

            Guid pageGuid = Guid.Empty;
            if ( Guid.TryParse( GetAttributeValue( badge, attributeKey ), out pageGuid ) )
            {
                pageId = Rock.Web.Cache.PageCache.Read( pageGuid ).Id;
            }

            return pageId;
        }
    }
}