/****** Script for SelectTopNRows command from SSMS  ******/

insert category (issystem, EntityTypeId, name, guid)
values (1, 32, 'Checkin', newid())

set identity_insert dbo.workflowtype on
insert workflowtype (Id, [IsSystem], [IsActive], [Name], [Description], [CategoryId], [Order], [WorkTerm], [ProcessingIntervalSeconds], [IsPersisted], [LoggingLevel], [Guid])
select * from analysis.._workflowtype
set identity_insert dbo.workflowtype off
select * from workflowtype

set identity_insert dbo.workflowactivitytype on
insert WorkflowActivityType (Id, [IsActive], [WorkflowTypeId], [Name], [Description], [IsActivatedWithWorkflow], [Order], [Guid])
select * from analysis.._workflowactivitytype
set identity_insert dbo.workflowactivitytype off
select * from workflowactivitytype

/*
IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Model.GroupType')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Model.GroupType', NEWID(), 0, 0)
DECLARE @GroupTypeEntityTypeId int
SET @GroupTypeEntityTypeId = (SELECT Id FROM EntityType WHERE Name = 'Rock.Model.GroupType')

DECLARE @DecimalFieldTypeId int
SET @DecimalFieldTypeId = (SELECT Id FROM FieldType WHERE guid = 'c757a554-3009-4214-b05d-cea2b2ea6b8f')
DELETE [Attribute] WHERE guid = '63FA25AA-7796-4302-BF05-D96A1C390BD7'
INSERT INTO [Attribute] ( IsSystem, FieldTypeId, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, [Key], Name, [Order], IsGridColumn, IsMultiValue, IsRequired, Category, Guid)
VALUES ( 0, @DecimalFieldTypeId, @GroupTypeEntityTypeId, 'TakesAttendance', 'True', 'MinAge', 'Minimum Age', 0, 0, 0, 0, '', '63FA25AA-7796-4302-BF05-D96A1C390BD7')
DELETE [Attribute] WHERE guid = 'D05368C9-5069-49CD-B7E8-9CE8C46BB75D'
INSERT INTO [Attribute] ( IsSystem, FieldTypeId, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, [Key], Name, [Order], IsGridColumn, IsMultiValue, IsRequired, Category, Guid)
VALUES ( 0, @DecimalFieldTypeId, @GroupTypeEntityTypeId, 'TakesAttendance', 'True', 'MaxAge', 'Maximum Age', 1, 0, 0, 0, '', 'D05368C9-5069-49CD-B7E8-9CE8C46BB75D')

-- Groups
DELETE GTA FROM [GroupTypeAssociation] GTA INNER JOIN [GroupType] GT ON GT.Id = GTA.GroupTypeId AND GT.Name IN ('Family Ministries', 'Check-in Test (Don''t Use)', 'Middle Elementary', '3rd Grade', '4th Grade')
UPDATE [Group] SET ParentGroupId = null WHERE Name in ('Family Ministries', 'Check-in Test (Don''t Use)', 'Middle Elementary', '3rd Grade', '4th Grade')
DELETE [Group] WHERE Name in ('Family Ministries', 'Check-in Test (Don''t Use)', 'Middle Elementary', '3rd Grade', '4th Grade')
DELETE [GroupType] WHERE Name in ('Family Ministries', 'Check-in Test (Don''t Use)', 'Middle Elementary', '3rd Grade', '4th Grade')
DECLARE @ParentGroupTypeId int
DECLARE @ChildGroupTypeId int
DECLARE @GroupRoleId int

INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo]) VALUES (0, 'Family Ministries', NEWID(), 0, 0, 0, 0)
SET @ParentGroupTypeId = SCOPE_IDENTITY()

INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo]) VALUES (0, 'Check-in Test (Don''t Use)', NEWID(), 1, 1, 0, 0)
SET @ChildGroupTypeId = SCOPE_IDENTITY()
INSERT INTO [GroupTypeAssociation] VALUES (@ParentGroupTypeId, @ChildGroupTypeId);

INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo]) VALUES (0, 'Middle Elementary', NEWID(), 0, 0, 0, 0)
SET @ChildGroupTypeId = SCOPE_IDENTITY()
INSERT INTO [GroupTypeAssociation] VALUES (@ParentGroupTypeId, @ChildGroupTypeId);
SET @ParentGroupTypeId = @ChildGroupTypeId

INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo]) VALUES (0, '3rd Grade', NEWID(), 1, 1, 0, 0)
SET @ChildGroupTypeId = SCOPE_IDENTITY()
INSERT INTO [GroupTypeAssociation] VALUES (@ParentGroupTypeId, @ChildGroupTypeId);
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], Guid) SELECT 1, Id, @ChildGroupTypeId, 0, '7.0', newid() FROM Attribute WHERE guid = '63FA25AA-7796-4302-BF05-D96A1C390BD7'
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], Guid) SELECT 1, Id, @ChildGroupTypeId, 0, '10.99', newid() FROM Attribute WHERE guid = 'D05368C9-5069-49CD-B7E8-9CE8C46BB75D'
INSERT INTO [GroupRole] (IsSystem, GroupTypeId, Name, Guid, IsLeader) VALUES (0, @ChildGroupTypeId, 'Member', newid(), 0)
SET @GroupRoleId = SCOPE_IDENTITY()
UPDATE [GroupType] SET DefaultGroupRoleId = @GroupRoleId WHERE Id = @ChildGroupTypeId

INSERT INTO [GroupType] ( [IsSystem],[Name],[Guid],[AllowMultipleLocations],[TakesAttendance],[AttendanceRule],[AttendancePrintTo]) VALUES (0, '4th Grade', NEWID(), 1, 1, 0, 0)
SET @ChildGroupTypeId = SCOPE_IDENTITY()
INSERT INTO [GroupTypeAssociation] VALUES (@ParentGroupTypeId, @ChildGroupTypeId);
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], Guid) SELECT 1, Id, @ChildGroupTypeId, 0, '8.0', newid() FROM Attribute WHERE guid = '63FA25AA-7796-4302-BF05-D96A1C390BD7'
INSERT INTO [AttributeValue] (IsSystem, AttributeId, EntityId, [Order], [Value], Guid) SELECT 1, Id, @ChildGroupTypeId, 0, '11.99', newid() FROM Attribute WHERE guid = 'D05368C9-5069-49CD-B7E8-9CE8C46BB75D'
INSERT INTO [GroupRole] (IsSystem, GroupTypeId, Name, Guid, IsLeader) VALUES (0, @ChildGroupTypeId, 'Member', newid(), 0)
SET @GroupRoleId = SCOPE_IDENTITY()
UPDATE [GroupType] SET DefaultGroupRoleId = @GroupRoleId WHERE Id = @ChildGroupTypeId

DECLARE @GroupId int
INSERT INTO [Group] ( [IsSystem],[GroupTypeId],[Name],[IsSecurityRole],[IsActive],[Guid]) SELECT 0, GT.Id, 'Family Ministries', 0, 1, NEWID() FROM [GroupType] GT WHERE GT.Name = 'Family Ministries'
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [Group] ( [IsSystem],[ParentGroupId],[GroupTypeId],[Name],[IsSecurityRole],[IsActive],[Guid]) SELECT 0, @GroupId, GT.Id, 'Check-in Test (Don''t Use)', 0, 1, NEWID() FROM [GroupType] GT WHERE GT.Name = 'Check-in Test (Don''t Use)'
INSERT INTO [Group] ( [IsSystem],[ParentGroupId],[GroupTypeId],[Name],[IsSecurityRole],[IsActive],[Guid]) SELECT 0, @GroupId, GT.Id, 'Middle Elementary', 0, 1, NEWID() FROM [GroupType] GT WHERE GT.Name = 'Middle Elementary'
SET @GroupId = SCOPE_IDENTITY()
INSERT INTO [Group] ( [IsSystem],[ParentGroupId],[GroupTypeId],[Name],[IsSecurityRole],[IsActive],[Guid]) SELECT 0, @GroupId, GT.Id, '3rd Grade', 0, 1, NEWID() FROM [GroupType] GT WHERE GT.Name = '3rd Grade'
INSERT INTO [Group] ( [IsSystem],[ParentGroupId],[GroupTypeId],[Name],[IsSecurityRole],[IsActive],[Guid]) SELECT 0, @GroupId, GT.Id, '4th Grade', 0, 1, NEWID() FROM [GroupType] GT WHERE GT.Name = '4th Grade'

-- Create Schedules
DELETE [Schedule]
INSERT INTO [Schedule] ([Name],[Frequency],[FrequencyQualifier],[StartTime],[EndTime],[CheckInStartTime],[CheckInEndTime],[Guid],[IsShared])	VALUES ('4:30', 1, 'Saturday', '1/1/1900 4:30pm', '1/1/1900 5:30pm', '1/1/1900 4:00pm', '1/1/1900 5:00pm', NEWID(),1)
INSERT INTO [Schedule] ([Name],[Frequency],[FrequencyQualifier],[StartTime],[EndTime],[CheckInStartTime],[CheckInEndTime],[Guid],[IsShared])	VALUES ('6:00', 1, 'Saturday', '1/1/1900 6:00pm', '1/1/1900 7:00pm', '1/1/1900 5:30pm', '1/1/1900 6:30pm', NEWID(),1)
INSERT INTO [Schedule] ([Name],[Frequency],[FrequencyQualifier],[StartTime],[EndTime],[CheckInStartTime],[CheckInEndTime],[Guid],[IsShared])	VALUES ('9:00', 1, 'Sunday', '1/1/1900 9:00am', '1/1/1900 10:00am', '1/1/1900 8:30am', '1/1/1900 9:30am', NEWID(),1)
INSERT INTO [Schedule] ([Name],[Frequency],[FrequencyQualifier],[StartTime],[EndTime],[CheckInStartTime],[CheckInEndTime],[Guid],[IsShared])	VALUES ('10:30', 1, 'Sunday', '1/1/1900 10:30am', '1/1/1900 11:30am', '1/1/1900 10:00am', '1/1/1900 11:00am', NEWID(),1)
INSERT INTO [Schedule] ([Name],[Frequency],[FrequencyQualifier],[StartTime],[EndTime],[CheckInStartTime],[CheckInEndTime],[Guid],[IsShared])	VALUES ('12:00', 1, 'Sunday', '1/1/1900 12:00pm', '1/1/1900 1:00pm', '1/1/1900 11:30am', '1/1/1900 12:30pm', NEWID(),1)
INSERT INTO [Schedule] ([Name],[Frequency],[FrequencyQualifier],[StartTime],[EndTime],[CheckInStartTime],[CheckInEndTime],[Guid],[IsShared])	VALUES ('4:30 (test)', 0, '', '1/1/1900 12:01am', '1/1/1900 11:59pm', '1/1/1900 12:01am', '1/1/1900 11:59pm', NEWID(),1)
INSERT INTO [Schedule] ([Name],[Frequency],[FrequencyQualifier],[StartTime],[EndTime],[CheckInStartTime],[CheckInEndTime],[Guid],[IsShared])	VALUES ('6:00 (test)', 0, '', '1/1/1900 12:01am', '1/1/1900 11:59pm', '1/1/1900 12:01am', '1/1/1900 11:59pm', NEWID(),1)

-- Create Locations
DELETE [Location]
DECLARE @LocationId int

-- Anderson Locations
INSERT INTO [Location] ([Guid], [Name], [IsActive])	VALUES (NEWID(), 'Anderson', 1)
SET @LocationId = SCOPE_IDENTITY()
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [Guid])	VALUES (@LocationId, 'Bldg 3000 - K-1', 1, NEWID())

-- Greenville Locations
INSERT INTO [Location] ([Guid], [Name], [IsActive])	VALUES (NEWID(), 'Greenville', 1)
SET @LocationId = SCOPE_IDENTITY()
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [Guid]) VALUES (@LocationId, 'Youth Bldg - K-1', 1, NEWID())

-- Spartanburg Locations
INSERT INTO [Location] ([Guid], [Name], [IsActive])	VALUES (NEWID(), 'Spartanburg', 1)
SET @LocationId = SCOPE_IDENTITY()
INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [Guid]) VALUES (@LocationId, 'Bldg A - K-1', 1, NEWID())

INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [Guid]) 
	SELECT ISNULL(BL.Id,CL.Id), 'Shockwave', 1, NEWID()
	FROM [Location] CL
	LEFT OUTER JOIN [Location] BL ON BL.ParentLocationId = CL.Id
	WHERE CL.ParentLocationId IS NULL AND CL.Name = 'Anderson' AND BL.Name = 'Bldg 3000 - K-1' 

INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [Guid]) 
	SELECT ISNULL(BL.Id,CL.Id), 'Shockwave', 1, NEWID()
	FROM [Location] CL
	LEFT OUTER JOIN [Location] BL ON BL.ParentLocationId = CL.Id
	WHERE CL.ParentLocationId IS NULL AND CL.Name = 'Greenville' AND BL.Name = 'Youth Bldg - K-1' 

INSERT INTO [Location] ([ParentLocationId], [Name], [IsActive], [Guid]) 
	SELECT ISNULL(BL.Id,CL.Id), 'Shockwave', 1, NEWID()
	FROM [Location] CL
	LEFT OUTER JOIN [Location] BL ON BL.ParentLocationId = CL.Id
	WHERE CL.ParentLocationId IS NULL AND CL.Name = 'Spartanburg' AND BL.Name = 'Bldg A - K-1' 

DELETE [DeviceLocation]
DELETE [Device]


DECLARE @fieldTypeIdText int = (select Id from FieldType where Guid = '9C204CD0-1233-41C5-818A-C5DA439445AA')

-- Device Types

DECLARE @DeviceTypeValueId int
SET @DeviceTypeValueId = (SELECT [Id] FROM [DefinedValue] WHERE [Guid] = 'BC809626-1389-4543-B8BB-6FAC79C27AFD')
DECLARE @PrinterTypevalueId int
SET @PrinterTypevalueId = (SELECT [Id] FROM [DefinedValue] WHERE [Guid] = '8284B128-E73B-4863-9FC2-43E6827B65E6')

DECLARE @PrinterDeviceId int
INSERT INTO [Device] ([Name],[DeviceTypeValueId],[IPAddress],[PrintFrom],[PrintToOverride],[Guid])
VALUES ('Test Label Printer',@PrinterTypevalueId, '10.1.20.200',0,1,NEWID())
SET @PrinterDeviceId = SCOPE_IDENTITY()

INSERT INTO [Device] ([Name],[DeviceTypeValueId],[PrinterDeviceId],[PrintFrom],[PrintToOverride],[Guid])
SELECT C.Name + ':' + B.Name + ':' + R.Name, @DeviceTypeValueId, @PrinterDeviceId, 0, 1, NEWID()
FROM Location C
INNER JOIN Location B
	ON B.ParentLocationId = C.Id
INNER JOIN Location R
	ON R.ParentLocationId = B.Id

INSERT INTO [DeviceLocation] (DeviceId, LocationId)
SELECT D.Id, R.Id
FROM Location C
INNER JOIN Location B
	ON B.ParentLocationId = C.Id
INNER JOIN Location R
	ON R.ParentLocationId = B.Id
INNER JOIN Device D 
	ON D.Name = C.Name + ':' + B.Name + ':' + R.Name

DELETE [GroupLocation]
INSERT INTO [GroupLocation] (GroupId, LocationId, Guid) 
SELECT G.Id, DL.LocationId, NEWID()
FROM DeviceLocation DL
INNER JOIN [Group] G ON G.Name IN ('3rd Grade', '4th Grade', 'Check-in Test (Don''t Use)')

DELETE [GroupLocationSchedule]

INSERT INTO [GroupLocationSchedule] (GroupLocationId, ScheduleId) 
SELECT GL.Id, S.Id
FROM GroupLocation GL
INNER JOIN [Group] G ON G.Id = GL.GroupId AND G.Name IN ('3rd Grade', '4th Grade')
INNER JOIN Schedule S ON S.[Name] NOT LIKE '%(test)'

INSERT INTO [GroupLocationSchedule] (GroupLocationId, ScheduleId) 
SELECT GL.Id, S.Id
FROM GroupLocation GL
INNER JOIN [Group] G ON G.Id = GL.GroupId AND G.Name = 'Check-in Test (Don''t Use)'
INNER JOIN Schedule S ON S.[Name] LIKE '%(test)'


-- Workflow

-- Workflow Action Entity Types
IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.FilterActiveLocations')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.FilterActiveLocations', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.FilterByAge')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.FilterByAge', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.FindFamilies')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.FindFamilies', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.FindFamilyMembers')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.FindFamilyMembers', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.FindRelationships')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.FindRelationships', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.LoadGroups')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.LoadGroups', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.LoadGroupTypes')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.LoadGroupTypes', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.LoadLocations')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.LoadLocations', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.LoadSchedules')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.LoadSchedules', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.RemoveEmptyGroups')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.RemoveEmptyGroups', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.RemoveEmptyGroupTypes')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.RemoveEmptyGroupTypes', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.RemoveEmptyLocations')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.RemoveEmptyLocations', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.RemoveEmptyPeople')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.RemoveEmptyPeople', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.SaveAttendance')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.SaveAttendance', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.CreateLabels')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.CreateLabels', NEWID(), 0, 0)

IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Workflow.Action.CheckIn.CalculateLastAttended')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Workflow.Action.CheckIn.CalculateLastAttended', NEWID(), 0, 0)

-- Workflow Entity Type
IF NOT EXISTS(SELECT Id FROM EntityType WHERE Name = 'Rock.Model.Workflow')
INSERT INTO EntityType (Name, Guid, IsEntity, IsSecured)
VALUES ('Rock.Model.Workflow', NEWID(), 0, 0)
DECLARE @WorkflowEntityTypeId int
SET @WorkflowEntityTypeId = (SELECT Id FROM EntityType WHERE Name = 'Rock.Model.Workflow')

*/

set identity_insert dbo.workflowactiontype on
insert workflowactiontype (Id, [ActivityTypeId], [Name], [Order], [EntityTypeId], [IsActionCompletedOnSuccess], [IsActivityCompletedOnSuccess], [Guid])
select * from analysis.._workflowactiontype
set identity_insert dbo.workflowactiontype off

update attributevalue set value = 3
where -- blocktype = attended checkin
