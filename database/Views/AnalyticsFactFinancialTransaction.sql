IF OBJECT_ID(N'[dbo].[AnalyticsFactFinancialTransaction]', 'V') IS NOT NULL
    DROP VIEW AnalyticsFactFinancialTransaction
GO

--select count(*) from AnalyticsFactFinancialTransaction 2185726
CREATE VIEW AnalyticsFactFinancialTransaction
AS
SELECT asft.*
    ,tt.NAME [TransactionType]
    ,ts.NAME [TransactionSource]
    ,CASE asft.IsScheduled
        WHEN 0
            THEN 'Scheduled'
        ELSE 'Non-Scheduled'
        END [ScheduleType]
    ,adpcAuthorizedPerson.[Id] [AuthorizedPersonKey]
    ,paAuthorizedPerson.PersonId [AuthorizedCurrentPersonId]
    ,NULL [ProcessedByPersonKey] -- TODO
    ,NULL [GivingUnitKey] -- TODO
    ,fg.NAME [FinancialGateway]
    ,et.NAME [EntityTypeName]
    ,ct.NAME [CurrencyType]
    ,cct.NAME [CreditCardType]
FROM AnalyticsSourceFinancialTransaction asft
JOIN AnalyticsDimFinancialTransactionType tt ON tt.TransactionTypeId = asft.TransactionTypeValueId
LEFT JOIN AnalyticsDimFinancialTransactionSource ts ON ts.SourceId = asft.SourceTypeValueId
LEFT JOIN AnalyticsDimFinancialTransactionCurrencyType ct ON ct.CurrencyTypeId = asft.CurrencyTypeValueId
LEFT JOIN AnalyticsDimFinancialTransactionCreditCardType cct ON cct.CreditCardTypeId = asft.CreditCardTypeValueId
JOIN PersonAlias paAuthorizedPerson ON asft.AuthorizedPersonAliasId = paAuthorizedPerson.Id
JOIN Person p ON p.Id = paAuthorizedPerson.PersonId
LEFT JOIN AnalyticsDimPersonCurrent adpcAuthorizedPerson ON adpcAuthorizedPerson.PersonId = paAuthorizedPerson.PersonId
LEFT JOIN PersonAlias paProcessedByPerson ON asft.ProcessedByPersonAliasId = paProcessedByPerson.Id
LEFT JOIN AnalyticsDimPersonCurrent adpcProcessedByPerson ON adpcProcessedByPerson.PersonId = paProcessedByPerson.PersonId
LEFT JOIN FinancialGateway fg ON asft.FinancialGatewayId = fg.Id
LEFT JOIN EntityType et ON et.Id = asft.EntityTypeId
