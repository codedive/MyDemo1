namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration291020151230 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceProviderServiceMapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceProviderId = c.String(maxLength: 128),
                        ServiceId = c.String(maxLength: 128),
                        Service_ServiceId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.Service_ServiceId)
                .ForeignKey("dbo.ServiceProvider", t => t.ServiceProviderId)
                .Index(t => t.ServiceProviderId)
                .Index(t => t.Service_ServiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceProviderServiceMapping", "ServiceProviderId", "dbo.ServiceProvider");
            DropForeignKey("dbo.ServiceProviderServiceMapping", "Service_ServiceId", "dbo.Services");
            DropIndex("dbo.ServiceProviderServiceMapping", new[] { "Service_ServiceId" });
            DropIndex("dbo.ServiceProviderServiceMapping", new[] { "ServiceProviderId" });
            DropTable("dbo.ServiceProviderServiceMapping");
        }
    }
}
