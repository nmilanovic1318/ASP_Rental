namespace RVAS_Bicikle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplyDataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bicycles", "Frame", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Brands", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Customers", "Surname", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Customers", "PhoneNumber", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Customers", "Address", c => c.String(nullable: false, maxLength: 75));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "Address", c => c.String());
            AlterColumn("dbo.Customers", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Customers", "Surname", c => c.String());
            AlterColumn("dbo.Customers", "Name", c => c.String());
            AlterColumn("dbo.Brands", "Name", c => c.String());
            AlterColumn("dbo.Bicycles", "Frame", c => c.String());
        }
    }
}
