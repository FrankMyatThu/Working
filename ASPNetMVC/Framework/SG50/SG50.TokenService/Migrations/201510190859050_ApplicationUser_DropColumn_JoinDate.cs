namespace SG50.TokenService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUser_DropColumn_JoinDate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbl_AppUser", "JoinDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_AppUser", "JoinDate", c => c.DateTime(nullable: false));
        }
    }
}
