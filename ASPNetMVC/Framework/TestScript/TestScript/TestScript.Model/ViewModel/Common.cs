using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestScript.Base.Util;

namespace TestScript.Model.ViewModel
{
    #region Common view model(s)
    public class tbl_Pager_To_Client
    {
        public int BatchIndex { get; set; }
        public int DisplayPageIndex { get; set; }
        public int BehindTheScenesIndex { get; set; }
    }
    public class tbl_Pager_To_Service
    {
        public int BatchIndex { get; set; }
        public int PagerShowIndexOneUpToX { get; set; }
        public int RecordPerPage { get; set; }
        public int RecordPerBatch { get; set; }

        [Display(Name = "OrderByClause")]
        [RegularExpression(FormatStandardizer.Server_OrderByClause, ErrorMessage = "Invalid {0}")]
        public string OrderByClause { get; set; }
    }
    public class tbl_GridListing<T>
    {
        public List<tbl_Pager_To_Client> List_tbl_Pager_To_Client { get; set; }
        public List<T> List_T { get; set; }
    }
    #endregion

    #region Web API level helper
    public class IsDateCorrectAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                String dts = value as string;
                if (string.IsNullOrEmpty(dts)) return true;
                DateTime.ParseExact(dts, "dd/MM/YYYY", CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
    #endregion
}
