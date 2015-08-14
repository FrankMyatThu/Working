namespace GeekQuiz.TokenServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropColumnToUser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbl_AppUser", "MiddleName");
            Sql(@"
                UPDATE dbo.tbl_AppUser
                SET FirstName = 'Frank',                        
                        LastName = 'Myat Thu',
                        ZipCode = '510415'
                WHERE UserName = 'Myat'    
                ");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_AppUser", "MiddleName", c => c.String());
        }
    }
}
