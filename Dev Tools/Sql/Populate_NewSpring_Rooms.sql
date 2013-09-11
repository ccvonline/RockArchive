/* ---------------------------------------------------------------------- */
------------------------------ NEWSPRING TEST DATA ------------------------
/* ---------------------------------------------------------------------- */

-- Data Type
DECLARE @TextFieldTypeId int
SET @TextFieldTypeId = (SELECT Id FROM FieldType WHERE guid = '9C204CD0-1233-41C5-818A-C5DA439445AA')
DECLARE @BooleanFieldTypeId int
SET @BooleanFieldTypeId = (SELECT Id FROM FieldType WHERE guid = '1EDAFDED-DFE6-4334-B019-6EECBA89E05A')
DECLARE @DecimalRangeFieldTypeId int
SET @DecimalRangeFieldTypeId = (SELECT Id FROM FieldType WHERE guid = '758D9648-573E-4800-B5AF-7CC29F4BE170')
DECLARE @IntegerRangeFieldTypeId INT
SET @IntegerRangeFieldTypeId = (SELECT Id FROM [FieldType] WHERE [Guid] = '9D5F21E0-DEA0-4E8E-BA42-71151F6A8ED4')
DECLARE @IntegerFieldTypeId INT
SET @IntegerFieldTypeId = (SELECT Id FROM [FieldType] WHERE [Guid] = 'A75DFC58-7A1B-4799-BF31-451B2BBE38FF')
DECLARE @DefinedTypeFieldTypeId int 
SET @DefinedTypeFieldTypeId = (SELECT Id FROM FieldType WHERE guid = 'BC48720C-3610-4BCF-AE66-D255A17F1CDF')
DECLARE @DefinedValueFieldTypeId int 
SET @DefinedValueFieldTypeId = (SELECT Id FROM FieldType WHERE guid = '59D5A94C-94A0-4630-B80A-BB25697D74C7')

-- Person Entity Type
DECLARE @PersonEntityTypeId int
SET @PersonEntityTypeId = (SELECT Id FROM EntityType WHERE Name = 'Rock.Model.Person')

-- Attribute
DECLARE @AttributeGradeTransition int
SELECT @AttributeGradeTransition = [ID] from [Attribute] where [Guid] = '265734A6-C888-45B4-A7A5-9A26478306B8'

------------------------------------------------------------------------
-- Set Defined Value for Check In Note Type
------------------------------------------------------------------------
DECLARE @NoteDefinedTypeId int
SELECT @NoteDefinedTypeId = [Id] FROM [DefinedType] Where [Name] = 'Timeline Sources'
INSERT [DefinedValue]
VALUES ( 1, @NoteDefinedTypeId, 4, 'Check In Note', 'Note created during the check in process', NEWID() )

------------------------------------------------------------------------
-- Create Defined Type and Defined Values for Allergies
------------------------------------------------------------------------
DECLARE @AllergyDefinedTypeId int
INSERT [DefinedType]
VALUES ( 0, 1, 0, 'Person', 'Allergies', 'List of potential allergies', '50F984FE-1CCF-4645-997E-A5610F3E5FFE' )
SET @AllergyDefinedTypeId = SCOPE_IDENTITY()
INSERT [DefinedValue]
VALUES ( 0, @AllergyDefinedTypeId, 0, 'Milk', 'Milk Allergy', NEWID() )
INSERT [DefinedValue]
VALUES ( 0, @AllergyDefinedTypeId, 0, 'Eggs', 'Egg Allergy', NEWID() )
INSERT [DefinedValue]
VALUES ( 0, @AllergyDefinedTypeId, 0, 'Peanuts', 'Peanut Allergy', NEWID() )
INSERT [DefinedValue]
VALUES ( 0, @AllergyDefinedTypeId, 0, 'Tree Nuts', 'Tree Nut Allergy', NEWID() )
INSERT [DefinedValue]
VALUES ( 0, @AllergyDefinedTypeId, 0, 'Fish', 'Fish Allergy', NEWID() )
INSERT [DefinedValue]
VALUES ( 0, @AllergyDefinedTypeId, 0, 'Shellfish', 'Shellfish Allergy', NEWID() )
INSERT [DefinedValue]
VALUES ( 0, @AllergyDefinedTypeId, 0, 'Soy', 'Soy Allergy', NEWID() )
INSERT [DefinedValue]
VALUES ( 0, @AllergyDefinedTypeId, 0, 'Wheat', 'Wheat Allergy', NEWID() )
INSERT [DefinedValue]
VALUES ( 0, @AllergyDefinedTypeId, 0, 'Latex', 'Latex Allergy', NEWID() )

INSERT Attribute ([IsSystem],[FieldTypeId],[EntityTypeId],[EntityTypeQualifierColumn],[EntityTypeQualifierValue],[Key],[Name],[Description],[Order],[IsGridColumn],[DefaultValue],[IsMultiValue],[IsRequired],[Guid])
VALUES (0, @DefinedValueFieldTypeId, @PersonEntityTypeId, '', '', 'Allergy', 'Allergy', 'The item(s) this person is allergic to.', 0, 0, '', 0, 0, NEWID())

------------------------------------------------------------------------
-- Update Defined Type names for Ability Level
------------------------------------------------------------------------
DECLARE @DTAbilityLevelId int
SELECT @DTAbilityLevelId = [Id] FROM [DefinedType] WHERE [Name] = 'Ability Level'

DECLARE @AbilityCuddler uniqueidentifier
DECLARE @AbilityCrawler uniqueidentifier
DECLARE @AbilityWalker uniqueidentifier
DECLARE @AbilityPottyTrained uniqueidentifier
DECLARE @AbilityToddler uniqueidentifier

UPDATE [DefinedValue]
SET Name = 'Cuddler' 
WHERE DefinedTypeId = @DTAbilityLevelId AND [Name] = 'Infant'

UPDATE [DefinedValue]
SET Name = 'Crawler', Description = 'The child is able to crawl.'
WHERE DefinedTypeId = @DTAbilityLevelId AND [Name] = 'Crawling or Walking'

INSERT [DefinedValue]
VALUES ( 1, @DTAbilityLevelId, 2, 'Walker', 'The child is able to walk.', NEWID() )

INSERT [DefinedValue]
VALUES ( 1, @DTAbilityLevelId, 4, 'Toddler', 'The child is able to run and climb.', NEWID() )

SELECT @AbilityCuddler = [Guid] FROM [DefinedValue] 
WHERE [DefinedTypeId] = @DTAbilityLevelId 
AND Name = 'Cuddler'

SELECT @AbilityCrawler = [Guid] FROM [DefinedValue] 
WHERE [DefinedTypeId] = @DTAbilityLevelId 
AND Name = 'Crawler'

SELECT @AbilityWalker = [Guid] FROM [DefinedValue] 
WHERE [DefinedTypeId] = @DTAbilityLevelId 
AND Name = 'Walker'

SELECT @AbilityPottyTrained = [Guid] FROM [DefinedValue] 
WHERE [DefinedTypeId] = @DTAbilityLevelId 
AND Name = 'Potty Trained'

SELECT @AbilityToddler = [Guid] FROM [DefinedValue] 
WHERE [DefinedTypeId] = @DTAbilityLevelId 
AND Name = 'Toddler'

------------------------------------------------------------------------
-- Insert Attribute for GradeTransitionDate
------------------------------------------------------------------------

INSERT INTO [AttributeValue] (IsSystem, AttributeId, Value, Guid) 
VALUES (0, @AttributeGradeTransition, '08/01', NEWID())

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Model.GroupType')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured, IsCommon)
VALUES ('Rock.Model.GroupType', NEWID(), 0, 0, 0)

DECLARE @GroupTypeEntityTypeId int
SET @GroupTypeEntityTypeId = (SELECT Id FROM EntityType WHERE Name = 'Rock.Model.GroupType')
DECLARE @GroupEntityTypeId int
SET @GroupEntityTypeId = (SELECT Id FROM EntityType WHERE Name = 'Rock.Model.Group')
DECLARE @AttributeEntityTypeId INT
SET @AttributeEntityTypeId = (SELECT Id FROM [EntityType] WHERE Name = 'Rock.Model.Attribute')

-- Group Type Check-in Category Id
DECLARE @GroupTypeCheckInCategoryId INT
SET @GroupTypeCheckInCategoryId = (
	SELECT Id FROM Category 
	WHERE EntityTypeId = @AttributeEntityTypeId 
	AND EntityTypeQualifierColumn = 'EntityTypeId' 
	AND EntityTypeQualifierValue = CAST(@GroupTypeEntityTypeId AS varchar)
	AND Name = 'Check-in'
)
IF @GroupTypeCheckInCategoryId IS NULL
BEGIN
	INSERT INTO Category (IsSystem, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, Name, Guid)
	VALUES (0, @AttributeEntityTypeId, 'EntityTypeId', CAST(@GroupTypeEntityTypeId AS varchar), 'Checkin', NEWID())
	SET @GroupTypeCheckInCategoryId = SCOPE_IDENTITY()
END

------------------------------------------------------------------------
-- Add GroupTypes
------------------------------------------------------------------------
DECLARE @TopLevelGroupTypeId int
DECLARE @AgeTypeId int
DECLARE @GradeTypeId int
DECLARE @AbilityTypeId int
DECLARE @GenderTypeId int
DECLARE @KidSpringTypeId int
DECLARE @NurseryTypeId int
DECLARE @PreschoolTypeId int
DECLARE @ElementaryTypeId int
DECLARE @FuseTypeId int
DECLARE @MSTypeId int
DECLARE @HSTypeId int
DECLARE @GSTypeId int
DECLARE @CTTypeId int
DECLARE @NSTypeId int

DECLARE @GroupTypePurposeCheckinTemplateId INT
SET @GroupTypePurposeCheckinTemplateId = (select [Id] from [DefinedValue] where [Guid] = '4A406CB0-495B-4795-B788-52BDFDE00B01');

DECLARE @GroupTypePurposeCheckinFilterId INT
SET @GroupTypePurposeCheckinFilterId = (select [Id] from [DefinedValue] where [Guid] = '6BCED84C-69AD-4F5A-9197-5C0F9C02DD34');

-- Set a top level group
INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo],[ShowInGroupList],[ShowInNavigation],[Order],[LocationSelectionMode],[InheritedGroupTypeId],[GroupTypePurposeValueId])
VALUES (0, 'Checkin', NEWID(), 1, 0, 0, 0, 0, 0, 0, 0, NULL, @GroupTypePurposeCheckinTemplateId)
SET @TopLevelGroupTypeId = SCOPE_IDENTITY()

-- Age/Grade/Ability GroupTypes
INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo],[ShowInGroupList],[ShowInNavigation],[Order],[LocationSelectionMode],[InheritedGroupTypeId],[GroupTypePurposeValueId])
   VALUES (0, 'Check in by Age', NEWID(), 0, 0, 0, 0, 0, 0, 0, 0, NULL, @GroupTypePurposeCheckinFilterId)
SET @AgeTypeId = SCOPE_IDENTITY()
INSERT INTO [GroupTypeAssociation] VALUES (@TopLevelGroupTypeId, @AgeTypeId);

INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo],[ShowInGroupList],[ShowInNavigation],[Order],[LocationSelectionMode],[InheritedGroupTypeId],[GroupTypePurposeValueId])
   VALUES (0, 'Check in by Grade', NEWID(), 0, 0, 0, 0, 0, 0, 0, 0, @AgeTypeId, @GroupTypePurposeCheckinFilterId)
SET @GradeTypeId = SCOPE_IDENTITY()
INSERT INTO [GroupTypeAssociation] VALUES (@TopLevelGroupTypeId, @GradeTypeId);

INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo],[ShowInGroupList],[ShowInNavigation],[Order],[LocationSelectionMode],[InheritedGroupTypeId],[GroupTypePurposeValueId])
   VALUES (0, 'Check in by Ability', NEWID(), 0, 0, 0, 0, 0, 0, 0, 0, @AgeTypeId, @GroupTypePurposeCheckinFilterId)
SET @AbilityTypeId = SCOPE_IDENTITY()
INSERT INTO [GroupTypeAssociation] VALUES (@TopLevelGroupTypeId, @AbilityTypeId);

-- Add GroupType attributes to Age to filter checkin people
DECLARE @GroupTypeAttributeAgeRange int

INSERT INTO [Attribute] ( IsSystem, FieldTypeId, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, [Key], Name, [Order], IsGridColumn, IsMultiValue, IsRequired, Guid, Description)
VALUES ( 0, @DecimalRangeFieldTypeId, @GroupTypeEntityTypeId, 'Id', @AgeTypeId, 'AgeRange', 'Age Range', 0, 0, 0, 0, NEWID(), 'The age range required to check in to these grouptypes.')
SET @GroupTypeAttributeAgeRange = SCOPE_IDENTITY()

------------------------------------------------------------------------
-- Pause GroupType, add Group Attributes
------------------------------------------------------------------------
DECLARE @AgeRangeAttributeId int
DECLARE @GradeRangeAttributeId int
DECLARE @AbilityAttributeId int
DECLARE @GenderAttributeId int

-- Age Range Group Attribute
INSERT INTO [Attribute] ( IsSystem, FieldTypeId, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, [Key], Name, [Order], IsGridColumn, IsMultiValue, IsRequired, [Guid], [Description] )
	VALUES ( 0, @DecimalRangeFieldTypeId, @GroupEntityTypeId, 'GroupTypeId', @AgeTypeId, 'AgeRange', 'Age Range', 0, 0, 0, 0, NEWID(), 'Defines the age range required to check in to these groups.' )
SET @AgeRangeAttributeId = SCOPE_IDENTITY()
INSERT INTO [AttributeCategory] (AttributeId, CategoryId) VALUES (@AgeRangeAttributeId, @GroupTypeCheckInCategoryId)

-- Grade Range Group Attribute
INSERT INTO [Attribute] ( IsSystem, FieldTypeId, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, [Key], Name, [Order], IsGridColumn, IsMultiValue, IsRequired, [Guid], [Description] )
	VALUES ( 0, @IntegerRangeFieldTypeId, @GroupEntityTypeId, 'GroupTypeId', @GradeTypeId, 'GradeRange', 'Grade Range', 0, 0, 0, 0, NEWID(), 'Defines the grade range required to check in to these groups.' )
SET @GradeRangeAttributeId = SCOPE_IDENTITY()
INSERT INTO [AttributeCategory] (AttributeId, CategoryId) VALUES (@GradeRangeAttributeId, @GroupTypeCheckInCategoryId)

-- Ability Level Group Attribute
INSERT INTO [Attribute] ( IsSystem, FieldTypeId, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, [Key], Name, [Order], IsGridColumn, IsMultiValue, IsRequired, [Guid], [Description] )
	VALUES ( 0, @DefinedValueFieldTypeId, @GroupEntityTypeId, 'GroupTypeId', @AbilityTypeId, 'AbilityLevel', 'Ability Level', 1, 0, 0, 0, NEWID(), 'Defines the ability level required to check in to these groups.' )
SET @AbilityAttributeId = SCOPE_IDENTITY()
INSERT INTO [AttributeCategory] (AttributeId, CategoryId) VALUES (@AbilityAttributeId, @GroupTypeCheckInCategoryId)

------------------------------------------------------------------------
-- Resume Adding GroupTypes
------------------------------------------------------------------------

-- KidSpring
INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo],[ShowInGroupList],[ShowInNavigation],[Order],[LocationSelectionMode],[InheritedGroupTypeId],[GroupTypePurposeValueId])
VALUES (0, 'KidSpring', NEWID(), 1, 0, 0, 0, 0, 0, 0, 0, NULL, NULL)
SET @KidSpringTypeId = SCOPE_IDENTITY()
INSERT INTO [GroupTypeAssociation] VALUES (@TopLevelGroupTypeId, @KidSpringTypeId);

-- Nursery
INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo],[ShowInGroupList],[ShowInNavigation],[Order],[LocationSelectionMode],[InheritedGroupTypeId],[GroupTypePurposeValueId]) 
VALUES (0, 'Nursery', NEWID(), 1, 0, 0, 0, 0, 0, 0, 0, @AbilityTypeId, NULL)
SET @NurseryTypeId = SCOPE_IDENTITY()
INSERT INTO [GroupTypeAssociation] VALUES (@KidSpringTypeId, @NurseryTypeId);
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GroupTypeAttributeAgeRange, @NurseryTypeId, 0, '0.0,2.0', NEWID())

-- Preschool
INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo],[ShowInGroupList],[ShowInNavigation],[Order],[LocationSelectionMode],[InheritedGroupTypeId],[GroupTypePurposeValueId]) 
VALUES (0, 'Preschool', NEWID(), 1, 0, 0, 0, 0, 0, 0, 0, @AbilityTypeId, NULL)
SET @PreschoolTypeId = SCOPE_IDENTITY()
INSERT INTO [GroupTypeAssociation] VALUES (@KidSpringTypeId, @PreschoolTypeId);
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GroupTypeAttributeAgeRange, @PreschoolTypeId, 0, '2.0,4.0', NEWID())

-- Elementary
INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo],[ShowInGroupList],[ShowInNavigation],[Order],[LocationSelectionMode],[InheritedGroupTypeId],[GroupTypePurposeValueId]) 
VALUES (0, 'Elementary', NEWID(), 1, 0, 0, 0, 0, 0, 0, 0, @GradeTypeId, NULL)
SET @ElementaryTypeId = SCOPE_IDENTITY()
INSERT INTO [GroupTypeAssociation] VALUES (@KidSpringTypeId, @ElementaryTypeId);
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GroupTypeAttributeAgeRange, @ElementaryTypeId, 0, '4.0,11.0', NEWID())


------------------------------------------------------------------------------

-- Fuse
INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo],[ShowInGroupList],[ShowInNavigation],[Order],[LocationSelectionMode],[InheritedGroupTypeId],[GroupTypePurposeValueId]) 
VALUES (0, 'Fuse', NEWID(), 1, 0, 0, 0, 0, 0, 0, 0, @GradeTypeId, NULL)
SET @FuseTypeId = SCOPE_IDENTITY()
INSERT INTO [GroupTypeAssociation] VALUES (@TopLevelGroupTypeId, @FuseTypeId);
INSERT INTO [Attribute] ( IsSystem, FieldTypeId, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, [Key], Name, [Order], IsGridColumn, IsMultiValue, IsRequired, [Guid], [Description] )
VALUES ( 0, @IntegerFieldTypeId, @GroupEntityTypeId, 'GroupTypeId', @FuseTypeId, 'Gender', 'Gender', 1, 0, 0, 0, NEWID(), 'Defines the gender required to check into these group types.' )
SET @GenderAttributeId = SCOPE_IDENTITY()

-- Middle School
INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo],[ShowInGroupList],[ShowInNavigation],[Order],[LocationSelectionMode],[InheritedGroupTypeId],[GroupTypePurposeValueId]) 
VALUES (0, 'Middle School', NEWID(), 1, 0, 0, 0, 0, 0, 0, 0, @FuseTypeId, NULL)
SET @MSTypeId = SCOPE_IDENTITY()
INSERT INTO [GroupTypeAssociation] VALUES (@FuseTypeId, @MSTypeId);
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GroupTypeAttributeAgeRange, @MSTypeId, 0, '11.0,14.0', NEWID())

-- High School
INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo],[ShowInGroupList],[ShowInNavigation],[Order],[LocationSelectionMode],[InheritedGroupTypeId],[GroupTypePurposeValueId]) 
VALUES (0, 'High School', NEWID(), 1, 0, 0, 0, 0, 0, 0, 0, @FuseTypeId, NULL)
SET @HSTypeId = SCOPE_IDENTITY()
INSERT INTO [GroupTypeAssociation] VALUES (@FuseTypeId, @HSTypeId);
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GroupTypeAttributeAgeRange, @HSTypeId, 0, '14.0,19.99', NEWID())

------------------------------------------------------------------------
-- Add Campus & Location
------------------------------------------------------------------------
DELETE [Location]
DECLARE @CampusLocationId int
DECLARE @CampusID int

INSERT INTO [Location] ([Guid], [Name], [IsActive], [IsLocation], [Street1], [City], [State], [Zip], [Country])	
VALUES (NEWID(), 'Anderson', 1, 1, '2940 Concord Road', 'Anderson', 'SC', '29621', 'US')
SET @CampusLocationId = SCOPE_IDENTITY()

INSERT INTO [Campus] ( [IsSystem], [Name], [Guid], [LocationId], [ShortCode] )
VALUES (0, 'Anderson', NEWID(), @CampusLocationId, 'AND')
SET @CampusID = SCOPE_IDENTITY()

------------------------------------------------------------------------
-- Add Groups
------------------------------------------------------------------------
DECLARE @TopLevelGroupId int
DECLARE @ParentGroupId int
DECLARE @KidSpringGroupId int
DECLARE @FuseGroupId int
DECLARE @NSGroupId int
DECLARE @CTGroupId int
DECLARE @GSGroupId int
DECLARE @GroupId int

-- Check-in Area
SELECT @TopLevelGroupTypeId = [Id] FROM [GroupType] GT WHERE GT.Name = 'Check-in'
INSERT INTO [Group] ( [IsSystem],[GroupTypeId],[Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @TopLevelGroupTypeId, 'Check-in', 0, 1, 0, NEWID() 
SET @TopLevelGroupId = SCOPE_IDENTITY()

-- KidSpring
INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @TopLevelGroupId, @KidSpringTypeId, @CampusID, 'KidSpring', 0, 1, 0, NEWID() 
SET @ParentGroupId = SCOPE_IDENTITY()

-- Nursery Groups
INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @NurseryTypeId, @CampusID, 'Wonder Way - 1', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '0,0.59', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityCuddler, NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @NurseryTypeId, @CampusID, 'Wonder Way - 2', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '0,0.59', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityCuddler, NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @NurseryTypeId, @CampusID, 'Wonder Way - 3', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '0.59,1.09', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityCrawler, NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @NurseryTypeId, @CampusID, 'Wonder Way - 4', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '0.59,1.09', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityCrawler, NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @NurseryTypeId, @CampusID, 'Wonder Way - 5', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '1.09,1.59', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityWalker, NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @NurseryTypeId, @CampusID, 'Wonder Way - 6', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '1.09,1.59', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityWalker, NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @NurseryTypeId, @CampusID, 'Wonder Way - 7', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '1.59,2.09', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityToddler, NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @NurseryTypeId, @CampusID, 'Wonder Way - 8', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '1.59,2.09', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityToddler, NEWID())

---- Preschool
INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @PreschoolTypeId, @CampusID, 'Fire Station', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '2,2', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityToddler, NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @PreschoolTypeId, @CampusID, 'Lil'' Spring', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '2,2', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityToddler, NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @PreschoolTypeId, @CampusID, 'Pop''s Garage', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '2,2', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityToddler, NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @PreschoolTypeId, @CampusID, 'Spring Fresh', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '3,3', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityPottyTrained, NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @PreschoolTypeId, @CampusID, 'SpringTown Police', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '3,3', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityPottyTrained, NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @PreschoolTypeId, @CampusID, 'SpringTown Toys', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '3,3', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityPottyTrained, NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @PreschoolTypeId, @CampusID, 'Treehouse', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '4,4', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AbilityAttributeId, @GroupId, 0, @AbilityPottyTrained, NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @PreschoolTypeId, @CampusID, 'Base Camp Jr.', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '2,4', NEWID())

-- Elementary
INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @ElementaryTypeId, @CampusID, 'Base Camp (ES)', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '5,12', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '0,5', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @ElementaryTypeId, @CampusID, 'ImagiNation - K', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '4,5', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '0,0', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @ElementaryTypeId, @CampusID, 'ImagiNation - 1st', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '6,7', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '1,1', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @ElementaryTypeId, @CampusID, 'Jump Street - 2nd', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '7,8', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '2,2', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @ElementaryTypeId, @CampusID, 'Jump Street - 3rd', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '8,9', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '3,3', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @ElementaryTypeId, @CampusID, 'Shockwave - 4th', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '9,10', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '4,4', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @ElementaryTypeId, @CampusID, 'Shockwave - 5th', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '10,11', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '5,5', NEWID())

-- Fuse
INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @TopLevelGroupId, @FuseTypeId, @CampusID, 'Fuse', 0, 1, 0, NEWID() 
SET @ParentGroupId = SCOPE_IDENTITY()

-- Middle School Groups
INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @MSTypeId, @CampusID, '6th Grade Boy', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '11,12', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '6,6', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GenderAttributeId, @GroupId, 0, '1', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @MSTypeId, @CampusID, '6th Grade Girl', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '11,12', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '6,6', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GenderAttributeId, @GroupId, 0, '2', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @MSTypeId, @CampusID, '7th Grade Boy', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '12,13', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '7,7', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GenderAttributeId, @GroupId, 0, '1', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @MSTypeId, @CampusID, '7th Grade Girl', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '12,13', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '7,7', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GenderAttributeId, @GroupId, 0, '2', NEWID())


INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @MSTypeId, @CampusID, '8th Grade Boy', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '13,14', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '8,8', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GenderAttributeId, @GroupId, 0, '1', NEWID())


INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @MSTypeId, @CampusID, '8th Grade Girl', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '13,14', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '8,8', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GenderAttributeId, @GroupId, 0, '2', NEWID())

-- High School Groups
INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @HSTypeId, @CampusID, '9th Grade Boy', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '14,15', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '9,9', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GenderAttributeId, @GroupId, 0, '1', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @HSTypeId, @CampusID, '9th Grade Girl', 0, 1, 0, NEWID() 
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '14,15', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '9,9', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GenderAttributeId, @GroupId, 0, '2', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @HSTypeId, @CampusID, '10th Grade Boy', 0, 1, 0, NEWID() 
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '15,16', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '10,10', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GenderAttributeId, @GroupId, 0, '1', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @HSTypeId, @CampusID, '10th Grade Girl', 0, 1, 0, NEWID() 
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '15,16', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '10,10', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GenderAttributeId, @GroupId, 0, '2', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @HSTypeId, @CampusID, '11th Grade Boy', 0, 1, 0, NEWID() 
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '16,17', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '11,11', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GenderAttributeId, @GroupId, 0, '1', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @HSTypeId, @CampusID, '11th Grade Girl', 0, 1, 0, NEWID() 
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '16,17', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '11,11', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GenderAttributeId, @GroupId, 0, '2', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @HSTypeId, @CampusID, '12th Grade Boy', 0, 1, 0, NEWID() 
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '17,19', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '12,12', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GenderAttributeId, @GroupId, 0, '1', NEWID())

INSERT INTO [Group] ( [IsSystem],[ParentGroupId], [GroupTypeId], [CampusId], [Name],[IsSecurityRole],[IsActive],[Order],[Guid]) 
SELECT 0, @ParentGroupId, @HSTypeId, @CampusID, '12th Grade Girl', 0, 1, 0, NEWID() 
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @AgeRangeAttributeId, @GroupId, 0, '17,19', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GradeRangeAttributeId, @GroupId, 0, '12,12', NEWID())
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], [Guid]) VALUES (0, @GenderAttributeId, @GroupId, 0, '2', NEWID())

------------------------------------------------------------------------
-- Create Schedules
------------------------------------------------------------------------
DELETE [Schedule]
INSERT INTO [Schedule] ([Name],[iCalendarContent],[CheckInStartOffsetMinutes],[CheckInEndOffsetMinutes],[EffectiveStartDate],[Guid]) VALUES 
    ('9:15 AM',
'BEGIN:VCALENDAR
BEGIN:VEVENT
DTEND:20130701T235900
DTSTART:20130625T000000
RRULE:FREQ=DAILY
END:VEVENT
END:VCALENDAR', '0', '1439', '06/01/2013', NEWID() )
INSERT INTO [Schedule] ([Name],[iCalendarContent], [CheckInStartOffsetMinutes],[CheckInEndOffsetMinutes],[EffectiveStartDate],[Guid]) VALUES 
    ('11:15 AM', 
'BEGIN:VCALENDAR
BEGIN:VEVENT
DTEND:20130701T235900
DTSTART:20130625T000000
RRULE:FREQ=DAILY
END:VEVENT
END:VCALENDAR', '0', '1439', '06/01/2013', NEWID() )
INSERT INTO [Schedule] ([Name],[iCalendarContent], [CheckInStartOffsetMinutes],[CheckInEndOffsetMinutes],[EffectiveStartDate],[Guid]) VALUES 
    ('4:00 PM', 
'BEGIN:VCALENDAR
BEGIN:VEVENT
DTEND:20130701T235900
DTSTART:20130625T000000
RRULE:FREQ=DAILY
END:VEVENT
END:VCALENDAR', '0', '1439', '06/01/2013', NEWID() )
INSERT INTO [Schedule] ([Name],[iCalendarContent], [CheckInStartOffsetMinutes],[CheckInEndOffsetMinutes],[EffectiveStartDate],[Guid]) VALUES 
    ('6:00 PM', 
'BEGIN:VCALENDAR
BEGIN:VEVENT
DTEND:20130701T235900
DTSTART:20130625T000000
RRULE:FREQ=DAILY
END:VEVENT
END:VCALENDAR', '0', '1439', '06/01/2013', NEWID() )

------------------------------------------------------------------------
-- Create Locations
------------------------------------------------------------------------
DECLARE @KioskLocationId int
DECLARE @RoomLocationId int

-- Don't make a distinction between buildings, use the campus as a parent
SET @KioskLocationId = @CampusLocationId

-- KidSpring rooms
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Wonder Way - 1', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Wonder Way - 2', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Wonder Way - 3', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Wonder Way - 4', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Wonder Way - 5', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Wonder Way - 6', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Wonder Way - 7', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Wonder Way - 8', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Fire Station', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Lil'' Spring', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Pop''s Garage', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Spring Fresh', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'SpringTown Police', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'SpringTown Toys', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Treehouse', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Base Camp Jr.', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Base Camp (ES)', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'ImagiNation - K', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'ImagiNation - 1st', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Jump Street - 2nd', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Jump Street - 3rd', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Shockwave - 4th', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, 'Shockwave - 5th', 1, 1, NEWID())

-- Fuse rooms
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, '6th Grade Boy', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, '6th Grade Girl', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, '7th Grade Boy', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, '7th Grade Girl', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, '8th Grade Boy', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, '8th Grade Girl', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, '9th Grade Boy', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, '9th Grade Girl', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, '10th Grade Boy', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, '10th Grade Girl', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, '11th Grade Boy', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, '11th Grade Girl', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, '12th Grade Boy', 1, 1, NEWID())
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [IsLocation], [Guid]) VALUES (@KioskLocationId, '12th Grade Girl', 1, 1, NEWID())

------------------------------------------------------------------------
-- Devices (Kiosks)
------------------------------------------------------------------------
DELETE [DeviceLocation]
DELETE [Device]

-- Device Types

DECLARE @DeviceTypeValueId int
SET @DeviceTypeValueId = (SELECT [Id] FROM [DefinedValue] WHERE [Guid] = 'BC809626-1389-4543-B8BB-6FAC79C27AFD')
DECLARE @PrinterTypevalueId int
SET @PrinterTypevalueId = (SELECT [Id] FROM [DefinedValue] WHERE [Guid] = '8284B128-E73B-4863-9FC2-43E6827B65E6')

DECLARE @PrinterDeviceId int
INSERT INTO [Device] ([Name],[DeviceTypeValueId],[IPAddress],[PrintFrom],[PrintToOverride],[Guid])
VALUES ('Test Printer',@PrinterTypevalueId, '10.1.20.200',0,1,NEWID())
SET @PrinterDeviceId = SCOPE_IDENTITY()

INSERT INTO [Device] ([Name],[DeviceTypeValueId],[PrinterDeviceId],[PrintFrom],[PrintToOverride],[Guid])
SELECT C.ShortCode + '-CHECK01', @DeviceTypeValueId, @PrinterDeviceId, 0, 1, NEWID()
FROM Location L
INNER JOIN Campus C
ON C.LocationId = L.ID

INSERT INTO [Device] ([Name],[DeviceTypeValueId],[PrinterDeviceId], [IpAddress],[PrintFrom],[PrintToOverride],[Guid])
SELECT 'CEN-RDUBAY', @DeviceTypeValueId, @PrinterDeviceId, '10.0.20.82', 0, 1, NEWID()

INSERT INTO [Device] ([Name],[DeviceTypeValueId],[PrinterDeviceId], [IpAddress],[PrintFrom],[PrintToOverride],[Guid])
SELECT 'CEN-DSTEVENS', @DeviceTypeValueId, @PrinterDeviceId, '10.0.20.94', 0, 1, NEWID()

INSERT INTO [DeviceLocation] (DeviceId, LocationId)
SELECT D.Id, B.Id
FROM Location L
INNER JOIN Campus C
on C.LocationId = L.Id
INNER JOIN Location B
	ON B.ParentLocationId = L.Id
INNER JOIN Device D 
	ON D.Name = C.ShortCode + '-CHECK01'

INSERT INTO [DeviceLocation] (DeviceId, LocationId)
SELECT D.Id, B.Id
FROM Location L
INNER JOIN Location B
	ON B.ParentLocationId = L.Id
INNER JOIN Device D 
	ON D.Name = 'CEN-RDUBAY'

INSERT INTO [DeviceLocation] (DeviceId, LocationId)
SELECT D.Id, B.Id
FROM Location L
INNER JOIN Location B
	ON B.ParentLocationId = L.Id
INNER JOIN Device D 
	ON D.Name = 'CEN-DSTEVENS'

------------------------------------------------------------------------
-- Group Locations
------------------------------------------------------------------------
-- Insert a grouplocation for each checkin room
DELETE [GroupLocation]
INSERT INTO [GroupLocation] (GroupId, LocationId, IsMailing, IsLocation, Guid) 
SELECT G.Id, L.Id, 0, 1, NEWID()
FROM Location L
INNER JOIN [Group] G 
ON G.Name = L.Name

------------------------------------------------------------------------
-- Group Location Schedule
------------------------------------------------------------------------
DELETE [GroupLocationSchedule]

INSERT INTO [GroupLocationSchedule] (GroupLocationId, ScheduleId) 
SELECT GL.Id, S.Id
FROM GroupLocation GL
INNER JOIN [Group] G 
ON G.Id = GL.GroupId
INNER JOIN [Group] G2
ON G.ParentGroupId = G2.Id
INNER JOIN Schedule S 
ON S.[Name] LIKE '%M'

/* ---------------------------------------------------------------------- */
------------------------------ END TEST DATA ---------------------------------
/* ---------------------------------------------------------------------- */
