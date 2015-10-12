namespace SG50.TokenService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActiveUser_LastRequestedTime : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE tbl_AppActiveUser ADD LastRequestedTime DATETIME NOT NULL DEFAULT (GETDATE())");
        }
        
        public override void Down()
        {

        }
    }
}
