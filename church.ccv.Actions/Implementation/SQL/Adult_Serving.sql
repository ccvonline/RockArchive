USE [RockRMS_20160915]
GO
/****** Object:  UserDefinedFunction [dbo].[_church_ccv_ufnActions_Adult_IsServing]    Script Date: 9/25/2016 5:01:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--IsServing: Determines if the given person is serving or not.
ALTER FUNCTION [dbo].[_church_ccv_ufnActions_Adult_IsServing](@PersonId int)
RETURNS @IsServingTable TABLE
(
	PersonId int NOT NULL,
	IsServing bit, --True if they are, false if they're not
	GroupIds varchar(MAX) NULL
)
AS
BEGIN

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
		gm.GroupMemberStatus != 0 AND --Make sure they aren't InActive (Pending and Active are fine)
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
