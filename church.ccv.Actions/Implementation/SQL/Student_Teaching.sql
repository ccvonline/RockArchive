USE [RockRMS_20160915]
GO
/****** Object:  UserDefinedFunction [dbo].[_church_ccv_ufnActions_Student_IsTeaching]    Script Date: 9/25/2016 5:28:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--IsTeaching: Returns true if the person is in at least one "Teacing" group, as a "leader" (see below on that)
ALTER FUNCTION [dbo].[_church_ccv_ufnActions_Student_IsTeaching](@PersonId int)
RETURNS @IsTeachingTable TABLE
(
	PersonId int NOT NULL,

    --Undefined Group: Students will have ONE group type they can be in to teach, but it is currently undefined.
    --For now, I'll stub it out.
    Undefined_IsTeaching bit,
    Undefined_GroupIds varchar(MAX) NULL
    --
)
AS
BEGIN

    -- Note: Being IN the group isn't enough. The person's role in the group
	-- must have the "IsLeader" flag set. 
		
	--Example: An Attendee in a Neighborhood Group wouldn't have true returned.
	-- A Co-Coach in a Neighborhood Group would also _NOT_ have true returned. (Because despite the name, the IsLeader flag is not set on that role type)
	-- A COACH in a Neighborhood Group WOULD have true returned, because they're the only group role type that's flagged as a leader.
	-- I know.

	-- To see which roles count, see Rock -> General Settings-> Group Types-> One of the above Groups.
	-- Expand the Roles panel, and there you'll see which role types are "Leaders" and therefore get Teaching credit.
    
    -- UNDEFINED GROUP
    --------------------
    DECLARE @Undefined_GroupTypeId int = 99999 --Until i's defined, make it an ID that's impossible to have
    DECLARE @Undefined_GroupIds varchar(MAX)
	
    -- Get all groups the person is teaching in
    DECLARE @Undefined_GroupIdTable table( id int )

	INSERT INTO @Undefined_GroupIdTable( id )
	SELECT GroupId
	FROM [dbo].GroupMember gm
		INNER JOIN [dbo].[Group] g ON g.Id = gm.GroupId
		INNER JOIN [dbo].[GroupTypeRole] gtr ON gtr.Id = gm.GroupRoleId
	WHERE 
        g.GroupTypeId = @Undefined_GroupTypeId AND
		gm.PersonId = @PersonId AND 
		gm.GroupMemberStatus != 0 AND --Make sure they aren't InActive. (pending OR active are fine)
        gtr.IsLeader = 1 AND --Make sure their role in the group is Leader
		g.IsActive = 1 --Make sure the GROUP IS active

    -- build a comma delimited string with the groups
	SELECT @Undefined_GroupIds = COALESCE(@Undefined_GroupIds + ', ', '' ) + CONVERT(nvarchar(MAX), id)
	FROM @Undefined_GroupIdTable

    -- if there's at least one group, then the person's teaching
    DECLARE @Undefined_IsTeaching bit
	
	IF LEN(@Undefined_GroupIds) > 0 
		SET @Undefined_IsTeaching = 1
	ELSE
		SET @Undefined_IsTeaching = 0
    ----------------------


	--Put the results into a single row we'll return
	INSERT INTO @IsTeachingTable( 
        PersonId, 
        
        -- Undefined
        Undefined_IsTeaching,
        Undefined_GroupIds
        --
    )
	SELECT 
		@PersonId, 
		
        -- Undefined
        @Undefined_IsTeaching,
        @Undefined_GroupIds
        --

	RETURN;
END
