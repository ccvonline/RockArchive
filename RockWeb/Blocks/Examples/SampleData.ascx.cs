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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Examples
{
    /// <summary>
    /// Block that can load sample data into your Rock database.
    /// </summary>
    [DisplayName( "Rock Solid Church Sample Data" )]
    [Category( "Examples" )]
    [Description( "Loads the Rock Solid Church sample data into your Rock system." )]
    public partial class SampleData : Rock.Web.UI.RockBlock
    {
        #region Fields
        /// <summary>
        /// The Url to the sample data
        /// </summary>
        private static string _xmlFileUrl = "http://storage.rockrms.com/sampledata/sampledata.xml";

        /// <summary>
        /// Holds the Person Image binary file type.
        /// </summary>
        private static BinaryFileType _binaryFileType = new BinaryFileTypeService().Get( Rock.SystemGuid.BinaryFiletype.PERSON_IMAGE.AsGuid() );

        /// <summary>
        /// The storage type to use for the people photos.
        /// </summary>
        private static EntityTypeCache _storageEntityType = EntityTypeCache.Read( Rock.SystemGuid.EntityType.STORAGE_PROVIDER_DATABASE.AsGuid() );

        /// <summary>
        /// Percent of additional time someone tends to NOT attend during the summer months (7-9)
        /// </summary>
        private int summerPercentFactor = 30;

        /// <summary>
        /// A random number generator for use when calculating random attendance data.
        /// </summary>
        private static Random _random = new Random( (int)DateTime.Now.Ticks );

        /// <summary>
        /// The number of characters (length) that security codes should be.
        /// </summary>
        private static int _securityCodeLength = 5;

        /// <summary>
        /// A little lookup list for finding a group/location appropriate for the child's attendance data
        /// </summary>
        private static List<ClassGroupLocation> _classes = new List<ClassGroupLocation>
        {
            new ClassGroupLocation { GroupId = 25, LocationId = 4, MinAge =  0.0, MaxAge = 3.0,   Name = "Nursery - Bunnies Room"  },
            new ClassGroupLocation { GroupId = 26, LocationId = 5, MinAge =  0.0, MaxAge = 3.99,  Name = "Crawlers/Walkers - Kittens Room" },
            new ClassGroupLocation { GroupId = 27, LocationId = 6, MinAge =  0.0, MaxAge = 5.99,  Name = "Preschool - Puppies Room" },
            new ClassGroupLocation { GroupId = 28, LocationId = 7, MinAge =  4.75, MaxAge = 8.75, Name = "Grades K-1 - Bears Room" },
            new ClassGroupLocation { GroupId = 29, LocationId = 8, MinAge =   6.0, MaxAge = 10.99, Name = "Grades 2-3 - Bobcats Room" },
            new ClassGroupLocation { GroupId = 30, LocationId = 9, MinAge =   8.0, MaxAge = 13.99, Name = "Grades 4-6 - Outpost Room" },
            new ClassGroupLocation { GroupId = 31, LocationId = 10, MinAge = 12.0, MaxAge = 15.0,  Name = "Grades 7-8 - Warehouse" },
            new ClassGroupLocation { GroupId = 32, LocationId = 11, MinAge = 13.0, MaxAge = 19.0,  Name = "Grades 9-12 - Garage" },
        };

        /// <summary>
        /// Holds a cached copy of the "start time" DateTime for any scheduleIds this block encounters.
        /// </summary>
        private Dictionary<int, DateTime> _scheduleTimes = new Dictionary<int, DateTime>();

        /// <summary>
        /// Holds a cached copy of the Id for each person Guid
        /// </summary>
        private Dictionary<Guid, int> _peopleDictionary = new Dictionary<Guid, int>();

        /// <summary>
        /// Holds a cached copy of the location Id for each family Guid
        /// </summary>
        private Dictionary<Guid, int> _familyLocationDictionary = new Dictionary<Guid, int>();

        /// <summary>
        /// Magic kiosk Id used for attendance data.
        /// </summary>
        private static int _kioskDeviceId = 2;

        #endregion

        #region Properties

        // used for public / protected properties

        #endregion

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );
            Server.ScriptTimeout = 300;
            ScriptManager.GetCurrent(Page).AsyncPostBackTimeout = 300;
        }

        #endregion

        #region Events

        /// <summary>
        /// This is the entry point for when the user clicks the "load data" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bbtnLoadData_Click( object sender, EventArgs e )
        {
            string saveFile = Path.Combine( MapPath( "~" ), "sampledata1.xml" );

            try
            {
                if ( DownloadFile( _xmlFileUrl, saveFile ) )
                {
                    ProcessXml( saveFile );
                    nbMessage.Visible = true;
                    nbMessage.Title = "Success";
                    nbMessage.NotificationBoxType = NotificationBoxType.Success;
                    nbMessage.Text = string.Format( @"<p>Happy tire-kicking! The data is in your database. Hint: try <a href='{0}'>searching for the Decker family</a>.</p>
                        <p>Here are some of the things you'll find in the sample data:</p>{1}"
                        , ResolveRockUrl( "~/Person/Search/name/Decker" ), GetStories( saveFile ) );
                    bbtnLoadData.Visible = false;
                }
            }
            catch ( Exception ex )
            {
                nbMessage.Visible = true;
                nbMessage.Title = "Oops!";
                nbMessage.NotificationBoxType = NotificationBoxType.Danger;
                nbMessage.Text = string.Format( "That wasn't supposed to happen.  The error was:<br/>{0}<br/>{1}<br/>{2}", ex.Message.ConvertCrLfToHtmlBr(), FlattenInnerExceptions(ex.InnerException),
                    ex.StackTrace.ConvertCrLfToHtmlBr() );
            }

            if ( File.Exists( saveFile ) )
            {
                File.Delete( saveFile );
            }
        }

        /// <summary>
        /// Extract the stories out of the XML and put them on the results page.
        /// </summary>
        /// <param name="saveFile"></param>
        /// <returns></returns>
        protected string GetStories( string saveFile )
        {
            var xdoc = XDocument.Load( saveFile );
            StringBuilder sb = new StringBuilder();
            sb.Append( "<ul>" );
            foreach ( var comment in xdoc.Element( "data" ).DescendantNodes().OfType<XComment>() )
            {
                sb.AppendFormat( "<li>{0}</li>", comment.ToString().Replace( "<!--", "").Replace( "-->", "" ) );
            }
            sb.Append( "</ul>" );
            return sb.ToString();
        }

        /// <summary>
        /// Handles the BlockUpdated event of the PageLiquid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Download the given fileUrl and store it at the fileOutput.
        /// </summary>
        /// <param name="fileUrl">The file Url to fetch.</param>
        /// <param name="fileOutput">The full path location to store the file.</param>
        /// <returns></returns>
        private bool DownloadFile( string fileUrl, string fileOutput )
        {
            bool isSuccess = false;
            try
            {
                using ( WebClient client = new WebClient() )
                {
                    client.DownloadFile( fileUrl, fileOutput );
                }
                isSuccess = true;
            }
            catch ( WebException ex )
            {
                nbMessage.Text = string.Format( "While trying to fetch {0}, {1} ", fileUrl, ex.Message);
                nbMessage.Visible = true;
            }

            return isSuccess;
        }

        /// <summary>
        /// Process all the data in the XML file; deleting stuff and then adding stuff.
        /// as per https://github.com/SparkDevNetwork/Rock/wiki/z.-Rock-Solid-Demo-Church-Specification#wiki-xml-data
        /// </summary>
        /// <param name="sampleXmlFile"></param>
        private void ProcessXml( string sampleXmlFile )
        {
            var xdoc = XDocument.Load( sampleXmlFile );

            RockTransactionScope.WrapTransaction( () =>
            {
                using ( new UnitOfWorkScope() )
                {
                    var elemFamilies = xdoc.Element( "data" ).Element( "families" );
                    var elemGroups = xdoc.Element( "data" ).Element( "groups" );
                    var elemRelationships = xdoc.Element( "data" ).Element( "relationships" );

                    // First we'll clean up by deleting any previously created data such as
                    // families, addresses, people, photos, attendance data, etc.
                    DeleteExistingGroups( elemGroups );
                    DeleteExistingFamilyData( elemFamilies );

                    // Now we can add the families (and people) and then groups.
                    AddFamilies( elemFamilies );
                    AddRelationships( elemRelationships );
                    AddGroups( elemGroups );
                }
            } );
        }
   
        /// <summary>
        /// Adds a KnownRelationship record between the two supplied Guids with the given 'is' relationship type:
        ///     
        ///     Role / inverse Role
        ///     ================================
        ///     step-parent     / step-child
        ///     grandparent     / grandchild
        ///     previous-spouse / previous-spouse
        ///     can-check-in    / allow-check-in-by
        ///     parent          / child
        ///     sibling         / sibling
        ///     invited         / invited-by
        ///     related         / related
        ///     
        /// ...for xml such as:
        /// <relationships>
        ///     <relationship a="Ben" personGuid="3C402382-3BD2-4337-A996-9E62F1BAB09D"
        ///     has="step-parent" forGuid="3D7F6605-3666-4AB5-9F4E-D7FEBF93278E" name="Brian" />
        ///  </relationships>
        ///  
        /// </summary>
        /// <param name="elemRelationships"></param>
        private void AddRelationships( XElement elemRelationships )
        {
            if ( elemRelationships == null )
            {
                return;
            }

            Guid ownerRoleGuid = Rock.SystemGuid.GroupRole.GROUPROLE_KNOWN_RELATIONSHIPS_OWNER.AsGuid();
            Guid knownRelationshipsGroupTypeGuid = Rock.SystemGuid.GroupType.GROUPTYPE_KNOWN_RELATIONSHIPS.AsGuid();

            var groupTypeRoles = new GroupTypeRoleService().Queryable("GroupType")
                .Where( r => r.GroupType.Guid == knownRelationshipsGroupTypeGuid ).ToList();

            // We have to create (or fetch existing) two groups for each relationship, adding the
            // other person as a member of that group with the appropriate GroupTypeRole (GTR):
            //   * a group with person as owner (GTR) and forPerson as type/role (GTR) 
            //   * a group with forPerson as owner (GTR) and person as inverse-type/role (GTR)
 
            foreach ( var elemRelationship in elemRelationships.Elements( "relationship" ) )
            {
                // skip any illegally formatted items
                if ( elemRelationship.Attribute( "personGuid" ) == null || elemRelationship.Attribute( "forGuid" ) == null ||
                    elemRelationship.Attribute( "has" ) == null )
                {
                    continue;
                }

                Guid personGuid = elemRelationship.Attribute( "personGuid" ).Value.Trim().AsGuid();
                Guid forGuid = elemRelationship.Attribute( "forGuid" ).Value.Trim().AsGuid();
                int ownerPersonId = _peopleDictionary[personGuid];
                int forPersonId = _peopleDictionary[forGuid];

                string rType = elemRelationship.Attribute( "has" ).Value.Trim();
  
                var memberService = new GroupMemberService();
                int roleId = -1;

                switch ( rType )
                {
                    case "step-parent":
                        roleId = groupTypeRoles.Where( r => r.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_KNOWN_RELATIONSHIPS_STEP_PARENT.AsGuid() )
                            .Select( r => r.Id ).FirstOrDefault();
                        break;

                    case "step-child":
                        roleId = groupTypeRoles.Where( r => r.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_KNOWN_RELATIONSHIPS_STEP_CHILD.AsGuid() )
                            .Select( r => r.Id ).FirstOrDefault();
                        break;

                    case "can-check-in":
                        roleId = groupTypeRoles.Where( r => r.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_KNOWN_RELATIONSHIPS_CAN_CHECK_IN.AsGuid() )
                            .Select( r => r.Id ).FirstOrDefault();
                        break;

                    case "allow-check-in-by":
                        roleId = groupTypeRoles.Where( r => r.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_KNOWN_RELATIONSHIPS_ALLOW_CHECK_IN_BY.AsGuid() )
                            .Select( r => r.Id ).FirstOrDefault();
                        break;

                    case "grandparent":
                        roleId = groupTypeRoles.Where( r => r.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_KNOWN_RELATIONSHIPS_GRANDPARENT.AsGuid() )
                            .Select( r => r.Id ).FirstOrDefault();
                        break;

                    case "grandchild":
                        roleId = groupTypeRoles.Where( r => r.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_KNOWN_RELATIONSHIPS_GRANDCHILD.AsGuid() )
                            .Select( r => r.Id ).FirstOrDefault();
                        break;

                    case "invited":
                        roleId = groupTypeRoles.Where( r => r.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_KNOWN_RELATIONSHIPS_INVITED.AsGuid() )
                            .Select( r => r.Id ).FirstOrDefault();
                        break;

                    case "invited-by":
                        roleId = groupTypeRoles.Where( r => r.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_KNOWN_RELATIONSHIPS_INVITED_BY.AsGuid() )
                            .Select( r => r.Id ).FirstOrDefault();
                        break;

                    case "previous-spouse":
                        roleId = groupTypeRoles.Where( r => r.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_KNOWN_RELATIONSHIPS_PREVIOUS_SPOUSE.AsGuid() )
                            .Select( r => r.Id ).FirstOrDefault();
                        break;

                    case "sibling":
                        roleId = groupTypeRoles.Where( r => r.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_KNOWN_RELATIONSHIPS_SIBLING.AsGuid() )
                            .Select( r => r.Id ).FirstOrDefault();
                        break;

                    case "parent":
                        roleId = groupTypeRoles.Where( r => r.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_KNOWN_RELATIONSHIPS_PARENT.AsGuid() )
                            .Select( r => r.Id ).FirstOrDefault();
                        break;

                    case "child":
                        roleId = groupTypeRoles.Where( r => r.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_KNOWN_RELATIONSHIPS_CHILD.AsGuid() )
                            .Select( r => r.Id ).FirstOrDefault();
                        break;

                    case "related":
                        roleId = groupTypeRoles.Where( r => r.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_IMPLIED_RELATIONSHIPS_RELATED.AsGuid() )
                            .Select( r => r.Id ).FirstOrDefault();
                        break;

                    default:
                        //throw new NotSupportedException( string.Format( "unknown relationship type {0}", elemRelationship.Attribute( "has" ).Value ) );
                        // just skip unknown relationship types
                        continue;
                }
                
                // find the person's KnownRelationship "owner" group
                var group = memberService.Queryable()
                    .Where( m =>
                    m.PersonId == ownerPersonId &&
                    m.GroupRole.Guid == ownerRoleGuid
                    )
                    .Select( m => m.Group )
                    .FirstOrDefault();

                // create it if it does not yet exist
                if ( group == null )
                {
                    var ownerRole = new GroupTypeRoleService().Get( ownerRoleGuid );
                    if ( ownerRole != null && ownerRole.GroupTypeId.HasValue )
                    {
                        var ownerGroupMember = new GroupMember();
                        ownerGroupMember.PersonId = ownerPersonId;
                        ownerGroupMember.GroupRoleId = ownerRole.Id;

                        group = new Group();
                        group.Name = ownerRole.GroupType.Name;
                        group.GroupTypeId = ownerRole.GroupTypeId.Value;
                        group.Members.Add( ownerGroupMember );

                        var groupService = new GroupService();
                        groupService.Add( group, CurrentPersonAlias );
                        groupService.Save( group, CurrentPersonAlias );

                        group = groupService.Get( group.Id );
                    }
                }

                // Now find (and add if not found) the forPerson as a member with the "has" role-type
                var groupMember = memberService.Queryable()
                    .Where( m =>
                        m.GroupId == group.Id &&
                        m.PersonId == forPersonId &&
                        m.GroupRoleId == roleId )
                    .FirstOrDefault();

                if ( groupMember == null )
                {
                    groupMember = new GroupMember()
                    {
                        GroupId = group.Id,
                        PersonId = forPersonId,
                        GroupRoleId = roleId,
                    };
                    memberService.Add( groupMember, CurrentPersonAlias );
                }

                memberService.Save( groupMember, CurrentPersonAlias );
                
                // now create thee inverse relationship
                var inverseGroupMember = memberService.GetInverseRelationship(
                    groupMember, createGroup: true, personAlias: CurrentPersonAlias );
                if ( inverseGroupMember != null )
                {
                    memberService.Save( inverseGroupMember, CurrentPersonAlias );
                }
            }
        }

        /// <summary>
        /// Handles adding families from the given XML element snippet
        /// </summary>
        /// <param name="elemFamilies"></param>
        private void AddFamilies( XElement elemFamilies )
        {
            if ( elemFamilies == null )
            {
                return;
            }

            // Next create the family along with its members.
            foreach ( var elemFamily in elemFamilies.Elements( "family" ) )
            {
                Guid guid = elemFamily.Attribute( "guid" ).Value.Trim().AsGuid();
                var familyMembers = BuildFamilyMembersFromXml( elemFamily.Element( "members" ) );

                GroupService groupService = new GroupService();

                Group family = groupService.SaveNewFamily( familyMembers, 1, savePersonAttributes: true, personAlias: CurrentPersonAlias );
                family.Guid = guid;

                // add the families address(es)
                AddFamilyAddresses( groupService, family, elemFamily.Element( "addresses" ) );

                // add their attendance data
                AddFamilyAttendance( family, elemFamily );

                // lastly, save the data and move to the next family
                groupService.Save( family, CurrentPersonAlias );

                foreach ( var p in family.Members )
                {
                    // Put the person's id into the people dictionary for later use.
                    if ( !_peopleDictionary.ContainsKey( p.Person.Guid ) )
                    {
                        _peopleDictionary.Add( p.Person.Guid, p.PersonId );
                    }
                }
            }
        }

        /// <summary>
        /// Handles adding groups from the given XML element snippet.
        /// </summary>
        /// <param name="elemGroups"></param>
        private void AddGroups( XElement elemGroups )
        {
            // Add groups
            if ( elemGroups == null )
            {
                return;
            }

            GroupService groupService = new GroupService();

            // Next create the group along with its members.
            foreach ( var elemGroup in elemGroups.Elements( "group" ) )
            {
                Guid guid = elemGroup.Attribute( "guid" ).Value.Trim().AsGuid();
                String type = elemGroup.Attribute( "type" ).Value;
                Group group = new Group()
                {
                    Guid = guid,
                    Name = elemGroup.Attribute( "name" ).Value.Trim()
                };

                // skip any where there is no group type given -- they are invalid entries.
                if ( string.IsNullOrEmpty( elemGroup.Attribute( "type" ).Value.Trim() ) )
                {
                    return;
                }

                int? roleId;
                GroupTypeCache groupType;
                switch ( elemGroup.Attribute( "type" ).Value.Trim() )
                {
                    case "serving":
                        groupType = GroupTypeCache.Read( Rock.SystemGuid.GroupType.GROUPTYPE_SERVING_TEAM.AsGuid() );
                        group.GroupTypeId = groupType.Id;
                        roleId = groupType.DefaultGroupRoleId;
                        break;
                    case "smallgroup":
                        groupType = GroupTypeCache.Read( Rock.SystemGuid.GroupType.GROUPTYPE_SMALL_GROUP.AsGuid() );
                        group.GroupTypeId = groupType.Id;
                        roleId = groupType.DefaultGroupRoleId;
                        break;
                    default:
                        throw new NotSupportedException( string.Format( "unknown group type {0}", elemGroup.Attribute( "type" ).Value.Trim() ) );
                }

                if ( elemGroup.Attribute( "description" ) != null )
                {
                    group.Description = elemGroup.Attribute( "description" ).Value;
                }

                if ( elemGroup.Attribute( "parentGroupGuid" ) != null )
                {
                    var parentGroup = groupService.Get( elemGroup.Attribute( "parentGroupGuid" ).Value.AsGuid() );
                    group.ParentGroupId = parentGroup.Id;
                }

                // Set the group's meeting location
                if ( elemGroup.Attribute( "meetsAtHomeOfFamily") != null )
                {
                    int meetingLocationValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_MEETING_LOCATION.AsGuid() ).Id;
                    var groupLocation = new GroupLocation()
                    {
                        IsMappedLocation = false,
                        IsMailingLocation = false,
                        GroupLocationTypeValueId = meetingLocationValueId,
                        LocationId = _familyLocationDictionary[elemGroup.Attribute( "meetsAtHomeOfFamily" ).Value.AsGuid()],
                    };

                    // Set the group location's GroupMemberPersonId if given (required?)
                    if ( elemGroup.Attribute( "meetsAtHomeOfPerson" ) != null )
                    {
                        groupLocation.GroupMemberPersonId = _peopleDictionary[elemGroup.Attribute( "meetsAtHomeOfPerson" ).Value.AsGuid()];
                    }
                    group.GroupLocations.Add( groupLocation );
                }

                group.LoadAttributes();

                // Set the study topic
                if ( elemGroup.Attribute( "studyTopic" ) != null )
                {
                    group.SetAttributeValue( "StudyTopic", elemGroup.Attribute( "studyTopic" ).Value );
                }

                // Set the meeting time
                if ( elemGroup.Attribute( "meetingTime" ) != null )
                {
                    group.SetAttributeValue( "MeetingTime", elemGroup.Attribute( "meetingTime" ).Value );
                }

                // Add each person as a member
                foreach ( var elemPerson in elemGroup.Elements( "person" ) )
                {
                    Guid personGuid = elemPerson.Attribute( "guid" ).Value.Trim().AsGuid();

                    GroupMember groupMember = new GroupMember();
                    groupMember.GroupMemberStatus = GroupMemberStatus.Active;
                    groupMember.GroupRoleId = roleId ?? -1;
                    groupMember.PersonId = _peopleDictionary[personGuid];
                    group.Members.Add( groupMember );
                }

                groupService.Add( group );
                groupService.Save( group, CurrentPersonAlias );
                group.SaveAttributeValues( CurrentPersonAlias );

            }
        }

        /// <summary>
        /// Deletes the family's addresses, phone numbers, photos, viewed records, and people.
        /// TODO: delete attendance codes for attendance data that's about to be deleted when
        /// we delete the person record.
        /// </summary>
        /// <param name="families"></param>
        private void DeleteExistingFamilyData( XElement families )
        {
            PersonService personService = new PersonService();
            PhoneNumberService phoneNumberService = new PhoneNumberService();
            PersonViewedService personViewedService = new PersonViewedService();
            BinaryFileService binaryFileService = new BinaryFileService();

            foreach ( var elemFamily in families.Elements( "family" ) )
            {
                Guid guid = elemFamily.Attribute( "guid" ).Value.Trim().AsGuid();

                GroupService groupService = new GroupService();
                Group family = groupService.Get( guid );
                if ( family != null )
                {

                    var groupMemberService = new GroupMemberService();
                    var members = groupMemberService.GetByGroupId( family.Id );

                    // delete the people records
                    string errorMessage;
                    List<int> photoIds = members.Select( m => m.Person ).Where( p => p.PhotoId != null ).Select( a => (int)a.PhotoId ).ToList();

                    foreach ( var person in members.Select( m => m.Person ) )
                    {
                        person.GivingGroupId = null;
                        person.PhotoId = null;

                        // delete phone numbers
                        foreach ( var phone in phoneNumberService.GetByPersonId( person.Id ) )
                        {
                            if ( phone != null )
                            {
                                phoneNumberService.Delete( phone, CurrentPersonAlias );
                                phoneNumberService.Save( phone, CurrentPersonAlias );
                            }
                        }

                        // delete person viewed records
                        foreach ( var view in personViewedService.GetByTargetPersonId( person.Id ) )
                        {
                            personViewedService.Delete( view );
                            personViewedService.Save( view );
                        }

                        if ( personService.CanDelete( person, out errorMessage ) )
                        {
                            personService.Delete( person, CurrentPersonAlias );
                        }
                        personService.Save( person, CurrentPersonAlias );
                    }

                    // delete all member photos
                    foreach ( var photo in binaryFileService.GetByIds( photoIds ) )
                    {
                        binaryFileService.Delete( photo );
                        binaryFileService.Save( photo );
                    }

                    DeleteGroupAndMemberData( family );
                }
            }
        }

        /// <summary>
        /// Generic method to delete the members of a group and then the group.
        /// </summary>
        /// <param name="group"></param>
        private void DeleteGroupAndMemberData( Group group )
        {
            GroupService groupService = new GroupService();

            // delete addresses
            GroupLocationService groupLocationService = new GroupLocationService();
            if ( group.GroupLocations.Count > 0 )
            {
                foreach ( var groupLocations in group.GroupLocations.ToList() )
                {
                    group.GroupLocations.Remove( groupLocations );
                    groupLocationService.Delete( groupLocations, CurrentPersonAlias );
                    groupLocationService.Save( groupLocations, CurrentPersonAlias );
                }
            }

            // delete members
            var groupMemberService = new GroupMemberService();
            var members = groupMemberService.GetByGroupId( group.Id );
            foreach ( var member in members.ToList() )
            {
                group.Members.Remove( member );
                groupMemberService.Delete( member );
                groupMemberService.Save( member, CurrentPersonAlias );
            }

            // now delete the group
            if ( groupService.Delete( group, CurrentPersonAlias ) )
            {
                groupService.Save( group, CurrentPersonAlias );
            }
            else
            {
                throw new InvalidOperationException( "Unable to delete group: " + group.Name );
            }
        }

        /// <summary>
        /// Delete all groups found in the given XML.
        /// </summary>
        /// <param name="elemGroups"></param>
        private void DeleteExistingGroups( XElement elemGroups )
        {
            if ( elemGroups == null )
            {
                return;
            }

            GroupService groupService = new GroupService();
            foreach ( var elemGroup in elemGroups.Elements( "group" ) )
            {
                Guid guid = elemGroup.Attribute( "guid" ).Value.Trim().AsGuid();
                Group group = groupService.Get( guid );
                if ( group != null )
                {
                    DeleteGroupAndMemberData( group );
                }
            }
        }

        /// <summary>
        /// Grabs the necessary parameters from the XML and then calls the CreateAttendance() method
        /// to generate all the attendance data for the family.
        /// </summary>
        /// <param name="family"></param>
        /// <param name="elemFamily"></param>
        private void AddFamilyAttendance( Group family, XElement elemFamily )
        {
            // return from here if there's not startingAttendance date
            if ( elemFamily.Attribute( "startingAttendance" ) == null )
            {
                return;
            }

            // get some variables we'll need to create the attendance records
            DateTime startingDate = DateTime.Parse( elemFamily.Attribute( "startingAttendance" ).Value.Trim() );
            DateTime endDate = RockDateTime.Now;

            // If the XML specifies an endingAttendance date use it, otherwise use endingAttendanceWeeksAgo
            // to calculate the end date otherwise we'll just use the current date as the end date.
            if ( elemFamily.Attribute( "endingAttendance" ) != null )
            {
                DateTime.TryParse( elemFamily.Attribute( "endingAttendance" ).Value.Trim(), out endDate );
            }
            else if ( elemFamily.Attribute( "endingAttendanceWeeksAgo" ) != null )
            {
                int endingWeeksAgo = 0;
                int.TryParse( elemFamily.Attribute( "endingAttendanceWeeksAgo" ).Value.Trim(), out endingWeeksAgo );
                endDate = RockDateTime.Now.AddDays( -7 * endingWeeksAgo );
            }

            int pctAttendance = 100;
            if ( elemFamily.Attribute( "percentAttendance" ) != null )
            {
                int.TryParse( elemFamily.Attribute( "percentAttendance" ).Value.Trim(), out pctAttendance );
            }

            int pctAttendedRegularService = 100;
            if ( elemFamily.Attribute( "percentAttendedRegularService" ) != null )
            {
                int.TryParse( elemFamily.Attribute( "percentAttendedRegularService" ).Value.Trim(), out pctAttendedRegularService );
            }

            int scheduleId = 13;
            if ( elemFamily.Attribute( "attendingScheduleId" ) != null )
            {
                int.TryParse( elemFamily.Attribute( "attendingScheduleId" ).Value.Trim(), out scheduleId );
                if ( ! _scheduleTimes.ContainsKey(scheduleId) )
                {
                    Schedule schedule = new ScheduleService().Get( scheduleId );
                    if ( schedule == null )
                    {
                        // We're not going to continue if they are missing this schedule
                        return;
                    }

                    _scheduleTimes.Add( scheduleId, schedule.GetCalenderEvent().DTStart.Value );
                }
            }

            int altScheduleId = 4;
            if ( elemFamily.Attribute( "attendingAltScheduleId" ) != null )
            {
                int.TryParse( elemFamily.Attribute( "attendingAltScheduleId" ).Value.Trim(), out altScheduleId );
                if ( ! _scheduleTimes.ContainsKey( altScheduleId ) )
                {
                    Schedule schedule = new ScheduleService().Get( altScheduleId );
                    if ( schedule == null )
                    {
                        // We're not going to continue if they are missing this schedule
                        return;
                    }

                    _scheduleTimes.Add( altScheduleId, schedule.GetCalenderEvent().DTStart.Value );
                }
            }

            CreateAttendance( family.Members, startingDate, endDate, pctAttendance, pctAttendedRegularService, scheduleId, altScheduleId );
        }

        /// <summary>
        /// Adds attendance data for each child for each weekend since the starting date up to
        /// the weekend ending X weeks ago (endingWeeksAgo).  It will randomly skip a weekend
        /// based on the percentage (pctAttendance) and it will vary which service they attend
        /// between the scheduleId and altScheduleId based on the percentage (pctAttendedRegularService)
        /// given.
        /// </summary>
        /// <param name="familyMembers"></param>
        /// <param name="startingDate">The first date of attendance</param>
        /// <param name="endDate">The end date of attendance</param>
        /// <param name="pctAttendance"></param>
        /// <param name="pctAttendedRegularService"></param>
        /// <param name="scheduleId"></param>
        /// <param name="altScheduleId"></param>
        private void CreateAttendance( ICollection<GroupMember> familyMembers, DateTime startingDate, DateTime endDate, int pctAttendance, int pctAttendedRegularService, int scheduleId, int altScheduleId )
        {
            Guid childGuid = Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_CHILD.AsGuid();

            // foreach weekend between the starting and ending date...
            for ( DateTime date = startingDate; date <= endDate; date = date.AddDays( 7 ) )
            {
                // set an additional factor 
                int summerFactor = ( 7 <= date.Month && date.Month <= 9 ) ? summerPercentFactor : 0;
                if ( _random.Next( 0, 100 ) > pctAttendance - summerFactor )
                {
                    continue; // skip this week
                }

                // which service did they attend
                int serviceSchedId = ( _random.Next( 0, 100 ) > pctAttendedRegularService ) ? scheduleId : altScheduleId;

                // randomize check-in time slightly by +- 0-15 minutes (and 1 out of 4 times being late)
                int minutes = _random.Next( 0, 15 );
                int plusMinus = ( _random.Next( 0, 4 ) == 0 ) ? 1 : -1;
                int randomSeconds = _random.Next( 0, 60 );

                var time = _scheduleTimes[serviceSchedId];

                DateTime dtTime = new DateTime( date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second );
                DateTime checkinDateTime = dtTime.AddMinutes( Convert.ToDouble( plusMinus * minutes ) ).AddSeconds( randomSeconds );

                // foreach child in the family
                foreach ( var member in familyMembers.Where( m => m.GroupRole.Guid == childGuid ) )
                {
                    // Find a class room (group location)
                    // TODO -- someday perhaps we will change this to actually find a real GroupLocationSchedule record
                    var item = (from classroom in _classes
                                    where member.Person.AgePrecise >= classroom.MinAge
                                    && member.Person.AgePrecise <= classroom.MaxAge
                                    orderby classroom.MinAge, classroom.MaxAge
                                    select classroom).FirstOrDefault();

                    // If no suitable classroom was found, skip
                    if ( item == null )
                    {
                        continue;
                    }

                    // Only create one attendance record per day for each person/schedule/group/location
                    AttendanceCode attendanceCode = new AttendanceCode()
                    {
                        Code = GenerateRandomCode( _securityCodeLength ),
                        IssueDateTime = RockDateTime.Now,
                    };

                    Attendance attendance = new Attendance()
                    {
                        ScheduleId = scheduleId,
                        GroupId = item.GroupId,
                        LocationId = item.LocationId,
                        DeviceId = _kioskDeviceId,
                        PersonId = member.PersonId,
                        AttendanceCode = attendanceCode,
                        StartDateTime = checkinDateTime,
                        EndDateTime = null,
                        DidAttend = true
                    };

                    member.Person.Attendances.Add( attendance );
                }
            }
        }

        /// <summary>
        /// A little method to generate a random sequence of characters of a certain length.
        /// </summary>
        /// <param name="len">length of code to generate</param>
        /// <returns>a random sequence of alpha numeric characters</returns>
        private static string GenerateRandomCode( int len )
        {
            string chars = "BCDFGHJKMNPQRTVWXYZ0123456789";
            var code = Enumerable.Range( 0, len ).Select( x => chars[_random.Next( 0, chars.Length )] );
            return new string( code.ToArray() );
        }

        /// <summary>
        /// Takes the given XML element and creates a family member collection.
        /// If the person already exists, their record will be loaded otherwise
        /// a new person will be created using all the attributes for the given
        /// 'person' tag.
        /// </summary>
        /// <param name="elemMembers"></param>
        /// <returns>a list of family members.</returns>
        private List<GroupMember> BuildFamilyMembersFromXml( XElement elemMembers )
        {
            var familyMembers = new List<GroupMember>();

            // First add each person to the familyMembers collection
            foreach ( var personElem in elemMembers.Elements( "person" ) )
            {
                var groupMember = new GroupMember();
                Guid guid = Guid.Parse( personElem.Attribute( "guid" ).Value.Trim() );

                // Attempt to find an existing person...
                Person person = new PersonService().Get( guid );
                if ( person == null )
                {
                    person = new Person();
                    person.Guid = guid;
                    person.FirstName = personElem.Attribute( "firstName" ).Value.Trim();
                    if ( personElem.Attribute( "nickName" ) != null )
                    {
                        person.NickName = personElem.Attribute( "nickName" ).Value.Trim();
                    }

                    if ( personElem.Attribute( "lastName" ) != null )
                    {
                        person.LastName = personElem.Attribute( "lastName" ).Value.Trim();
                    }

                    if ( personElem.Attribute( "birthDate" ) != null )
                    {
                        person.BirthDate = DateTime.Parse( personElem.Attribute( "birthDate" ).Value.Trim() );
                    }

                    // Now, if their age was given we'll change the given birth year to make them
                    // be this age as of Today.
                    if ( personElem.Attribute( "age" ) != null )
                    {
                        int age = int.Parse( personElem.Attribute( "age" ).Value.Trim() );
                        int ageDiff = person.Age - age  ?? 0;
                        person.BirthDate = person.BirthDate.Value.AddYears( ageDiff );
                    }
                    
                    if ( personElem.Attribute( "email" ) != null )
                    {
                        var emailAddress = personElem.Attribute( "email" ).Value.Trim();
                        if ( emailAddress.IsValidEmail() )
                        {
                            person.Email = emailAddress;
                            person.IsEmailActive = personElem.Attribute( "emailIsActive" ) != null && personElem.Attribute( "emailIsActive" ).Value.FromTrueFalse();
                            person.DoNotEmail = personElem.Attribute( "emailDoNotEmail" ) != null && personElem.Attribute( "emailDoNotEmail" ).Value.FromTrueFalse();
                        }
                    }

                    if ( personElem.Attribute( "photoUrl" ) != null )
                    {
                        person.PhotoId = SavePhoto( personElem.Attribute( "photoUrl" ).Value.Trim() );
                    }

                    if ( personElem.Attribute( "recordType" ) != null && personElem.Attribute( "recordType" ).Value.Trim() == "person" )
                    {
                        person.RecordTypeValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_TYPE_PERSON.AsGuid() ).Id;
                    }

                    if ( personElem.Attribute( "maritalStatus" ) != null && personElem.Attribute( "maritalStatus" ).Value.Trim() == "married" )
                    {
                        person.MaritalStatusValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_MARITAL_STATUS_MARRIED.AsGuid() ).Id;
                    }

                    switch ( personElem.Attribute( "recordStatus" ).Value.Trim() )
                    {
                        case "active":
                            person.RecordStatusValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_ACTIVE.AsGuid() ).Id;
                            break;
                        case "inactive":
                            person.RecordStatusValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_INACTIVE.AsGuid() ).Id;
                            break;
                        default:
                            person.RecordStatusValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_PENDING.AsGuid() ).Id;
                            break;
                    }

                    if ( personElem.Attribute( "gender" ) != null )
                    {
                        switch ( personElem.Attribute( "gender" ).Value.Trim().ToLower() )
                        {
                            case "m":
                            case "male":
                                person.Gender = Gender.Male;
                                break;
                            case "f":
                            case "female":
                                person.Gender = Gender.Female;
                                break;
                            default:
                                person.Gender = Gender.Unknown;
                                break;
                        }
                    }
                    else
                    {
                        person.Gender = Gender.Unknown;
                    }

                    if ( personElem.Attribute( "connectionStatus" ) != null )
                    {
                        switch ( personElem.Attribute( "connectionStatus" ).Value.Trim().ToLower() )
                        {
                            case "member":
                                person.ConnectionStatusValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_CONNECTION_STATUS_MEMBER.AsGuid() ).Id;
                                break;
                            case "attendee":
                                person.ConnectionStatusValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_CONNECTION_STATUS_ATTENDEE.AsGuid() ).Id;
                                break;
                            case "visitor":
                            default:
                                person.ConnectionStatusValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_CONNECTION_STATUS_VISITOR.AsGuid() ).Id;
                                break;
                        }
                    }

                    if ( personElem.Attribute( "homePhone" ) != null && !string.IsNullOrEmpty( personElem.Attribute( "homePhone" ).Value.Trim() ) )
                    {
                        var phoneNumber = new PhoneNumber
                        {
                            NumberTypeValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_HOME.AsGuid() ).Id,
                            Number = personElem.Attribute( "homePhone" ).Value.Trim()
                        };
                        person.PhoneNumbers.Add( phoneNumber );
                    }

                    if ( personElem.Attribute( "mobilePhone" ) != null && !string.IsNullOrEmpty( personElem.Attribute( "mobilePhone" ).Value.Trim() ) )
                    {
                        var phoneNumber = new PhoneNumber
                        {
                            NumberTypeValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_MOBILE.AsGuid() ).Id,
                            Number = personElem.Attribute( "mobilePhone" ).Value.Trim()
                        };
                        person.PhoneNumbers.Add( phoneNumber );
                    }

                    if ( personElem.Attribute( "workPhone" ) != null && !string.IsNullOrEmpty( personElem.Attribute( "workPhone" ).Value.Trim() ) )
                    {
                        var phoneNumber = new PhoneNumber
                        {
                            NumberTypeValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_WORK.AsGuid() ).Id,
                            Number = personElem.Attribute( "workPhone" ).Value.Trim()
                        };
                        person.PhoneNumbers.Add( phoneNumber );
                    }
                }

                groupMember.Person = person;

                if ( personElem.Attribute( "familyRole" ) != null && personElem.Attribute( "familyRole" ).Value.Trim().ToLower() == "adult" )
                {
                    groupMember.GroupRoleId = new GroupTypeRoleService().Get( Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT.AsGuid() ).Id;
                }
                else
                {
                    groupMember.GroupRoleId = new GroupTypeRoleService().Get( Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_CHILD.AsGuid() ).Id;
                }

                // person attributes
                if ( personElem.Elements( "attributes" ).Any() )
                {
                    AddPersonAttributes( groupMember, personElem.Elements( "attributes" ) );
                }

                familyMembers.Add( groupMember );
            }

            return familyMembers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupMember"></param>
        /// <param name="attributes"></param>
        private void AddPersonAttributes( GroupMember groupMember, IEnumerable<XElement> attributes )
        {
            // In order to add attributes to the person, you have to first load them all
            groupMember.Person.LoadAttributes();

            foreach ( var personAttribute in attributes.Elements( "attribute" ) )
            {
                foreach ( var pa in personAttribute.Attributes() )
                {
                    groupMember.Person.SetAttributeValue( pa.Name.LocalName, pa.Value );
                }
            }
        }

        /// <summary>
        /// Fetches the given remote photoUrl and stores it locally in the binary file table
        /// then returns Id of the binary file.
        /// </summary>
        /// <param name="photoUrl">a URL to a photo (jpg, png, bmp, tiff).</param>
        /// <returns>Id of the binaryFile</returns>
        private int? SavePhoto( string photoUrl )
        {
            // always create a new BinaryFile record of IsTemporary when a file is uploaded
            BinaryFile binaryFile = new BinaryFile();
            binaryFile.IsTemporary = true;
            binaryFile.BinaryFileTypeId = _binaryFileType.Id;
            binaryFile.FileName = Path.GetFileName( photoUrl );
            binaryFile.Data = new BinaryFileData();
            binaryFile.SetStorageEntityTypeId( _storageEntityType.Id );

            var webClient = new WebClient();
            try
            {
                binaryFile.Data.Content = webClient.DownloadData( photoUrl );

                if ( webClient.ResponseHeaders != null )
                {
                    binaryFile.MimeType = webClient.ResponseHeaders["content-type"];
                }
                else
                {
                    switch ( Path.GetExtension( photoUrl ) )
                    {
                        case ".jpg":
                        case ".jpeg":
                            binaryFile.MimeType = "image/jpg";
                            break;
                        case ".png":
                            binaryFile.MimeType = "image/png";
                            break;
                        case ".gif":
                            binaryFile.MimeType = "image/gif";
                            break;
                        case ".bmp":
                            binaryFile.MimeType = "image/bmp";
                            break;
                        case ".tiff":
                            binaryFile.MimeType = "image/tiff";
                            break;
                        case ".svg":
                        case ".svgz":
                            binaryFile.MimeType = "image/svg+xml";
                            break;
                        default:
                            throw new NotSupportedException( string.Format( "unknown MimeType for {0}", photoUrl ) );
                    }
                }

                var binaryFileService = new BinaryFileService();
                binaryFileService.Add( binaryFile );
                binaryFileService.Save( binaryFile );
                return binaryFile.Id;
            }
            catch ( WebException )
            {
                return null;
            }
        }

        /// <summary>
        /// Adds the given addresses in the xml snippet to the given family.
        /// </summary>
        /// <param name="groupService"></param>
        /// <param name="family"></param>
        /// <param name="addresses"></param>
        private void AddFamilyAddresses( GroupService groupService, Group family, XElement addresses )
        {
            if ( addresses == null || addresses.Elements( "address" ) == null )
            {
                return;
            }

            // First add each person to the familyMembers collection
            foreach ( var addressElem in addresses.Elements( "address" ) )
            {
                var addressType = ( addressElem.Attribute( "type" ) != null ) ? addressElem.Attribute( "type" ).Value.Trim() : "";
                var street1 = ( addressElem.Attribute( "street1" ) != null ) ? addressElem.Attribute( "street1" ).Value.Trim() : "";
                var street2 = ( addressElem.Attribute( "street2" ) != null ) ? addressElem.Attribute( "street2" ).Value.Trim() : "";
                var city = ( addressElem.Attribute( "city" ) != null ) ? addressElem.Attribute( "city" ).Value.Trim() : "";
                var state = ( addressElem.Attribute( "state" ) != null ) ? addressElem.Attribute( "state" ).Value.Trim() : "";
                var zip = ( addressElem.Attribute( "zip" ) != null ) ? addressElem.Attribute( "zip" ).Value.Trim() : "";
                var lat = ( addressElem.Attribute( "lat" ) != null ) ? addressElem.Attribute( "lat" ).Value.Trim() : "";
                var lng = ( addressElem.Attribute( "long" ) != null ) ? addressElem.Attribute( "long" ).Value.Trim() : "";

                string locationTypeGuid;

                switch ( addressType )
                {
                    case "home":
                        locationTypeGuid = Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME;
                        break;
                    case "work":
                        locationTypeGuid = Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_WORK;
                        break;
                    case "previous":
                        locationTypeGuid = Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_PREVIOUS;
                        break;
                    default:
                        throw new NotSupportedException( string.Format( "unknown addressType: {0}", addressType ) );
                }

                groupService.AddNewFamilyAddress( family, locationTypeGuid, street1, street2, city, state, zip, CurrentPersonAlias );

                var location = family.GroupLocations.Where( gl => gl.Location.Street1 == street1 ).Select( gl => gl.Location ).FirstOrDefault();

                // Set the address with the given latitude and longitude
                double latitude;
                double longitude;
                if ( !string.IsNullOrEmpty( lat ) && !string.IsNullOrEmpty( lng )
                    && double.TryParse( lat, out latitude ) && double.TryParse( lng, out longitude )
                    && location != null )
                {
                    location.SetLocationPointFromLatLong( latitude, longitude );
                }

                // Put the location id into the dictionary for later use.
                if ( location != null && !_familyLocationDictionary.ContainsKey( family.Guid ) )
                {
                    _familyLocationDictionary.Add( family.Guid, location.Id );
                }
            }
        }

        /// <summary>
        /// Flattens exception's innerexceptions and returns an Html formatted string
        /// useful for debugging.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private string FlattenInnerExceptions( Exception ex )
        {
            StringBuilder sb = new StringBuilder();
            while ( ex != null && ex.InnerException != null )
            {
                sb.AppendLine( ex.InnerException.Message.ConvertCrLfToHtmlBr() );
                ex = ex.InnerException;
            }
            return sb.ToString();
        }

        #endregion

        # region Helper Class
        protected class ClassGroupLocation
        {
            public string Name { get; set; }
            public int GroupId { get; set; }
            public int LocationId { get; set; }
            public double MinAge { get; set; }
            public double MaxAge { get; set; }
        }
        #endregion
    }
}