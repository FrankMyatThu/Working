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
        #region User Profile Area

        #region Step 1 (General Info)
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public string SaltKey { get; set; }        
        #endregion

        #region Step 2 (General Info)
        [MaxLength(100)]
        public string NickName { get; set; }

        [MaxLength(255)]
        public string Photo { get; set; }

        [MaxLength]
        public string CCMail { get; set; }

        public DateTime? DOB { get; set; }

        public bool IsReceivedEmail { get; set; }
        #endregion

        #region Step 3 (Location Info)
        [MaxLength]
        public string Fax { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        public int? PostalCode { get; set; }

        [MaxLength(255)]
        public string City { get; set; }

        [MaxLength(36)]
        public string CountryID { get; set; }
        #endregion

        #region Step 4 (Company Info)
        [MaxLength(36)]
        public string CompanyID { get; set; }

        [MaxLength(36)]
        public string BusinessUnitID { get; set; }
        #endregion

        #region Step 5 (Optional)
        [MaxLength]
        public string Remark { get; set; }
        #endregion

        #endregion

        #region User Setting (Only administrator can configure)
        [MaxLength(36)]
        public string GroupID { get; set; }
        public bool IsLocked { get; set; }
        public bool IsEnable { get; set; }
        public DateTime? LastLoginTime { get; set; }
        #endregion

        #region Default table attributes
        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        #endregion

        public virtual IList<UsedPassword> UserUsedPassword { get; set; }
        public virtual IList<ActiveUser> ActiveUser { get; set; }
    }
}