using Microsoft.AspNet.Identity.EntityFramework;
using SG50.TokenService.Models.POCO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SG50.TokenService.Models.Entities
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        static string ConnectionName = "AuthContext";

        public ApplicationDbContext()
            : base(ConnectionName, throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        /// Helper table(s)
        public DbSet<ActiveUser> ActiveUser { get; set; }
        public DbSet<UserUsedPassword> UserUsedPassword { get; set; }

        /// Company table(s)
        public DbSet<Company> Company { get; set; }
        public DbSet<BusinessUnit> BusinessUnit { get; set; }
        public DbSet<WhiteListIP> WhiteListIP { get; set; }

        /// Menu program role table(s)
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupPermission> GroupPermission { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<ProgramMenu> ProgramMenu { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            /// Main table(s)
            modelBuilder.Entity<ApplicationUser>().ToTable("tbl_User");
            modelBuilder.Entity<IdentityRole>().ToTable("tbl_AppRole");
            modelBuilder.Entity<IdentityUserRole>().ToTable("tbl_AppUserRole");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("tbl_AppUserClaim");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("tbl_AppUserLogin");
            //modelBuilder.Ignore<IdentityRole>();
            //modelBuilder.Ignore<IdentityUserRole>();
            //modelBuilder.Ignore<IdentityUserClaim>();            
            //modelBuilder.Ignore<IdentityUserLogin>();            

            /// Helper table(s)
            modelBuilder.Entity<ActiveUser>().ToTable("tbl_ActiveUser");
            modelBuilder.Entity<UserUsedPassword>().ToTable("tbl_UserUsedPassword");

            /// Company table(s)
            modelBuilder.Entity<Company>().ToTable("tbl_Company");
            modelBuilder.Entity<BusinessUnit>().ToTable("tbl_BusinessUnit");
            modelBuilder.Entity<WhiteListIP>().ToTable("tbl_WhiteListIP");

            /// Menu program role table(s)
            modelBuilder.Entity<Group>().ToTable("tbl_Group");
            modelBuilder.Entity<GroupPermission>().ToTable("tbl_GroupPermission");
            modelBuilder.Entity<Role>().ToTable("tbl_Role");
            modelBuilder.Entity<RolePermission>().ToTable("tbl_RolePermission");
            modelBuilder.Entity<ProgramMenu>().ToTable("tbl_ProgramMenu");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}