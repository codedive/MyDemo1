namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration28102015213 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Plans", "CreatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Plans", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
