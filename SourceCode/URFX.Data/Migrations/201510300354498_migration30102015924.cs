namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration30102015924 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Countries", newName: "Country");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Country", newName: "Countries");
        }
    }
}
