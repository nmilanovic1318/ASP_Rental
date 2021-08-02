namespace RVAS_Bicikle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModelToBicycle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bicycles", "Model", c => c.String(nullable: false, maxLength: 25));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bicycles", "Model");
        }
    }
}
