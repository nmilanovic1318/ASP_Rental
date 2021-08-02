namespace RVAS_Bicikle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveGuardianPresenceFromRental : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Rentals", "DateReturned", c => c.DateTime());
            DropColumn("dbo.Rentals", "UnderAgeGuardianPresent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rentals", "UnderAgeGuardianPresent", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Rentals", "DateReturned", c => c.DateTime(nullable: false));
        }
    }
}
