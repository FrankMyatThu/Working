using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestScript.Base.Util;

namespace TestScript.Model.ViewModel
{
    /// <summary>
    ///  To search data
    /// </summary>
    public class Product_Criteria_Model : tbl_Pager_To_Service
    {
        [Display(Name = "Srno")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public int? SrNo { get; set; }

        [Display(Name = "Product Id")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public int? ProductID { get; set; }

        [Display(Name = "Product name")]        
        [StringLength(250, MinimumLength = 0, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Name_MultiWord, ErrorMessage = "Invalid {0}")]
        public string ProductName { get; set; }

        [Display(Name = "Description")]        
        [StringLength(250, MinimumLength = 0, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Name_MultiWord, ErrorMessage = "Invalid {0}")]
        public string Description { get; set; }

        [Display(Name = "Price")]        
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public decimal? Price { get; set; }
    }
    /// <summary>
    /// To bind data
    /// </summary>
    public class ProductBindingModel
    {
        [Display(Name = "Srno")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public int? SrNo { get; set; }

        [Display(Name = "TotalRecordCount")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public int? TotalRecordCount { get; set; }

        [Display(Name = "Product Id")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public int? ProductID { get; set; }
        
        [Display(Name = "Product name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Name_MultiWord, ErrorMessage = "Invalid {0}")]
        public string ProductName { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Name_MultiWord, ErrorMessage = "Invalid {0}")]
        public string Description { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "{0} is required.")]        
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public decimal Price { get; set; }
        
    }   
}
