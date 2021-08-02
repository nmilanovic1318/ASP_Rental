namespace RVAS_Bicikle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRepairsTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Services", "BicycleId", "dbo.Bicycles");
            DropIndex("dbo.Services", new[] { "BicycleId" });
            CreateTable(
                "dbo.Repairs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateOfRepair = c.DateTime(nullable: false),
                        BicycleId = c.Int(nullable: false),
                        Price = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bicycles", t => t.BicycleId, cascadeDelete: true)
                .Index(t => t.BicycleId);
            
            DropTable("dbo.Services");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateServiced = c.DateTime(nullable: false),
                        BicycleId = c.Int(nullable: false),
                        Price = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Repairs", "BicycleId", "dbo.Bicycles");
            DropIndex("dbo.Repairs", new[] { "BicycleId" });
            DropTable("dbo.Repairs");
            CreateIndex("dbo.Services", "BicycleId");
            AddForeignKey("dbo.Services", "BicycleId", "dbo.Bicycles", "Id", cascadeDelete: true);
        }
    }
}
