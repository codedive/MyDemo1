namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration11220157273 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.UserLocation", name: "Id", newName: "DistrictId");
            RenameIndex(table: "dbo.UserLocation", name: "IX_Id", newName: "IX_DistrictId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.UserLocation", name: "IX_DistrictId", newName: "IX_Id");
            RenameColumn(table: "dbo.UserLocation", name: "DistrictId", newName: "Id");
        }
    }
}
