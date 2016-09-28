USE [RockRMS_20160915]
GO
/****** Object:  UserDefinedFunction [dbo].[_church_ccv_ufnActions_Adult_IsServing]    Script Date: 9/28/2016 3:58:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--IsServing: Determines if the given person is serving or not.
ALTER FUNCTION [dbo].[_church_ccv_ufnActions_Adult_IsServing](@PersonId int, @ActiveMemberRequired bit)
RETURNS @IsServingTable TABLE
(
	PersonId int NOT NULL,
	IsServing bit, --True if they are, false if they're not
	GroupIds varchar(MAX) NULL
)
AS
BEGIN

    -- Defined a bit mask that will ensure the GroupMemberStatus
    -- is either ONLY active, or Pending / Active.
    DECLARE @GroupMemberReqMask int

    -- How this works:
    -- Pending MemberStatus = 0011 in binary (3 in decimal)
    -- Active MemberStatus = 0001 in binary  (1 in decimal)
    
    IF @ActiveMemberRequired = 1
        -- Define the mask as 0001, which means it'll only return 1 when masked against Active
        SET @GroupMemberReqMask = 1
    ELSE
        -- Define the mask as 0011, which means 1 is returned when masked against Pending OR Active
        SET @GroupMemberReqMask = 3



    -- Declare group IDs that are valid for "Serving"
	DECLARE @ValidGroupTypeIds table( id int )
	INSERT INTO @ValidGroupTypeIds 
	VALUES
	(23)	--@Serving_GroupTypeId

	--Note: While there is currently only ONE serving group, it encompasses the people
	--that should get serving credit for hosting a group in their home. This works by Rock
	--automatically putting them into a special group (of GroupTypeId 23).

							
	DECLARE @IsServing bit
	DECLARE @GroupIds varchar(MAX)

	--Values we need to build a comma delimited list of groups
	DECLARE @GroupIdTable table( id int )

	--get all the groups the person serves in
	INSERT INTO @GroupIdTable( id )
	SELECT GroupId
	FROM [dbo].GroupMember gm
		INNER JOIN [dbo].[Group] g ON g.Id = gm.GroupId
		INNER JOIN @ValidGroupTypeIds vgt ON vgt.Id = g.GroupTypeId
	WHERE 
		gm.PersonId = @PersonId AND 
		(gm.GroupMemberStatus & @GroupMemberReqMask) != 0 AND --Make sure the status matches the mask requirement
		g.IsActive = 1 --Make sure the GROUP is active

	-- build a comma delimited string with the groups
	SELECT @GroupIds = COALESCE(@GroupIds + ', ', '' ) + CONVERT(nvarchar(MAX), id)
	FROM @GroupIdTable

	-- if there's at least one group, then the person's being mentored
	IF LEN(@GroupIds) > 0
		SET @IsServing = 1
	ELSE
		SET @IsServing = 0

	--Put the results into a single row we'll return
	INSERT INTO @IsServingTable( PersonId, IsServing, GroupIds )
	SELECT 
		@PersonId, 
		@IsServing, 
		@GroupIds

	RETURN;
END
