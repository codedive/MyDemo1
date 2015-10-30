namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration281020151113 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Services", "CreatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Services", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
