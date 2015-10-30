namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration29102015102 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceProviderServiceMapping", "Service_ServiceId", "dbo.Services");
            DropIndex("dbo.ServiceProviderServiceMapping", new[] { "Service_ServiceId" });
            DropColumn("dbo.ServiceProviderServiceMapping", "ServiceId");
            RenameColumn(table: "dbo.ServiceProviderServiceMapping", name: "Service_ServiceId", newName: "ServiceId");
            AlterColumn("dbo.ServiceProviderServiceMapping", "ServiceId", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceProviderServiceMapping", "ServiceId", c => c.Int(nullable: false));
            CreateIndex("dbo.ServiceProviderServiceMapping", "ServiceId");
            AddForeignKey("dbo.ServiceProviderServiceMapping", "ServiceId", "dbo.Services", "ServiceId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceProviderServiceMapping", "ServiceId", "dbo.Services");
            DropIndex("dbo.ServiceProviderServiceMapping", new[] { "ServiceId" });
            AlterColumn("dbo.ServiceProviderServiceMapping", "ServiceId", c => c.Int());
            AlterColumn("dbo.ServiceProviderServiceMapping", "ServiceId", c => c.String(maxLength: 128));
            RenameColumn(table: "dbo.ServiceProviderServiceMapping", name: "ServiceId", newName: "Service_ServiceId");
            AddColumn("dbo.ServiceProviderServiceMapping", "ServiceId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ServiceProviderServiceMapping", "Service_ServiceId");
            AddForeignKey("dbo.ServiceProviderServiceMapping", "Service_ServiceId", "dbo.Services", "ServiceId");
        }
    }
}
