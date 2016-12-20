IF OBJECT_ID(N'[dbo].[AnalyticsFactFinancialTransaction]', 'V') IS NOT NULL
    DROP VIEW AnalyticsFactFinancialTransaction
GO

--select count(*) from AnalyticsFactFinancialTransaction 2185726
CREATE VIEW AnalyticsFactFinancialTransaction
AS
SELECT asft.*
    ,isnull(tt.NAME, 'None') [TransactionType]
    ,isnull(ts.NAME, 'None') [TransactionSource]
    ,CASE asft.IsScheduled
        WHEN 1
            THEN 'Scheduled'
        ELSE 'Non-Scheduled'
        END [ScheduleType]
    ,adfcAuthorizedFamilyKey.Id [AuthorizedFamilyKey]
	,adpcProcessedByPerson.Id [ProcessedByPersonKey]
    ,adfcGivingUnit.Id [GivingUnitKey]
    ,isnull(fg.NAME, 'None') [FinancialGateway]
    ,isnull(et.NAME, 'None') [EntityTypeName]
    ,isnull(ct.NAME, 'None') [CurrencyType]
    ,isnull(cct.NAME, 'None') [CreditCardType]
FROM AnalyticsSourceFinancialTransaction asft
JOIN AnalyticsDimFinancialTransactionType tt ON tt.TransactionTypeId = asft.TransactionTypeValueId
LEFT JOIN AnalyticsDimFinancialTransactionSource ts ON ts.SourceId = asft.SourceTypeValueId
LEFT JOIN AnalyticsDimFinancialTransactionCurrencyType ct ON ct.CurrencyTypeId = asft.CurrencyTypeValueId
LEFT JOIN AnalyticsDimFinancialTransactionCreditCardType cct ON cct.CreditCardTypeId = asft.CreditCardTypeValueId
LEFT JOIN PersonAlias paProcessedByPerson ON asft.ProcessedByPersonAliasId = paProcessedByPerson.Id
LEFT JOIN AnalyticsDimPersonCurrent adpcProcessedByPerson ON adpcProcessedByPerson.PersonId = paProcessedByPerson.PersonId
LEFT JOIN AnalyticsDimFamilyCurrent adfcGivingUnit ON adfcGivingUnit.GroupId = asft.GivingGroupId
LEFT JOIN AnalyticsDimFamilyCurrent adfcAuthorizedFamilyKey ON adfcAuthorizedFamilyKey.GroupId = asft.AuthorizedFamilyId
LEFT JOIN FinancialGateway fg ON asft.FinancialGatewayId = fg.Id
LEFT JOIN EntityType et ON et.Id = asft.EntityTypeId
