declare
  @authorizedPersonId int = 1,
  @transactionCounter int = 0,
  @maxTransactionCount int = 200000, -- 4000 families giving 50 times a year
  @maxPersonCountForTransactions int = 4000,  --(select max(id) from Person),
  @transactionDateTime datetime,
  @transactionAmount decimal(18,2),
  @transactionNote nvarchar(max),
  @transactionTypeValueId int = (select Id from DefinedValue where Guid = '2D607262-52D6-4724-910D-5C6E8FB89ACC'),
  @currencyTypeCash int = (select Id from DefinedValue where Guid = 'F3ADC889-1EE8-4EB6-B3FD-8C10F3C8AF93'),
  @creditCardTypeVisa int = (select Id from DefinedValue where Guid = 'FC66B5F8-634F-4800-A60D-436964D27B64'),
  @sourceTypeWeb int = (select Id from DefinedValue where Guid = '7D705CE7-7B11-4342-A58E-53617C5B4E69'),
  @accountId int,
  @transactionId int

begin

begin transaction

/*
 truncate table FinancialTransactionDetail
 delete FinancialTransaction
*/

set @transactionDateTime = DATEADD(DAY, -366, SYSDATETIME())

while @transactionCounter < @maxTransactionCount
    begin
        set @transactionAmount = ROUND(rand() * 5000, 2);
        set @transactionNote = 'Random Note ' + convert(nvarchar(max), rand());
        set @authorizedPersonId = (select top 1 Id from Person where Id >= rand() * @maxPersonCountForTransactions);

        INSERT INTO [dbo].[FinancialTransaction]
                   ([AuthorizedPersonId]
                   ,[BatchId]
                   ,[GatewayId]
                   ,[TransactionDateTime]
                   ,[Amount]
                   ,[TransactionCode]
                   ,[Summary]
                   ,[TransactionTypeValueId]
                   ,[CurrencyTypeValueId]
                   ,[CreditCardTypeValueId]
                   ,[SourceTypeValueId]
                   ,[CheckMicrEncrypted]
                   ,[Guid])
             VALUES
                   (@authorizedPersonId
                   ,null
                   ,null
                   ,@transactionDateTime
                   ,@transactionAmount
                   ,null
                   ,@transactionNote
                   ,@transactionTypeValueId
                   ,@currencyTypeCash
                   ,@creditCardTypeVisa
                   ,@sourceTypeWeb
                   ,null
                   ,NEWID()
        )
        set @transactionId = SCOPE_IDENTITY()
        set @accountId = (select id from FinancialAccount where Id = round(RAND() * 2, 0) + 1)
 
        -- For contributions, we just need to put in the AccountId (entitytype/entityid would be null)
        INSERT INTO [dbo].[FinancialTransactionDetail]
                   ([TransactionId]
                   ,[AccountId]
                   ,[Amount]
                   ,[Summary]
                   ,[EntityTypeId]
                   ,[EntityId]
                   ,[Guid])
             VALUES
                   (@transactionId
                   ,@accountId
                   ,@transactionAmount
                   ,null
                   ,null
                   ,null
                   ,NEWID())

        set @transactionCounter += 1;
        set @transactionDateTime = DATEADD(ss, (86000*365/@maxTransactionCount), @transactionDateTime);
    end

commit transaction

end;


