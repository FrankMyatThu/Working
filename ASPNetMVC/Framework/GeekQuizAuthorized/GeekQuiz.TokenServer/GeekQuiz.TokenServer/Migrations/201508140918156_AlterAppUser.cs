namespace GeekQuiz.TokenServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterAppUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_AppUser", "FirstName", c => c.String());
            AddColumn("dbo.tbl_AppUser", "LastName", c => c.String());
            AddColumn("dbo.tbl_AppUser", "ZipCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_AppUser", "ZipCode");
            DropColumn("dbo.tbl_AppUser", "LastName");
            DropColumn("dbo.tbl_AppUser", "FirstName");
        }
    }
}
