using SG50.Base.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SG50.TokenService.ViewModels
{
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
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_UserName, ErrorMessage = "Invalid {0}")]
        public string UserName { get; set; }

        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_AlphaNumeric, ErrorMessage = "Invalid {0}")]
        public string RoleName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Password, ErrorMessage = "Invalid {0}")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "{0} is required.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// Ref http://stackoverflow.com/a/1406097/900284
        [Display(Name = "Join Date")]
        [Required(ErrorMessage = "{0} is required.")]        
        [Range(typeof(DateTime), "01/01/1900", "01/01/2100", ErrorMessage = "{0} must be between {1} and {2}")]
        public DateTime JoinDate { get; set; }
    }
    
    public class LoginUserBindingModel
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_UserName, ErrorMessage = "Invalid {0}")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Password, ErrorMessage = "Invalid {0}")]
        public string Password { get; set; }
    }
}