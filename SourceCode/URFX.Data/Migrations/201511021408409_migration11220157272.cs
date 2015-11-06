namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration11220157272 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserLocation",
                c => new
                    {
                        UserLocationId = c.Int(nullable: false, identity: true),
                        CityId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.UserLocationId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Districts", t => t.Id, cascadeDelete: false)
                .Index(t => t.CityId)
                .Index(t => t.Id)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLocation", "Id", "dbo.Districts");
            DropForeignKey("dbo.UserLocation", "CityId", "dbo.Cities");
            DropForeignKey("dbo.UserLocation", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserLocation", new[] { "UserId" });
            DropIndex("dbo.UserLocation", new[] { "Id" });
            DropIndex("dbo.UserLocation", new[] { "CityId" });
            DropTable("dbo.UserLocation");
        }
    }
}
