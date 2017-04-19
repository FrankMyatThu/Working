using ngNetCore.Base.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngNetCore.Model.ViewModel
{
    public class CreateUser_Binding_VM
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Name_MultiWord, ErrorMessage = "Invalid {0}")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Password, ErrorMessage = "Invalid {0}")]
        public string Password { get; set; }
    }
}
