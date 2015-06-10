-- =============================================
-- Author:		Kinyon, Masaon
-- Create date: 1/27/2015	
-- Description:	Used to generate the person datamart data.
-- =============================================
ALTER PROCEDURE [dbo].[_church_ccv_spDatamart_Person]

AS
BEGIN

	SET NOCOUNT ON;

	TRUNCATE TABLE _church_ccv_Datamart_Person;

	WITH Giving AS
	(
		SELECT *
		FROM (
			SELECT PA.PersonId AS [PersonId], 
				YEAR(FT.TransactionDateTime) AS [year],
				SUM(FTD.Amount) AS [total]
			FROM FinancialTransactionDetail FTD
			INNER JOIN FinancialTransaction FT ON FT.Id = FTD.TransactionId
			INNER JOIN FinancialAccount FA ON FA.Id = FTD.AccountId
			INNER JOIN PersonAlias PA ON PA.AliasPersonId = FT.AuthorizedPersonAliasId
			WHERE FA.Id IN (745,498,609,690,708,727) AND YEAR(FT.TransactionDateTime) >= 2007
			GROUP BY PA.PersonId, YEAR(FT.TransactionDateTime)
		) AS s
		PIVOT
		(
			SUM([total])
			FOR [year] IN ([2015],[2014],[2013],[2012],[2011],[2010],[2009],[2008],[2007])
		) AS s
	)

	INSERT INTO _church_ccv_Datamart_Person
		([PersonId]
		,[FamilyId]
		,[PersonGuid]
		,[FirstName]
		,[NickName]
		,[MiddleName]
		,[LastName]
		,[FullName]
		,[Age]
		,[Grade]
		,[BirthDate]
		,[Gender]
		,[MaritalStatus]
		,[FamilyRole]
		,[Campus]
		,[CampusId]
		,[ConnectionStatus]
        ,[AnniversaryDate]
        ,[AnniversaryYears]
        ,[NeighborhoodId]
        ,[NeighborhoodName]
        ,[TakenStartingPoint]
        ,[StartingPointDate]
        ,[InNeighborhoodGroup]
        ,[NeighborhoodGroupId]
        ,[NeighborhoodGroupName]
        ,[NearestGroupId]
        ,[NearestGroupName]
        ,[FirstVisitDate]
        ,[IsStaff]
        ,[PhotoUrl]
        ,[IsServing]
        ,[IsEra]
        ,[ServingAreas]
		,[CreatedDateTime]
		,[ModifiedDateTime]
		,[CreatedByPersonAliasId]
		,[ModifiedByPersonAliasId]	
		,[SpouseName]
		,[IsHeadOfHousehold]
		,[Address]
		,[City]
		,[State]
		,[PostalCode]
		,[Email]
		,[HomePhone]
		,[CellPhone]
		,[WorkPhone]
		,[IsBaptized]
		,[BaptismDate]
		,[LastContributionDate]
		,[Giving2015]
		,[Giving2014]
		,[Giving2013]
		,[Giving2012]
		,[Giving2011]
		,[Giving2010]
		,[Giving2009]
		,[Giving2008]
		,[Giving2007]
		,[GeoPoint]
		,[Latitude]
		,[Longitude]
		,[Guid]
		,[ForeignId]
		,[ViewedCount])
	SELECT
		P.[Id],
		F.[Id],
		P.[Guid],
		P.[FirstName],
		P.[NickName],
		P.[MiddleName],
		P.[LastName],
		P.[LastName] + ', ' + P.[FirstName], 
		dbo._church_ccv_ufnGetAge(P.BirthDate),
		dbo._church_ccv_ufnGetGrade(P.GraduationYear),
		P.[BirthDate],
		CASE 
			WHEN P.[Gender] = 1 THEN 'Male'
			ELSE 'Female'
		END,
		CAST(MS.[Value] AS varchar(15)),
		FR.Name,
		C.Name,
		F.CampusId,
		CS.Value,
		P.AnniversaryDate,
		YEAR(GETDATE()) - YEAR(P.AnniversaryDate),
		(SELECT TOP 1 Id
		FROM dbo.ufnGroup_GeofencingGroups(L.Id, 48)),
		(SELECT TOP 1 Name
		FROM dbo.ufnGroup_GeofencingGroups(L.Id, 48)),
		CASE
			WHEN (SELECT TOP 1 NG.Id
				 FROM GroupMember NGM
				 LEFT OUTER JOIN [Group] NG
					ON NG.Id = NGM.GroupId 
					AND NG.GroupTypeId = 49
				 WHERE NGM.PersonId = P.Id) IS NOT NULL THEN 1
			ELSE 0
		END,
		(SELECT MAX(A.StartDateTime)
		 FROM Attendance A
		 INNER JOIN [GroupMember] SPM
			ON A.GroupId = SPM.GroupId 
		 INNER JOIN [Group] SP
			ON SP.Id = SPM.GroupId
			AND SP.GroupTypeId = 49
		 WHERE SPM.PersonId = P.Id),
		CASE
			WHEN (SELECT TOP 1 NG.Id
				 FROM GroupMember NGM
				 LEFT OUTER JOIN [Group] NG
					ON NG.Id = NGM.GroupId 
					AND NG.GroupTypeId = 57
				 WHERE NGM.PersonId = P.Id) IS NOT NULL THEN 1
			ELSE 0
		END,--in NH group
		(SELECT TOP 1 NG.Id
		 FROM GroupMember NGM
		 LEFT OUTER JOIN [Group] NG
			ON NG.Id = NGM.GroupId 
			AND NG.GroupTypeId = 49
		 WHERE NGM.PersonId = P.Id),--groupid
		(SELECT TOP 1 NG.Name
		 FROM GroupMember NGM
		 LEFT OUTER JOIN [Group] NG
			ON NG.Id = NGM.GroupId 
			AND NG.GroupTypeId = 49
		 WHERE NGM.PersonId = P.Id),--groupname
		(SELECT TOP 1 G.Id
		 FROM dbo._church_ccv_Datamart_NearestGroup NG
		 INNER JOIN GroupLocation GL 
			ON GL.LocationId = NG.GroupLocationId
		 INNER JOIN [Group] G 
			ON G.Id = GL.GroupId AND G.GroupTypeId = 49
		 WHERE NG.FamilyLocationId = L.Id
		 ORDER BY NG.Distance),--nearest groupid
		(SELECT TOP 1 G.Name
		 FROM dbo._church_ccv_Datamart_NearestGroup NG
		 INNER JOIN GroupLocation GL 
			ON GL.LocationId = NG.GroupLocationId
		 INNER JOIN [Group] G 
			ON G.Id = GL.GroupId AND G.GroupTypeId = 49
		 WHERE NG.FamilyLocationId = L.Id
		 ORDER BY NG.Distance),--nearestgroupname
		FVV.ValueAsDateTime,
		(SELECT TOP 1 OCM.PersonId 
		 FROM GroupMember OCM
		 LEFT OUTER JOIN [Group] OC 
			ON OC.Id = OCM.GroupId 
			AND OC.GroupTypeId = 52
		 WHERE OCM.PersonId = P.Id),
		'http://rock.ccvonline.com/GetImage.ashx?id=' + CAST(P.PhotoId AS VARCHAR(10)),
		(SELECT TOP 1 STM.PersonId 
		 FROM GroupMember STM
		 LEFT OUTER JOIN [Group] ST
			ON ST.Id = STM.GroupId 
			AND ST.GroupTypeId = 26
		 WHERE STM.PersonId = P.Id),
		ERAV.Value,
		STUFF((SELECT ', ' + G.Name
			  FROM [Group] G
			  INNER JOIN [GroupMember] GM
				  ON GM.GroupId = G.Id
			  WHERE G.GroupTypeId = 23
			      AND GM.PersonId = P.Id
				  AND G.IsActive = 1 
			  FOR XML PATH('')), 1, 1, ''),
		P.[CreatedDateTime],
		P.[ModifiedDateTime],
		P.[CreatedByPersonAliasId],
		P.[ModifiedByPersonAliasId],
		(SELECT TOP 1 S.FirstName 
		 FROM Person S 
		 INNER JOIN GroupMember GM ON GM.PersonId = S.Id
		 INNER JOIN [Group] G ON G.Id = GM.GroupId
		 WHERE G.Id = F.Id
			AND S.Gender != P.Gender
			AND FM.GroupRoleId = 3
			AND GM.GroupRoleId = 3),
		CASE
			WHEN(SELECT TOP 1 S.Id
				 FROM Person S 
				 INNER JOIN GroupMember GM ON GM.PersonId = S.Id
				 INNER JOIN [Group] G ON G.Id = GM.GroupId
				 WHERE G.Id = F.Id
					AND FM.GroupRoleId = 3
					AND GM.GroupRoleId = 3
				 ORDER BY GM.GroupRoleId, S.Gender) = P.Id THEN 1
			 ELSE 0
		END,
		L.[Street1] + ' ' + L.[Street2],
		L.[City],
		L.[State],
		L.[PostalCode],
		CASE
			WHEN P.IsEmailActive = 1 THEN P.Email
			WHEN P.IsEmailActive = 0 THEN ''
		END,
		HP.NumberFormatted,
		CP.NumberFormatted,
		WP.NumberFormatted,
		CASE 
			WHEN BD.ValueAsDateTime IS NOT NULL THEN 1
			ELSE 0
		END, 
		BD.ValueAsDateTime,
		(SELECT MAX(FT.TransactionDateTime)
		 FROM FinancialTransactionDetail FTD
		 INNER JOIN FinancialTransaction FT ON FT.Id = FTD.TransactionId
		 INNER JOIN FinancialAccount FA ON FA.Id = FTD.AccountId
		 WHERE FA.Id IN (498,609,690,708,727)
			AND FT.AuthorizedPersonAliasId = P.Id),
		G.[2015],
		G.[2014],
		G.[2013],
		G.[2012],
		G.[2011],
		G.[2010],
		G.[2009],
		G.[2008],
		G.[2007],
		L.GeoPoint,
		L.GeoPoint.Lat,
		L.GeoPoint.Long,
		NEWID(),
		NULL,
		P.[ViewedCount]
	FROM Person P
	INNER JOIN DefinedValue MS ON MS.Id = P.MaritalStatusValueId
	INNER JOIN DefinedValue CS ON CS.Id = P.ConnectionStatusValueId
	LEFT OUTER JOIN AttributeValue BD ON BD.EntityId = P.Id AND BD.AttributeId = 174
	LEFT OUTER JOIN PhoneNumber HP ON HP.PersonId = P.Id AND HP.NumberTypeValueId = 13
	LEFT OUTER JOIN PhoneNumber CP ON CP.PersonId = P.Id AND CP.NumberTypeValueId = 12
	LEFT OUTER JOIN PhoneNumber WP ON WP.PersonId = P.Id AND WP.NumberTypeValueId = 136
	INNER JOIN GroupMember FM ON FM.PersonId = P.Id
	INNER JOIN [Group] F ON F.Id = FM.GroupId AND F.GroupTypeId = 10
	INNER JOIN [Campus] C ON C.Id = F.CampusId
	INNER JOIN GroupTypeRole FR ON FR.Id = FM.GroupRoleId AND FR.GroupTypeId = 10
	LEFT OUTER JOIN [GroupLocation] GL ON GL.GroupId = FM.GroupId AND GL.GroupLocationTypeValueId = 19
	LEFT OUTER JOIN [Location] L ON L.Id = GL.LocationId
	LEFT OUTER JOIN AttributeValue ERAV ON ERAV.EntityId = P.Id AND ERAV.AttributeId = 2533
	LEFT OUTER JOIN AttributeValue FVV ON FVV.EntityId = P.Id AND FVV.AttributeId = 717
	LEFT OUTER JOIN Giving G ON G.PersonId = P.Id
	WHERE P.RecordTypeValueId = 1
		AND P.RecordStatusValueId = 3

END

