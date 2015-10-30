namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration271020151238 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubServices", "ServiceId", "dbo.Services");
            DropIndex("dbo.SubServices", new[] { "ServiceId" });
            AddColumn("dbo.Services", "ParentServiceId", c => c.Int(nullable: false));
            CreateIndex("dbo.Services", "ParentServiceId");
            AddForeignKey("dbo.Services", "ParentServiceId", "dbo.Services", "ServiceId");
            DropTable("dbo.SubServices");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SubServices",
                c => new
                    {
                        SubServiceId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 200),
                        ServiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubServiceId);
            
            DropForeignKey("dbo.Services", "ParentServiceId", "dbo.Services");
            DropIndex("dbo.Services", new[] { "ParentServiceId" });
            DropColumn("dbo.Services", "ParentServiceId");
            CreateIndex("dbo.SubServices", "ServiceId");
            AddForeignKey("dbo.SubServices", "ServiceId", "dbo.Services", "ServiceId", cascadeDelete: true);
        }
    }
}
