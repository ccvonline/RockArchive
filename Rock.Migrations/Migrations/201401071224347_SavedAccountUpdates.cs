//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
namespace Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    ///
    /// </summary>
    public partial class SavedAccountUpdates : Rock.Migrations.RockMigration1
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.FinancialPersonSavedAccount", "TransactionCode", c => c.String(maxLength: 50));
            AddColumn("dbo.FinancialPersonSavedAccount", "GatewayEntityTypeId", c => c.Int());
            AddColumn("dbo.FinancialPersonSavedAccount", "CurrencyTypeValueId", c => c.Int());
            AddColumn("dbo.FinancialPersonSavedAccount", "CreditCardTypeValueId", c => c.Int());
            CreateIndex("dbo.FinancialPersonSavedAccount", "CreditCardTypeValueId");
            CreateIndex("dbo.FinancialPersonSavedAccount", "CurrencyTypeValueId");
            CreateIndex("dbo.FinancialPersonSavedAccount", "GatewayEntityTypeId");
            AddForeignKey("dbo.FinancialPersonSavedAccount", "CreditCardTypeValueId", "dbo.DefinedValue", "Id");
            AddForeignKey("dbo.FinancialPersonSavedAccount", "CurrencyTypeValueId", "dbo.DefinedValue", "Id");
            AddForeignKey("dbo.FinancialPersonSavedAccount", "GatewayEntityTypeId", "dbo.EntityType", "Id");

            Sql( @"
    UPDATE S SET
        [TransactionCode] = T.[TransactionCode],
        [GatewayEntityTypeId] = T.[GatewayEntityTypeId],
        [CurrencyTypeValueId] = T.[CurrencyTypeValueId],
        [CreditCardTypeValueId] = T.[CreditCardTypeValueId]
    FROM [FinancialPersonSavedAccount] S
    INNER JOIN [FinancialTransaction] T ON T.[Id] = S.[FinancialTransactionId]
" );

            DropForeignKey("dbo.FinancialPersonSavedAccount", "FinancialTransactionId", "dbo.FinancialTransaction");
            DropIndex("dbo.FinancialPersonSavedAccount", new[] { "FinancialTransactionId" });
            DropColumn("dbo.FinancialPersonSavedAccount", "FinancialTransactionId");
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            AddColumn("dbo.FinancialPersonSavedAccount", "FinancialTransactionId", c => c.Int(nullable: false));

            Sql( @"
    UPDATE S SET
        [FinancialTransactionId] = T.[Id]
    FROM [FinancialPersonSavedAccount] S
    INNER JOIN [FinancialTransaction] T ON T.[TransactionCode] = S.[TransactionCode]
" );

            CreateIndex( "dbo.FinancialPersonSavedAccount", "FinancialTransactionId" );
            AddForeignKey( "dbo.FinancialPersonSavedAccount", "FinancialTransactionId", "dbo.FinancialTransaction", "Id", cascadeDelete: true );

            DropForeignKey( "dbo.FinancialPersonSavedAccount", "GatewayEntityTypeId", "dbo.EntityType" );
            DropForeignKey("dbo.FinancialPersonSavedAccount", "CurrencyTypeValueId", "dbo.DefinedValue");
            DropForeignKey("dbo.FinancialPersonSavedAccount", "CreditCardTypeValueId", "dbo.DefinedValue");
            DropIndex("dbo.FinancialPersonSavedAccount", new[] { "GatewayEntityTypeId" });
            DropIndex("dbo.FinancialPersonSavedAccount", new[] { "CurrencyTypeValueId" });
            DropIndex("dbo.FinancialPersonSavedAccount", new[] { "CreditCardTypeValueId" });
            DropColumn("dbo.FinancialPersonSavedAccount", "CreditCardTypeValueId");
            DropColumn("dbo.FinancialPersonSavedAccount", "CurrencyTypeValueId");
            DropColumn("dbo.FinancialPersonSavedAccount", "GatewayEntityTypeId");
            DropColumn("dbo.FinancialPersonSavedAccount", "TransactionCode");
        }
    }
}
