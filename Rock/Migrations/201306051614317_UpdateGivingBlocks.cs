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
    public partial class UpdateGivingBlocks : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            DeleteBlock( "3BFFEDFD-2198-4A13-827A-4FD1A774949E" ); // One Time Gift
            DeleteBlock( "0F17BF49-A6D5-47C3-935A-B050127EA939" ); // Recurring Gift
            DeleteBlockType( "4A2AA794-A968-4CCD-973A-C90FD589996F" ); // Finance - One Time Gift
            DeleteBlockType( "F679692F-133E-4F57-9072-D87C675C3283" ); // Finance - Recurring Gift
            DeletePage( "9800CE96-C99B-4C70-ADD9-DF22E89378D4" ); // One Time Gift
            DeletePage( "FF73E611-2674-4BA1-A75F-E9291FAC0E19" ); // Recurring Gift            

            AddPage( "142627AE-6590-48E3-BFCA-3669260B8CF2", "Scheduled Contributions", "", "Default", "F23C5BF7-4F52-448F-8445-6BAEEE3030AB" );
            AddPage( "142627AE-6590-48E3-BFCA-3669260B8CF2", "Make a Contribution", "", "Default", "1615E090-1889-42FF-AB18-5F7BE9F24498" );
            AddBlockType( "Finance - Giving Profile Detail", "Giving profile details UI", "~/Blocks/Finance/GivingProfileDetail.ascx", "B343E2B7-0AD0-49B8-B78E-E47BD42171A7" );
            AddBlockType( "Finance - Giving Profile List", "", "~/Blocks/Finance/GivingProfileList.ascx", "694FF260-8C6F-4A59-93C9-CF3793FE30E6" );
            AddBlock( "F23C5BF7-4F52-448F-8445-6BAEEE3030AB", "694FF260-8C6F-4A59-93C9-CF3793FE30E6", "Scheduled Contributions", "", "Content", 0, "32A7BA7B-968E-4BFD-BEA3-042CF863D751" );
            AddBlock( "1615E090-1889-42FF-AB18-5F7BE9F24498", "B343E2B7-0AD0-49B8-B78E-E47BD42171A7", "Contributions", "", "Content", 0, "20C12A0F-BEC1-4620-9273-EEFE4CFB1D96" );

            AddDefinedType( "Financial", "Payment Type", "The type of payment associated with a transaction", "23E80D98-017E-47B9-BAF3-AC442A1EC3EE" );
            AddDefinedValue( "23E80D98-017E-47B9-BAF3-AC442A1EC3EE", "Credit Card", "Credit Card payment type", "09412338-AAAA-4644-BA2A-4CADBE653468" );
            AddDefinedValue( "23E80D98-017E-47B9-BAF3-AC442A1EC3EE", "Checking/ACH", "Checking/ACH payment type", "FFAD975C-7504-418F-8959-30BD0C62CD30" );

            // Remove current transaction frequency types
            DeleteDefinedValue( "35711E44-131B-4534-B0B2-F0A749292362" );
            DeleteDefinedValue( "72990023-0D43-4554-8D32-28461CAB8920" );
            DeleteDefinedValue( "1400753C-A0F9-4A45-8A1D-81C98450BD1F" );
            DeleteDefinedValue( "791C863D-2600-445B-98F8-3E5B66A3DEC4" );

            // Add & re-order transaction frequency types
            AddDefinedValue( "1F645CFB-5BBD-4465-B9CA-0D2104A1479B", "One-Time", "One Time", "82614683-7FB4-4F16-9087-6F85945A7B16" );
            AddDefinedValue( "1F645CFB-5BBD-4465-B9CA-0D2104A1479B", "One-Time (Future)", "One Time Future", "A5A12067-322E-44A4-94C4-561312F9913C" );
            AddDefinedValue( "1F645CFB-5BBD-4465-B9CA-0D2104A1479B", "Weekly", "Every Week", "35711E44-131B-4534-B0B2-F0A749292362" );
            AddDefinedValue( "1F645CFB-5BBD-4465-B9CA-0D2104A1479B", "Bi-Weekly", "Every Two Weeks", "72990023-0D43-4554-8D32-28461CAB8920" );            
            AddDefinedValue( "1F645CFB-5BBD-4465-B9CA-0D2104A1479B", "Twice a Month", "Twice a Month", "791C863D-2600-445B-98F8-3E5B66A3DEC4" );
            AddDefinedValue( "1F645CFB-5BBD-4465-B9CA-0D2104A1479B", "Monthly", "Once a Month", "1400753C-A0F9-4A45-8A1D-81C98450BD1F" );
                      

        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            
            DeleteDefinedType( "23E80D98-017E-47B9-BAF3-AC442A1EC3EE" ); // Payment Type
            DeleteDefinedValue( "09412338-AAAA-4644-BA2A-4CADBE653468" );
            DeleteDefinedValue( "FFAD975C-7504-418F-8959-30BD0C62CD30" );

            DeleteDefinedValue( "82614683-7FB4-4F16-9087-6F85945A7B16" ); // Frequency Type
            DeleteDefinedValue( "A5A12067-322E-44A4-94C4-561312F9913C" );

            DeleteBlock( "32A7BA7B-968E-4BFD-BEA3-042CF863D751" ); // Scheduled Contributions
            DeleteBlock( "20C12A0F-BEC1-4620-9273-EEFE4CFB1D96" ); // Contributions
            DeleteBlockType( "B343E2B7-0AD0-49B8-B78E-E47BD42171A7" ); // Finance - Giving Profile Detail
            DeleteBlockType( "694FF260-8C6F-4A59-93C9-CF3793FE30E6" ); // Finance - Giving Profile List
            DeletePage( "F23C5BF7-4F52-448F-8445-6BAEEE3030AB" ); // Scheduled Contributions
            DeletePage( "1615E090-1889-42FF-AB18-5F7BE9F24498" ); // Make a Contribution

            AddPage( "142627AE-6590-48E3-BFCA-3669260B8CF2", "One Time Gift", "", "Default", "9800CE96-C99B-4C70-ADD9-DF22E89378D4" );
            AddPage( "142627AE-6590-48E3-BFCA-3669260B8CF2", "Recurring Gift", "", "Default", "FF73E611-2674-4BA1-A75F-E9291FAC0E19" );
            AddBlockType( "Finance - One Time Gift", "", "~/Blocks/Finance/OneTimeGift.ascx", "4A2AA794-A968-4CCD-973A-C90FD589996F" );
            AddBlockType( "Finance - Recurring Gift", "", "~/Blocks/Finance/RecurringGift.ascx", "F679692F-133E-4F57-9072-D87C675C3283" );
            AddBlock( "9800CE96-C99B-4C70-ADD9-DF22E89378D4", "4A2AA794-A968-4CCD-973A-C90FD589996F", "One Time Gift", "", "Content", 0, "3BFFEDFD-2198-4A13-827A-4FD1A774949E" );
            AddBlock( "FF73E611-2674-4BA1-A75F-E9291FAC0E19", "F679692F-133E-4F57-9072-D87C675C3283", "Recurring Gift", "", "Content", 0, "0F17BF49-A6D5-47C3-935A-B050127EA939" );
        }
    }
}
