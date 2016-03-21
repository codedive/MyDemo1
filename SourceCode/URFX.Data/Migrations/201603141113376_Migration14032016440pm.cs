namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration14032016440pm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Job", "AdditionalTaskDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Job", "AdditionalTaskDescription");
        }
    }
}
