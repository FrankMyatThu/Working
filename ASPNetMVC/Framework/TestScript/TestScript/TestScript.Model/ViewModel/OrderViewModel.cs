using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestScript.Base.Util;

namespace TestScript.Model.ViewModel
{
    #region Order Listing
    /// <summary>
    ///  To search data
    /// </summary>
    public class Order_Criteria_Model : tbl_Pager_To_Service
    {
        [Display(Name = "Srno")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public int? SrNo { get; set; }

        [Display(Name = "Order Id")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public int? OrderId { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Name_MultiWord, ErrorMessage = "Invalid {0}")]
        public string Description { get; set; }

        [Display(Name = "Order Date")]
        [IsDateCorrect(ErrorMessage = "Invalid  {0}")]
        public DateTime OrderDate { get; set; }

        public List<OrderDetailBindingModel> List_OrderDetailBindingModel { get; set; }
    }
    /// <summary>
    /// To bind data
    /// </summary>
    public class OrderBindingModel
    {
        [Display(Name = "Srno")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public int? SrNo { get; set; }

        [Display(Name = "TotalRecordCount")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public int? TotalRecordCount { get; set; }

        [Display(Name = "Order Id")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public int? OrderId { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [RegularExpression(FormatStandardizer.Server_Name_MultiWord, ErrorMessage = "Invalid {0}")]
        public string Description { get; set; }

        [Display(Name = "Order Date")]
        [IsDateCorrect(ErrorMessage = "Invalid  {0}")]
        public DateTime OrderDate { get; set; }

        public List<OrderDetailBindingModel> List_OrderDetailBindingModel { get; set; }
    }
    #endregion

    #region Order Detail
    /// <summary>
    /// To bind data
    /// </summary>
    public class OrderDetailBindingModel
    {
        [Display(Name = "Order Detail Id")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public int? OrderDetailID { get; set; }

        [Display(Name = "Order Id")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public int? OrderId { get; set; }

        [Display(Name = "Product Id")]
        [Required(ErrorMessage = "{0} is required.")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public int ProductID { get; set; }

        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "{0} is required.")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public int Quantity { get; set; }

        [Display(Name = "Total")]
        [Required(ErrorMessage = "{0} is required.")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public decimal Total { get; set; }

        [Display(Name = "TotalGST")]
        [Required(ErrorMessage = "{0} is required.")]
        [RegularExpression(FormatStandardizer.Server_Numeric, ErrorMessage = "Invalid {0}")]
        public decimal TotalGST { get; set; }
    }
    #endregion
}
