namespace RVAS_Bicikle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGuardianPresenceToCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "GuardianPresent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "GuardianPresent");
        }
    }
}
