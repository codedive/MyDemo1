namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration1122015727 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.City", newName: "Cities");
            RenameTable(name: "dbo.Client", newName: "Clients");
            DropForeignKey("dbo.District", "CityId", "dbo.City");            
            DropIndex("dbo.District", new[] { "CityId" });           
            DropTable("dbo.District");            
        }
        
        public override void Down()
        {     
            CreateTable(
                "dbo.District",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        CityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);        
            CreateIndex("dbo.District", "CityId");            
            AddForeignKey("dbo.District", "CityId", "dbo.City", "CityId", cascadeDelete: true);
            RenameTable(name: "dbo.Clients", newName: "Client");
            RenameTable(name: "dbo.Cities", newName: "City");
        }
    }
}
