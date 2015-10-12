namespace SG50.TokenService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActiveUser_LastRequestedTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_AppActiveUser", "LastRequestedTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_AppActiveUser", "LastRequestedTime");
        }
    }
}
