namespace SG50.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pending_script : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_ActiveUser", "CreatedBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbl_ActiveUser", "UpdatedBy", c => c.Guid());
            AlterColumn("dbo.tbl_User", "CreatedBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbl_User", "UpdatedBy", c => c.Guid());
            AlterColumn("dbo.tbl_BusinessUnit", "CreatedBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbl_BusinessUnit", "UpdatedBy", c => c.Guid());
            AlterColumn("dbo.tbl_Company", "CreatedBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbl_Company", "UpdatedBy", c => c.Guid());
            AlterColumn("dbo.tbl_Country", "CreatedBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbl_Country", "UpdatedBy", c => c.Guid());
            AlterColumn("dbo.tbl_Group", "CreatedBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbl_Group", "UpdatedBy", c => c.Guid());
            AlterColumn("dbo.tbl_GroupPermission", "CreatedBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbl_GroupPermission", "UpdatedBy", c => c.Guid());
            AlterColumn("dbo.tbl_Role", "CreatedBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbl_Role", "UpdatedBy", c => c.Guid());
            AlterColumn("dbo.tbl_RolePermission", "CreatedBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbl_RolePermission", "UpdatedBy", c => c.Guid());
            AlterColumn("dbo.tbl_ProgramMenu", "CreatedBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbl_ProgramMenu", "UpdatedBy", c => c.Guid());
            AlterColumn("dbo.tbl_Module", "CreatedBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbl_Module", "UpdatedBy", c => c.Guid());
            AlterColumn("dbo.tbl_Application", "CreatedBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbl_Application", "UpdatedBy", c => c.Guid());
            AlterColumn("dbo.tbl_UserUsedPassword", "CreatedBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbl_UserUsedPassword", "UpdatedBy", c => c.Guid());
            AlterColumn("dbo.tbl_WhiteListIP", "CreatedBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.tbl_WhiteListIP", "UpdatedBy", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_WhiteListIP", "UpdatedBy", c => c.String());
            AlterColumn("dbo.tbl_WhiteListIP", "CreatedBy", c => c.String());
            AlterColumn("dbo.tbl_UserUsedPassword", "UpdatedBy", c => c.String());
            AlterColumn("dbo.tbl_UserUsedPassword", "CreatedBy", c => c.String());
            AlterColumn("dbo.tbl_Application", "UpdatedBy", c => c.String());
            AlterColumn("dbo.tbl_Application", "CreatedBy", c => c.String());
            AlterColumn("dbo.tbl_Module", "UpdatedBy", c => c.String());
            AlterColumn("dbo.tbl_Module", "CreatedBy", c => c.String());
            AlterColumn("dbo.tbl_ProgramMenu", "UpdatedBy", c => c.String());
            AlterColumn("dbo.tbl_ProgramMenu", "CreatedBy", c => c.String());
            AlterColumn("dbo.tbl_RolePermission", "UpdatedBy", c => c.String());
            AlterColumn("dbo.tbl_RolePermission", "CreatedBy", c => c.String());
            AlterColumn("dbo.tbl_Role", "UpdatedBy", c => c.String());
            AlterColumn("dbo.tbl_Role", "CreatedBy", c => c.String());
            AlterColumn("dbo.tbl_GroupPermission", "UpdatedBy", c => c.String());
            AlterColumn("dbo.tbl_GroupPermission", "CreatedBy", c => c.String());
            AlterColumn("dbo.tbl_Group", "UpdatedBy", c => c.String());
            AlterColumn("dbo.tbl_Group", "CreatedBy", c => c.String());
            AlterColumn("dbo.tbl_Country", "UpdatedBy", c => c.String());
            AlterColumn("dbo.tbl_Country", "CreatedBy", c => c.String());
            AlterColumn("dbo.tbl_Company", "UpdatedBy", c => c.String());
            AlterColumn("dbo.tbl_Company", "CreatedBy", c => c.String());
            AlterColumn("dbo.tbl_BusinessUnit", "UpdatedBy", c => c.String());
            AlterColumn("dbo.tbl_BusinessUnit", "CreatedBy", c => c.String());
            AlterColumn("dbo.tbl_User", "UpdatedBy", c => c.String());
            AlterColumn("dbo.tbl_User", "CreatedBy", c => c.String());
            AlterColumn("dbo.tbl_ActiveUser", "UpdatedBy", c => c.String());
            AlterColumn("dbo.tbl_ActiveUser", "CreatedBy", c => c.String());
        }
    }
}
