namespace SG50.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Setup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_ActiveUser",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        IP = c.String(),
                        UserAgent = c.String(),
                        JwtHMACKey = c.String(),
                        LastRequestedTime = c.DateTime(nullable: false),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.tbl_User",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Email = c.String(maxLength: 450),
                        HashedPassword = c.String(),
                        SaltKey = c.String(nullable: false),
                        NickName = c.String(maxLength: 100),
                        Photo = c.String(maxLength: 255),
                        CCMail = c.String(),
                        DOB = c.DateTime(),
                        IsReceivedEmail = c.Boolean(nullable: false),
                        IsNotifyCCMails = c.Boolean(nullable: false),
                        Phone = c.String(maxLength: 100),
                        Fax = c.String(maxLength: 100),
                        Address = c.String(maxLength: 255),
                        PostalCode = c.Int(),
                        City = c.String(maxLength: 255),
                        Remark = c.String(),
                        IsLocked = c.Boolean(nullable: false),
                        IsEnable = c.Boolean(nullable: false),
                        LastLoginTime = c.DateTime(),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                        tbl_Company_Id = c.Guid(),
                        tbl_BusinessUnit_Id = c.Guid(),
                        tbl_Country_Id = c.Guid(),
                        tbl_Group_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_Company", t => t.tbl_Company_Id)
                .ForeignKey("dbo.tbl_BusinessUnit", t => t.tbl_BusinessUnit_Id)
                .ForeignKey("dbo.tbl_Country", t => t.tbl_Country_Id)
                .ForeignKey("dbo.tbl_Group", t => t.tbl_Group_Id)
                .Index(t => t.Email, unique: true)
                .Index(t => t.tbl_Company_Id)
                .Index(t => t.tbl_BusinessUnit_Id)
                .Index(t => t.tbl_Country_Id)
                .Index(t => t.tbl_Group_Id);
            
            CreateTable(
                "dbo.tbl_BusinessUnit",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        BusinessUnitName = c.String(maxLength: 255),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_Company", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.tbl_Company",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyName = c.String(maxLength: 100),
                        Email = c.String(maxLength: 450),
                        PhoneNo1 = c.String(maxLength: 100),
                        PhoneNo2 = c.String(maxLength: 100),
                        PhoneNo3 = c.String(maxLength: 100),
                        Fax = c.String(),
                        Website = c.String(),
                        Address = c.String(maxLength: 255),
                        PostalCode = c.Int(),
                        City = c.String(maxLength: 255),
                        CountryID = c.String(maxLength: 36),
                        Logo = c.String(),
                        ShoppingCartHoldingTimeInMinute = c.Int(nullable: false),
                        MaximumDayForPasswordValidity = c.Int(nullable: false),
                        MaximumAllowedPasswordFailedAttempt = c.Int(nullable: false),
                        MaximumAllowedDayBeforeDeleteFile = c.Int(nullable: false),
                        MaximumAllowedUserInactivityDays = c.Int(nullable: false),
                        MaximumAllowedDayToApproveFile = c.Int(nullable: false),
                        MaximumDayForPasswordLifeTime = c.Int(nullable: false),
                        IsWhiteListIPCheck = c.Boolean(nullable: false),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tbl_Country",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 255),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tbl_Group",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        Name = c.String(maxLength: 255),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_Company", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.tbl_GroupPermission",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GroupId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_Group", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.tbl_Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.tbl_Role",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 255),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tbl_RolePermission",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        ProgramMenuId = c.Guid(nullable: false),
                        IsAllowedCreate = c.Boolean(nullable: false),
                        IsAllowedRetrieve = c.Boolean(nullable: false),
                        IsAllowedUpdate = c.Boolean(nullable: false),
                        IsAllowedDelete = c.Boolean(nullable: false),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_ProgramMenu", t => t.ProgramMenuId, cascadeDelete: true)
                .ForeignKey("dbo.tbl_Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.ProgramMenuId);
            
            CreateTable(
                "dbo.tbl_ProgramMenu",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ModuleId = c.Guid(nullable: false),
                        ProgramURI = c.String(maxLength: 255),
                        ProgramName = c.String(maxLength: 255),
                        MenuName = c.String(maxLength: 255),
                        LevelIndex = c.Int(nullable: false),
                        IsShow = c.Boolean(nullable: false),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_Module", t => t.ModuleId, cascadeDelete: true)
                .Index(t => t.ModuleId);
            
            CreateTable(
                "dbo.tbl_Module",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        ModuleName = c.String(maxLength: 255),
                        LevelIndex = c.Int(nullable: false),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_Application", t => t.ApplicationId, cascadeDelete: true)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.tbl_Application",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApplicationName = c.String(maxLength: 255),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tbl_UserUsedPassword",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        HashedPassword = c.String(),
                        SaltKey = c.String(),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.tbl_WhiteListIP",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        IPAddress = c.String(maxLength: 255),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_Company", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbl_WhiteListIP", "CompanyId", "dbo.tbl_Company");
            DropForeignKey("dbo.tbl_ActiveUser", "UserId", "dbo.tbl_User");
            DropForeignKey("dbo.tbl_UserUsedPassword", "UserId", "dbo.tbl_User");
            DropForeignKey("dbo.tbl_User", "tbl_Group_Id", "dbo.tbl_Group");
            DropForeignKey("dbo.tbl_GroupPermission", "RoleId", "dbo.tbl_Role");
            DropForeignKey("dbo.tbl_RolePermission", "RoleId", "dbo.tbl_Role");
            DropForeignKey("dbo.tbl_RolePermission", "ProgramMenuId", "dbo.tbl_ProgramMenu");
            DropForeignKey("dbo.tbl_ProgramMenu", "ModuleId", "dbo.tbl_Module");
            DropForeignKey("dbo.tbl_Module", "ApplicationId", "dbo.tbl_Application");
            DropForeignKey("dbo.tbl_GroupPermission", "GroupId", "dbo.tbl_Group");
            DropForeignKey("dbo.tbl_Group", "CompanyId", "dbo.tbl_Company");
            DropForeignKey("dbo.tbl_User", "tbl_Country_Id", "dbo.tbl_Country");
            DropForeignKey("dbo.tbl_User", "tbl_BusinessUnit_Id", "dbo.tbl_BusinessUnit");
            DropForeignKey("dbo.tbl_BusinessUnit", "CompanyId", "dbo.tbl_Company");
            DropForeignKey("dbo.tbl_User", "tbl_Company_Id", "dbo.tbl_Company");
            DropIndex("dbo.tbl_WhiteListIP", new[] { "CompanyId" });
            DropIndex("dbo.tbl_UserUsedPassword", new[] { "UserId" });
            DropIndex("dbo.tbl_Module", new[] { "ApplicationId" });
            DropIndex("dbo.tbl_ProgramMenu", new[] { "ModuleId" });
            DropIndex("dbo.tbl_RolePermission", new[] { "ProgramMenuId" });
            DropIndex("dbo.tbl_RolePermission", new[] { "RoleId" });
            DropIndex("dbo.tbl_GroupPermission", new[] { "RoleId" });
            DropIndex("dbo.tbl_GroupPermission", new[] { "GroupId" });
            DropIndex("dbo.tbl_Group", new[] { "CompanyId" });
            DropIndex("dbo.tbl_BusinessUnit", new[] { "CompanyId" });
            DropIndex("dbo.tbl_User", new[] { "tbl_Group_Id" });
            DropIndex("dbo.tbl_User", new[] { "tbl_Country_Id" });
            DropIndex("dbo.tbl_User", new[] { "tbl_BusinessUnit_Id" });
            DropIndex("dbo.tbl_User", new[] { "tbl_Company_Id" });
            DropIndex("dbo.tbl_User", new[] { "Email" });
            DropIndex("dbo.tbl_ActiveUser", new[] { "UserId" });
            DropTable("dbo.tbl_WhiteListIP");
            DropTable("dbo.tbl_UserUsedPassword");
            DropTable("dbo.tbl_Application");
            DropTable("dbo.tbl_Module");
            DropTable("dbo.tbl_ProgramMenu");
            DropTable("dbo.tbl_RolePermission");
            DropTable("dbo.tbl_Role");
            DropTable("dbo.tbl_GroupPermission");
            DropTable("dbo.tbl_Group");
            DropTable("dbo.tbl_Country");
            DropTable("dbo.tbl_Company");
            DropTable("dbo.tbl_BusinessUnit");
            DropTable("dbo.tbl_User");
            DropTable("dbo.tbl_ActiveUser");
        }
    }
}
