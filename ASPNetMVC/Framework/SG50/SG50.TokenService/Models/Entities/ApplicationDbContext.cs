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

        public DbSet<ActiveUser> ActiveUser { get; set; }
        public DbSet<UsedPassword> UsedPassword { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            //modelBuilder.Configurations.Add(new ApplicationUserMap());

            //modelBuilder.Entity<ApplicationUser>().HasRequired(x => x.ActiveUser).WithRequiredPrincipal(y => y.ApplicationUser);
            
            //// Configure AppUserId as PK for ActiveUser
            //modelBuilder.Entity<ActiveUser>()
            //    .HasKey(ActiveUser => ActiveUser.AppUserId);

            //// Configure AppUserId as FK for ActiveUser
            //modelBuilder.Entity<ApplicationUser>()
            //            .HasOptional(AppUser => AppUser.ActiveUser) // Mark StudentAddress is optional for Student
            //            .WithRequired(ActiveUser => ActiveUser.ApplicationUser); // Create inverse relationship



            /// Main table(s)
            modelBuilder.Entity<ApplicationUser>().ToTable("tbl_AppUser");
            modelBuilder.Entity<IdentityRole>().ToTable("tbl_AppRole");
            modelBuilder.Entity<IdentityUserRole>().ToTable("tbl_AppUserRole");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("tbl_AppUserClaim");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("tbl_AppUserLogin");

            /// Helper table(s)
            modelBuilder.Entity<ActiveUser>().ToTable("tbl_AppActiveUser");
            modelBuilder.Entity<UsedPassword>().ToTable("tbl_AppUsedPassword");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}