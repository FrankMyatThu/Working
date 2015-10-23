using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SG50.TokenService.Models.POCO
{
    public class Company
    {
        #region Step 1
        [Key]
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string CompanyName { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string PhoneNo1 { get; set; }

        [MaxLength(100)]
        public string PhoneNo2 { get; set; }

        [MaxLength(100)]
        public string PhoneNo3 { get; set; }

        [MaxLength]
        public string Fax { get; set; }

        [MaxLength]
        public string Website { get; set; }
        #endregion

        #region Step 2
        [MaxLength(255)]
        public string Address { get; set; }

        public int? PostalCode { get; set; }

        [MaxLength(255)]
        public string City { get; set; }

        [MaxLength(36)]
        public string CountryID { get; set; }

        public string Logo { get; set; }
        #endregion

        #region Step 3 Setting
        public int ShoppingCartHoldingTimeInMinute { get; set; }

        public int MaximumDayForPasswordValidity { get; set; }

        public int MaximumAllowedPasswordFailedAttempt { get; set; }

        public int MaximumAllowedDayBeforeDeleteFile { get; set; }

        public int MaximumAllowedUserInactivityDays { get; set; }

        public int MaximumAllowedDayToApproveFile { get; set; }

        public int MaximumDayForPasswordLifeTime { get; set; }

        public bool IsWhiteListIPCheck { get; set; }
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

        public virtual IList<ApplicationUser> ApplicationUser { get; set; }
        public virtual IList<BusinessUnit> BusinessUnit { get; set; }
    }
}