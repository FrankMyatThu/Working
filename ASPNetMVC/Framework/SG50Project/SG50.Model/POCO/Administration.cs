using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG50.Model.POCO
{
    #region Company table(s)
    public class tbl_Company {

        #region Step 1
        [Key]
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string CompanyName { get; set; }

        [MaxLength(450)]
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

        public virtual IList<tbl_User> tbl_User { get; set; }
        public virtual IList<tbl_BusinessUnit> tbl_BusinessUnit { get; set; }  
 
    }
    public class tbl_BusinessUnit {

        [Key]
        public Guid Id { get; set; }

        [ForeignKey("tbl_Company")]
        public Guid CompanyId { get; set; }

        [MaxLength(255)]
        public string BusinessUnitName { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual tbl_Company tbl_Company { get; set; }

    }
    public class tbl_WhiteListIP {

        [Key]
        public Guid Id { get; set; }

        [ForeignKey("tbl_Company")]
        public Guid CompanyId { get; set; }

        [MaxLength(255)]
        public string IPAddress { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual tbl_Company tbl_Company { get; set; }

    }
    public class tbl_Country {

        [Key]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

    }
    #endregion

    #region User tables(s)
    public class tbl_User {

        #region User Profile Area

        #region Step 1 (General Info)
        [Key]
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(450)]
        [Index(IsUnique = true)]        
        public string Email { get; set; }

        [MaxLength]
        public string HashedPassword { get; set; }

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
        [MaxLength(100)]
        public string Phone { get; set; }

        [MaxLength(100)]
        public string Fax { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        public int? PostalCode { get; set; }

        [MaxLength(255)]
        public string City { get; set; }

        //[ForeignKey("tbl_Country")]
        //public Guid CountryID { get; set; }
        #endregion

        #region Step 4 (Company Info)
        //[ForeignKey("tbl_Company")]
        //public Guid CompanyID { get; set; }

        //[ForeignKey("tbl_BusinessUnit")]
        //public Guid BusinessUnitID { get; set; }
        #endregion

        #region Step 5 (Optional)
        [MaxLength]
        public string Remark { get; set; }
        #endregion

        #endregion

        #region User Setting (Only administrator can configure)
        //[ForeignKey("tbl_Group")]
        //public Guid GroupID { get; set; }
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

        public virtual tbl_Company tbl_Company { get; set; }
        public virtual tbl_BusinessUnit tbl_BusinessUnit { get; set; }
        public virtual tbl_Country tbl_Country { get; set; }
        public virtual tbl_Group tbl_Group { get; set; }
        public virtual IList<tbl_UserUsedPassword> tbl_UserUsedPassword { get; set; }
        public virtual IList<tbl_ActiveUser> tbl_ActiveUser { get; set; }

    }
    public class tbl_ActiveUser {

        [Key]
        public Guid Id { get; set; }

        [ForeignKey("tbl_User")]
        public Guid UserId { get; set; }

        public string IP { get; set; }
        public string UserAgent { get; set; }
        public string JwtHMACKey { get; set; }
        public DateTime LastRequestedTime { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual tbl_User tbl_User { get; set; }

    }
    public class tbl_UserUsedPassword {
        
        [Key]        
        public Guid Id { get; set; }

        [ForeignKey("tbl_User")]
        public Guid UserId { get; set; }

        public string HashedPassword { get; set; }
        public string SaltKey { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual tbl_User tbl_User { get; set; }
    }
    #endregion 

    #region Menu program role table(s)
    public class tbl_Group {

        [Key]
        public Guid Id { get; set; }

        [ForeignKey("tbl_Company")]
        public Guid CompanyId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual tbl_Company tbl_Company { get; set; }
        public IList<tbl_GroupPermission> tbl_GroupPermission { get; set; }

    }
    public class tbl_GroupPermission {

        [Key, Column(Order = 1)]
        public Guid Id { get; set; }

        [ForeignKey("tbl_Group")]
        public Guid GroupId { get; set; }
        public tbl_Group tbl_Group { get; set; }

        [ForeignKey("tbl_Role")]
        public Guid RoleId { get; set; }
        public tbl_Role tbl_Role { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

    }
    public class tbl_Role {

        [Key]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public IList<tbl_GroupPermission> tbl_GroupPermission { get; set; }
        public IList<tbl_RolePermission> tbl_RolePermission { get; set; }

    }
    public class tbl_RolePermission {

        [Key, Column(Order = 1)]
        public Guid Id { get; set; }

        [ForeignKey("tbl_Role")]
        public Guid RoleId { get; set; }
        public tbl_Role tbl_Role { get; set; }

        [ForeignKey("tbl_ProgramMenu")]
        public Guid ProgramMenuId { get; set; }
        public tbl_ProgramMenu tbl_ProgramMenu { get; set; }

        public bool IsAllowedCreate { get; set; }
        public bool IsAllowedRetrieve { get; set; }
        public bool IsAllowedUpdate { get; set; }
        public bool IsAllowedDelete { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

    }
    public class tbl_ProgramMenu {

        [Key]
        public Guid Id { get; set; }

        [ForeignKey("tbl_Module")]
        public Guid ModuleId { get; set; }
        
        [MaxLength(255)]
        public string ProgramURI { get; set; }

        [MaxLength(255)]
        public string ProgramName { get; set; }

        [MaxLength(255)]
        public string MenuName { get; set; }

        public int LevelIndex { get; set; }

        public bool IsShow { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual tbl_Module tbl_Module { get; set; }
        public IList<tbl_RolePermission> tbl_RolePermission { get; set; }

    }
    #endregion

    #region Application level table(s)
    public class tbl_Module {

        [Key]
        public Guid Id { get; set; }

        [ForeignKey("tbl_Application")]
        public Guid ApplicationId { get; set; }

        [MaxLength(255)]
        public string ModuleName { get; set; }

        public int LevelIndex { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual tbl_Application tbl_Application { get; set; }

    }
    public class tbl_Application {

        [Key]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string ApplicationName { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

    }
    #endregion
}