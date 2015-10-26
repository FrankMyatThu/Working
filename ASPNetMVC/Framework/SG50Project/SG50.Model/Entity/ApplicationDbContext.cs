using SG50.Model.POCO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG50.Model.Entity
{
    public class ApplicationDbContext : DbContext
    {
        static string ConnectionName = "DbConnectionString";

        public ApplicationDbContext()
            : base(ConnectionName)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        #region Administration Area
        /// Company table(s)
        public DbSet<tbl_Company> tbl_Company { get; set; }
        public DbSet<tbl_BusinessUnit> tbl_BusinessUnit { get; set; }
        public DbSet<tbl_WhiteListIP> tbl_WhiteListIP { get; set; }
        public DbSet<tbl_Country> tbl_Country { get; set; }

        /// User tables(s)
        public DbSet<tbl_User> tbl_User { get; set; }
        public DbSet<tbl_ActiveUser> tbl_ActiveUser { get; set; }
        public DbSet<tbl_UserUsedPassword> tbl_UserUsedPassword { get; set; }

        /// Menu program role table(s)
        public DbSet<tbl_Group> tbl_Group { get; set; }
        public DbSet<tbl_GroupPermission> tbl_GroupPermission { get; set; }
        public DbSet<tbl_Role> tbl_Role { get; set; }
        public DbSet<tbl_RolePermission> tbl_RolePermission { get; set; }
        public DbSet<tbl_ProgramMenu> tbl_ProgramMenu { get; set; }

        /// Application level table(s)
        public DbSet<tbl_Module> tbl_Module { get; set; }
        public DbSet<tbl_Application> tbl_Application { get; set; }
        #endregion

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
