IF OBJECT_ID(N'[dbo].[AnalyticsDimPersonHistorical]', 'V') IS NOT NULL
    DROP VIEW AnalyticsDimPersonHistorical
GO

CREATE VIEW AnalyticsDimPersonHistorical
AS
SELECT asph.*
    ,ms.NAME [MaritalStatus]
    ,cs.NAME [ConnectionStatus]
    ,rr.NAME [ReviewReason]
    ,rs.NAME [RecordStatus]
    ,rsr.NAME [RecordStatusReason]
    ,rt.NAME [RecordType]
    ,ps.NAME [Suffix]
    ,pt.NAME [Title]
    ,CASE asph.Gender
        WHEN 1
            THEN 'Male'
        WHEN 2
            THEN 'Female'
        ELSE 'Unknown'
        END [GenderText]
    ,CASE asph.EmailPreference
        WHEN 0
            THEN 'Email Allowed'
        WHEN 1
            THEN 'No Mass Emails'
        WHEN 2
            THEN 'Do Not Email'
        ELSE 'Unknown'
        END [EmailPreferenceText]
	,fc.Id [PrimaryFamilyKey] 
FROM AnalyticsSourcePersonHistorical asph
LEFT JOIN AnalyticsDimPersonMaritalStatus ms ON ms.MaritalStatusId = asph.MaritalStatusValueId
LEFT JOIN AnalyticsDimPersonConnectionStatus cs ON cs.ConnectionStatusId = asph.ConnectionStatusValueId
LEFT JOIN AnalyticsDimPersonReviewReason rr ON rr.ReviewReasonId = asph.ReviewReasonValueId
LEFT JOIN AnalyticsDimPersonRecordStatus rs ON rs.RecordStatusId = asph.RecordStatusValueId
LEFT JOIN AnalyticsDimPersonRecordStatusReason rsr ON rsr.RecordStatusReasonId = asph.RecordStatusReasonValueId
LEFT JOIN AnalyticsDimPersonRecordType rt ON rt.RecordTypeId = asph.RecordTypeValueId
LEFT JOIN AnalyticsDimPersonSuffix ps ON ps.SuffixId = asph.SuffixValueId
LEFT JOIN AnalyticsDimPersonTitle pt ON pt.TitleId = asph.TitleValueId
left join AnalyticsDimFamilyCurrent fc on fc.GroupId = asph.PrimaryFamilyId
