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
        static string ConnectionName = "DefaultConnection";

        public ApplicationDbContext()
            : base(ConnectionName, throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        //public DbSet<ActiveUser> ActiveUser { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            modelBuilder.Entity<ApplicationUser>().ToTable("tbl_AppUser");
            modelBuilder.Entity<IdentityRole>().ToTable("tbl_AppRole");
            modelBuilder.Entity<IdentityUserRole>().ToTable("tbl_AppUserRole");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("tbl_AppUserClaim");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("tbl_AppUserLogin");
            modelBuilder.Entity<ActiveUser>().ToTable("tbl_AppActiveUser");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}