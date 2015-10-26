namespace SG50.TokenService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_ActiveUser",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AppUserId = c.String(maxLength: 128),
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
                .ForeignKey("dbo.tbl_User", t => t.AppUserId)
                .Index(t => t.AppUserId);
            
            CreateTable(
                "dbo.tbl_User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        SaltKey = c.String(nullable: false),
                        NickName = c.String(maxLength: 100),
                        Photo = c.String(maxLength: 255),
                        CCMail = c.String(),
                        DOB = c.DateTime(),
                        IsReceivedEmail = c.Boolean(nullable: false),
                        IsNotifyCCMails = c.Boolean(nullable: false),
                        Fax = c.String(),
                        Address = c.String(maxLength: 255),
                        PostalCode = c.Int(),
                        City = c.String(maxLength: 255),
                        CountryID = c.String(maxLength: 36),
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
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Company_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_Company", t => t.Company_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.tbl_AppUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.tbl_AppUserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.tbl_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.tbl_AppUserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.tbl_User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.tbl_AppRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.tbl_UserUsedPassword",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AppUserId = c.String(maxLength: 128),
                        Password = c.String(),
                        SaltKey = c.String(),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_User", t => t.AppUserId)
                .Index(t => t.AppUserId);
            
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
                        Email = c.String(maxLength: 256),
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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tbl_AppRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
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
            DropForeignKey("dbo.tbl_AppUserRole", "RoleId", "dbo.tbl_AppRole");
            DropForeignKey("dbo.tbl_GroupPermission", "RoleId", "dbo.tbl_Role");
            DropForeignKey("dbo.tbl_RolePermission", "RoleId", "dbo.tbl_Role");
            DropForeignKey("dbo.tbl_RolePermission", "ProgramMenuId", "dbo.tbl_ProgramMenu");
            DropForeignKey("dbo.tbl_GroupPermission", "GroupId", "dbo.tbl_Group");
            DropForeignKey("dbo.tbl_Group", "CompanyId", "dbo.tbl_Company");
            DropForeignKey("dbo.tbl_BusinessUnit", "CompanyId", "dbo.tbl_Company");
            DropForeignKey("dbo.tbl_User", "Company_Id", "dbo.tbl_Company");
            DropForeignKey("dbo.tbl_ActiveUser", "AppUserId", "dbo.tbl_User");
            DropForeignKey("dbo.tbl_UserUsedPassword", "AppUserId", "dbo.tbl_User");
            DropForeignKey("dbo.tbl_AppUserRole", "UserId", "dbo.tbl_User");
            DropForeignKey("dbo.tbl_AppUserLogin", "UserId", "dbo.tbl_User");
            DropForeignKey("dbo.tbl_AppUserClaim", "UserId", "dbo.tbl_User");
            DropIndex("dbo.tbl_WhiteListIP", new[] { "CompanyId" });
            DropIndex("dbo.tbl_AppRole", "RoleNameIndex");
            DropIndex("dbo.tbl_RolePermission", new[] { "ProgramMenuId" });
            DropIndex("dbo.tbl_RolePermission", new[] { "RoleId" });
            DropIndex("dbo.tbl_GroupPermission", new[] { "RoleId" });
            DropIndex("dbo.tbl_GroupPermission", new[] { "GroupId" });
            DropIndex("dbo.tbl_Group", new[] { "CompanyId" });
            DropIndex("dbo.tbl_BusinessUnit", new[] { "CompanyId" });
            DropIndex("dbo.tbl_UserUsedPassword", new[] { "AppUserId" });
            DropIndex("dbo.tbl_AppUserRole", new[] { "RoleId" });
            DropIndex("dbo.tbl_AppUserRole", new[] { "UserId" });
            DropIndex("dbo.tbl_AppUserLogin", new[] { "UserId" });
            DropIndex("dbo.tbl_AppUserClaim", new[] { "UserId" });
            DropIndex("dbo.tbl_User", new[] { "Company_Id" });
            DropIndex("dbo.tbl_User", "UserNameIndex");
            DropIndex("dbo.tbl_ActiveUser", new[] { "AppUserId" });
            DropTable("dbo.tbl_WhiteListIP");
            DropTable("dbo.tbl_AppRole");
            DropTable("dbo.tbl_ProgramMenu");
            DropTable("dbo.tbl_RolePermission");
            DropTable("dbo.tbl_Role");
            DropTable("dbo.tbl_GroupPermission");
            DropTable("dbo.tbl_Group");
            DropTable("dbo.tbl_Company");
            DropTable("dbo.tbl_BusinessUnit");
            DropTable("dbo.tbl_UserUsedPassword");
            DropTable("dbo.tbl_AppUserRole");
            DropTable("dbo.tbl_AppUserLogin");
            DropTable("dbo.tbl_AppUserClaim");
            DropTable("dbo.tbl_User");
            DropTable("dbo.tbl_ActiveUser");
        }
    }
}
