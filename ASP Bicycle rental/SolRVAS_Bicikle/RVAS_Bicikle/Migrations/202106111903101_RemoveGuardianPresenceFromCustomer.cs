namespace RVAS_Bicikle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveGuardianPresenceFromCustomer : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Customers", "GuardianPresent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "GuardianPresent", c => c.Boolean(nullable: false));
        }
    }
}
