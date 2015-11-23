﻿using SG50.Base.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG50.Model.ViewModel
{
    #region User related view model(s)
    public class CreateUserBindingModel
    {

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Name_SingleWord, ErrorMessage = "Invalid {0}")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Name_SingleWord, ErrorMessage = "Invalid {0}")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Password, ErrorMessage = "Invalid {0}")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "{0} is required.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
    public class LoginUserBindingModel
    {

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Password, ErrorMessage = "Invalid {0}")]
        public string Password { get; set; }

    }

    /// <summary>
    ///  To search data
    /// </summary>
    public class Country_Criteria_Model : tbl_Pager_To_Service
    {
        public int? SrNo { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
    /// <summary>
    /// To bind data
    /// </summary>
    public class CountryBindingModel
    {
        public int? SrNo { get; set; }
        public int? TotalRecordCount { get; set; }

        public string Id { get; set; }

        [Display(Name = "Country name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Name_MultiWord, ErrorMessage = "Invalid {0}")]
        public string Name { get; set; }

        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
    
    }    
    #endregion
}
