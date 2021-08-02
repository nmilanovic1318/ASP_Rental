namespace RVAS_Bicikle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGuardianPresenceToRental : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rentals", "UnderAgeGuardianPresent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rentals", "UnderAgeGuardianPresent");
        }
    }
}
