using GeekQuiz.TokenServer.Models.POCO;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GeekQuiz.TokenServer.Models.Entity
{
    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        public AuthContext()
            : base("AuthContext")
        {

        }
        
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            modelBuilder.Entity<ApplicationUser>().ToTable("tbl_AppUser");
            modelBuilder.Entity<IdentityRole>().ToTable("tbl_AppRole");
            modelBuilder.Entity<IdentityUserRole>().ToTable("tbl_AppUserRole");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("tbl_AppUserClaim");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("tbl_AppUserLogin");
            modelBuilder.Entity<Client>().ToTable("tbl_AppClient");
        }

        public static AuthContext Create()
        {
            return new AuthContext();
        }
        
    }
}