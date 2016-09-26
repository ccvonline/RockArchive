USE [RockRMS_20160915]
GO
/****** Object:  UserDefinedFunction [dbo].[_church_ccv_ufnActions_Student_IsMentored]    Script Date: 9/25/2016 4:51:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--IsMentored: Determines if the given person is being "mentored".
--This means they're in at least one group where someone is teaching / mentoring them.
ALTER FUNCTION [dbo].[_church_ccv_ufnActions_Student_IsMentored](@PersonId int)
RETURNS @IsMentoredTable TABLE
(
	PersonId int NOT NULL,

    NextGen_IsMentored bit,
    NextGen_GroupIds varchar(MAX) NULL
)
AS
BEGIN

    -- NextGen GROUP
    --------------------
    DECLARE @NextGen_Group_GroupTypeId int = 94
    DECLARE @NextGen_GroupIds varchar(MAX)
	
    -- Get all groups the person is mentored in
    DECLARE @NextGen_GroupIdTable table( id int )
	INSERT INTO @NextGen_GroupIdTable( id )
	SELECT GroupId
	FROM [dbo].GroupMember gm
		INNER JOIN [dbo].[Group] g ON g.Id = gm.GroupId
        INNER JOIN [dbo].[GroupTypeRole] gtr ON gtr.Id = gm.GroupRoleId
	WHERE 
        g.GroupTypeId = @NextGen_Group_GroupTypeId AND
		gm.PersonId = @PersonId AND 
		gm.GroupMemberStatus != 0 AND --Make sure they aren't InActive. (pending OR active are fine)
        gtr.IsLeader = 0 AND --Make sure their role in the group is NOT Leader
		g.IsActive = 1 --Make sure the GROUP IS active

    -- build a comma delimited string with the groups
	SELECT @NextGen_GroupIds = COALESCE(@NextGen_GroupIds + ', ', '' ) + CONVERT(nvarchar(MAX), id)
	FROM @NextGen_GroupIdTable

    -- if there's at least one group, then the person's mentored
    DECLARE @NextGen_IsMentored bit
	
	IF LEN(@NextGen_GroupIds) > 0 
		SET @NextGen_IsMentored = 1
	ELSE
		SET @NextGen_IsMentored = 0
    ----------------------


	--Put the results into a single row we'll return
	INSERT INTO @IsMentoredTable( 
        PersonId, 
        
        NextGen_IsMentored, 
        NextGen_GroupIds
        )
	SELECT 
		@PersonId, 

        @NextGen_IsMentored,
        @NextGen_GroupIds

	RETURN;
END
