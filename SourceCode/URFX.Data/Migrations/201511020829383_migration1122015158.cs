namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration1122015158 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserPlans", "NumberOfTeams", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserPlans", "NumberOfTeams");
        }
    }
}
