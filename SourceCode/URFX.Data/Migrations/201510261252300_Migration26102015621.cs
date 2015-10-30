namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration26102015621 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        PlanId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Detail = c.String(nullable: false),
                        ApplicationFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TeamRegistrationFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TeamRegistrationType = c.Int(nullable: false),
                        PerVisitPercentage = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PlanId);
            
            CreateTable(
                "dbo.UserPlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlanId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(),
                        ExpiredDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Plans", t => t.PlanId, cascadeDelete: true)
                .Index(t => t.PlanId)
                .Index(t => t.UserId);
            
            AlterColumn("dbo.Services", "Description", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.ServiceCategories", "Description", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.SubServices", "Description", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPlans", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.UserPlans", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserPlans", new[] { "UserId" });
            DropIndex("dbo.UserPlans", new[] { "PlanId" });
            AlterColumn("dbo.SubServices", "Description", c => c.String(maxLength: 200));
            AlterColumn("dbo.ServiceCategories", "Description", c => c.String(maxLength: 200));
            AlterColumn("dbo.Services", "Description", c => c.String(maxLength: 200));
            DropTable("dbo.UserPlans");
            DropTable("dbo.Plans");
        }
    }
}
