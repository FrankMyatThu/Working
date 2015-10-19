namespace SG50.TokenService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SG50DB_Migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_AppActiveUser",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AppUserId = c.String(nullable: false, maxLength: 128),
                        IP = c.String(),
                        UserAgent = c.String(),
                        JwtHMACKey = c.String(),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdateDate = c.DateTime(),
                        UpdateBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_AppUser", t => t.AppUserId, cascadeDelete: true)
                .Index(t => t.AppUserId);
            
            CreateTable(
                "dbo.tbl_AppUser",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        SaltKey = c.String(nullable: false),
                        JoinDate = c.DateTime(nullable: false),
                        PasswordResetToken = c.String(),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdateDate = c.DateTime(),
                        UpdateBy = c.String(),
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
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
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
                .ForeignKey("dbo.tbl_AppUser", t => t.UserId, cascadeDelete: true)
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
                .ForeignKey("dbo.tbl_AppUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.tbl_AppUserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.tbl_AppUser", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.tbl_AppRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.tbl_AppUsedPassword",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AppUserId = c.String(nullable: false, maxLength: 128),
                        Password = c.String(),
                        SaltKey = c.String(),
                        ExecutedTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdateBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbl_AppUser", t => t.AppUserId, cascadeDelete: true)
                .Index(t => t.AppUserId);
            
            CreateTable(
                "dbo.tbl_AppRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbl_AppUserRole", "RoleId", "dbo.tbl_AppRole");
            DropForeignKey("dbo.tbl_AppActiveUser", "AppUserId", "dbo.tbl_AppUser");
            DropForeignKey("dbo.tbl_AppUsedPassword", "AppUserId", "dbo.tbl_AppUser");
            DropForeignKey("dbo.tbl_AppUserRole", "UserId", "dbo.tbl_AppUser");
            DropForeignKey("dbo.tbl_AppUserLogin", "UserId", "dbo.tbl_AppUser");
            DropForeignKey("dbo.tbl_AppUserClaim", "UserId", "dbo.tbl_AppUser");
            DropIndex("dbo.tbl_AppRole", "RoleNameIndex");
            DropIndex("dbo.tbl_AppUsedPassword", new[] { "AppUserId" });
            DropIndex("dbo.tbl_AppUserRole", new[] { "RoleId" });
            DropIndex("dbo.tbl_AppUserRole", new[] { "UserId" });
            DropIndex("dbo.tbl_AppUserLogin", new[] { "UserId" });
            DropIndex("dbo.tbl_AppUserClaim", new[] { "UserId" });
            DropIndex("dbo.tbl_AppUser", "UserNameIndex");
            DropIndex("dbo.tbl_AppActiveUser", new[] { "AppUserId" });
            DropTable("dbo.tbl_AppRole");
            DropTable("dbo.tbl_AppUsedPassword");
            DropTable("dbo.tbl_AppUserRole");
            DropTable("dbo.tbl_AppUserLogin");
            DropTable("dbo.tbl_AppUserClaim");
            DropTable("dbo.tbl_AppUser");
            DropTable("dbo.tbl_AppActiveUser");
        }
    }
}
