namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration26102015517 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceId = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 200),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VisitRate = c.Int(nullable: false),
                        HourlyRate = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ServiceCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceId)
                .ForeignKey("dbo.ServiceCategories", t => t.ServiceCategoryId, cascadeDelete: true)
                .Index(t => t.ServiceCategoryId);
            
            CreateTable(
                "dbo.ServiceCategories",
                c => new
                    {
                        ServiceCategoryId = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ServiceCategoryId);
            
            CreateTable(
                "dbo.SubServices",
                c => new
                    {
                        SubServiceId = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 200),
                        ServiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubServiceId)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.ServiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubServices", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Services", "ServiceCategoryId", "dbo.ServiceCategories");
            DropIndex("dbo.SubServices", new[] { "ServiceId" });
            DropIndex("dbo.Services", new[] { "ServiceCategoryId" });
            DropTable("dbo.SubServices");
            DropTable("dbo.ServiceCategories");
            DropTable("dbo.Services");
        }
    }
}
