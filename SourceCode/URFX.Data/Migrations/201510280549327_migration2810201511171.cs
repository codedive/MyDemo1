namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration2810201511171 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Services", "ParentServiceId", "dbo.Services");
            DropIndex("dbo.Services", new[] { "ParentServiceId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Services", "ParentServiceId");
            AddForeignKey("dbo.Services", "ParentServiceId", "dbo.Services", "ServiceId");
        }
    }
}
