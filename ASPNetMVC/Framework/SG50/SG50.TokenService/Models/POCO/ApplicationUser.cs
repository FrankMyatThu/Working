using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SG50.TokenService.Models.POCO
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        public string PasswordResetToken { get; set; }

        [Required]
        [MaxLength(100)]
        public string LoginName { get; set; }

        [Required]        
        public string Password { get; set; }

        [Required]
        public string SaltKey { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }

        [Required]
        public virtual IList<UsedPassword> UserUsedPassword { get; set; }
        
        [Required]
        public virtual IList<ActiveUser> ActiveUser { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here

            return userIdentity;
        }
    }

    class ApplicationUserMap : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserMap()
        {
            //this.HasKey(c => c.Id);
            //this.Property(c => c.Id)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //this.HasRequired(x => x.ActiveUser).WithRequiredPrincipal(y => y.ApplicationUser);
        }
    }
}