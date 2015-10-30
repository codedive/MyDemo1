namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration281020151117 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Services", "ServiceCategoryId", "dbo.ServiceCategories");
            DropIndex("dbo.Services", new[] { "ServiceCategoryId" });
            DropIndex("dbo.Services", new[] { "ParentServiceId" });
            AlterColumn("dbo.Services", "ServiceCategoryId", c => c.Int());
            AlterColumn("dbo.Services", "ParentServiceId", c => c.Int());
            CreateIndex("dbo.Services", "ServiceCategoryId");
            CreateIndex("dbo.Services", "ParentServiceId");
            AddForeignKey("dbo.Services", "ServiceCategoryId", "dbo.ServiceCategories", "ServiceCategoryId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Services", "ServiceCategoryId", "dbo.ServiceCategories");
            DropIndex("dbo.Services", new[] { "ParentServiceId" });
            DropIndex("dbo.Services", new[] { "ServiceCategoryId" });
            AlterColumn("dbo.Services", "ParentServiceId", c => c.Int(nullable: false));
            AlterColumn("dbo.Services", "ServiceCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Services", "ParentServiceId");
            CreateIndex("dbo.Services", "ServiceCategoryId");
            AddForeignKey("dbo.Services", "ServiceCategoryId", "dbo.ServiceCategories", "ServiceCategoryId", cascadeDelete: true);
        }
    }
}
