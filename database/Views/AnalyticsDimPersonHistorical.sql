IF OBJECT_ID(N'[dbo].[AnalyticsDimPersonHistorical]', 'V') IS NOT NULL
    DROP VIEW AnalyticsDimPersonHistorical
GO

CREATE VIEW AnalyticsDimPersonHistorical
AS
SELECT *
FROM AnalyticsSourcePersonHistorical
