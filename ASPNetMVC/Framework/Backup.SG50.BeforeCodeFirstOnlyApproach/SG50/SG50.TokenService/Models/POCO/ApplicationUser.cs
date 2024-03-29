﻿using Microsoft.AspNet.Identity;
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
        public bool IsNotifyCCMails { get; set; }
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
        //[ForeignKey("Company")]
        //public Guid CompanyID { get; set; }

        //[ForeignKey("BusinessUnit")]
        //public string BusinessUnitID { get; set; }
        #endregion

        #region Step 5 (Optional)
        [MaxLength]
        public string Remark { get; set; }
        #endregion

        #endregion

        #region User Setting (Only administrator can configure)
        //[ForeignKey("Group")]
        //public string GroupID { get; set; }
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
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        #endregion

        //public virtual Company Company { get; set; }
        //public virtual Group Group { get; set; }
        //public virtual BusinessUnit BusinessUnit { get; set; } 
        public virtual IList<UserUsedPassword> UserUsedPassword { get; set; }
        public virtual IList<ActiveUser> ActiveUser { get; set; }
    }
}