namespace RVAS_Bicikle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateBicycleBrands : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Brands(Name) VALUES ('BMX')");
            Sql("INSERT INTO Brands(Name) VALUES ('Raleigh')");
            Sql("INSERT INTO Brands(Name) VALUES ('Focus')");
            Sql("INSERT INTO Brands(Name) VALUES ('Trek')");
            Sql("INSERT INTO Brands(Name) VALUES ('GT')");
        }
        
        public override void Down()
        {
            Sql("TRUNCATE TABLE dbo.Brands");
        }
    }
}
