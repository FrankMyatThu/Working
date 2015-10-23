namespace SG50.TokenService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUser_AddMoreColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_AppActiveUser", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.tbl_AppActiveUser", "UpdatedBy", c => c.String());
            AddColumn("dbo.tbl_AppUser", "NickName", c => c.String(maxLength: 100));
            AddColumn("dbo.tbl_AppUser", "Photo", c => c.String(maxLength: 255));
            AddColumn("dbo.tbl_AppUser", "CCMail", c => c.String());
            AddColumn("dbo.tbl_AppUser", "DOB", c => c.DateTime());
            AddColumn("dbo.tbl_AppUser", "IsReceivedEmail", c => c.Boolean(nullable: false));
            AddColumn("dbo.tbl_AppUser", "Fax", c => c.String());
            AddColumn("dbo.tbl_AppUser", "Address", c => c.String(maxLength: 255));
            AddColumn("dbo.tbl_AppUser", "PostalCode", c => c.Int());
            AddColumn("dbo.tbl_AppUser", "City", c => c.String(maxLength: 255));
            AddColumn("dbo.tbl_AppUser", "CountryID", c => c.String(maxLength: 36));
            AddColumn("dbo.tbl_AppUser", "CompanyID", c => c.String(maxLength: 36));
            AddColumn("dbo.tbl_AppUser", "BusinessUnitID", c => c.String(maxLength: 36));
            AddColumn("dbo.tbl_AppUser", "Remark", c => c.String());
            AddColumn("dbo.tbl_AppUser", "GroupID", c => c.String(maxLength: 36));
            AddColumn("dbo.tbl_AppUser", "IsLocked", c => c.Boolean(nullable: false));
            AddColumn("dbo.tbl_AppUser", "IsEnable", c => c.Boolean(nullable: false));
            AddColumn("dbo.tbl_AppUser", "LastLoginTime", c => c.DateTime());
            AddColumn("dbo.tbl_AppUser", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.tbl_AppUser", "UpdatedBy", c => c.String());
            AddColumn("dbo.tbl_AppUsedPassword", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tbl_AppUsedPassword", "UpdatedBy", c => c.String());
            DropColumn("dbo.tbl_AppActiveUser", "UpdateDate");
            DropColumn("dbo.tbl_AppActiveUser", "UpdateBy");
            DropColumn("dbo.tbl_AppUser", "PasswordResetToken");
            DropColumn("dbo.tbl_AppUser", "UpdateDate");
            DropColumn("dbo.tbl_AppUser", "UpdateBy");
            DropColumn("dbo.tbl_AppUsedPassword", "UpdateDate");
            DropColumn("dbo.tbl_AppUsedPassword", "UpdateBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_AppUsedPassword", "UpdateBy", c => c.String());
            AddColumn("dbo.tbl_AppUsedPassword", "UpdateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tbl_AppUser", "UpdateBy", c => c.String());
            AddColumn("dbo.tbl_AppUser", "UpdateDate", c => c.DateTime());
            AddColumn("dbo.tbl_AppUser", "PasswordResetToken", c => c.String());
            AddColumn("dbo.tbl_AppActiveUser", "UpdateBy", c => c.String());
            AddColumn("dbo.tbl_AppActiveUser", "UpdateDate", c => c.DateTime());
            DropColumn("dbo.tbl_AppUsedPassword", "UpdatedBy");
            DropColumn("dbo.tbl_AppUsedPassword", "UpdatedDate");
            DropColumn("dbo.tbl_AppUser", "UpdatedBy");
            DropColumn("dbo.tbl_AppUser", "UpdatedDate");
            DropColumn("dbo.tbl_AppUser", "LastLoginTime");
            DropColumn("dbo.tbl_AppUser", "IsEnable");
            DropColumn("dbo.tbl_AppUser", "IsLocked");
            DropColumn("dbo.tbl_AppUser", "GroupID");
            DropColumn("dbo.tbl_AppUser", "Remark");
            DropColumn("dbo.tbl_AppUser", "BusinessUnitID");
            DropColumn("dbo.tbl_AppUser", "CompanyID");
            DropColumn("dbo.tbl_AppUser", "CountryID");
            DropColumn("dbo.tbl_AppUser", "City");
            DropColumn("dbo.tbl_AppUser", "PostalCode");
            DropColumn("dbo.tbl_AppUser", "Address");
            DropColumn("dbo.tbl_AppUser", "Fax");
            DropColumn("dbo.tbl_AppUser", "IsReceivedEmail");
            DropColumn("dbo.tbl_AppUser", "DOB");
            DropColumn("dbo.tbl_AppUser", "CCMail");
            DropColumn("dbo.tbl_AppUser", "Photo");
            DropColumn("dbo.tbl_AppUser", "NickName");
            DropColumn("dbo.tbl_AppActiveUser", "UpdatedBy");
            DropColumn("dbo.tbl_AppActiveUser", "UpdatedDate");
        }
    }
}
