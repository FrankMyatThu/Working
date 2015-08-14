namespace GeekQuiz.TokenServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExtraColumnToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_AppUser", "MiddleName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_AppUser", "MiddleName");
        }
    }
}
