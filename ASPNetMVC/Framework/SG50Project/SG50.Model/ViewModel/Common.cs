using SG50.Base.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG50.Model.ViewModel
{
    #region Common view model(s)
    public class tbl_Pager
    {
        public int BatchIndex { get; set; }
        public int DisplayPageIndex { get; set; }
        public int BehindTheScenesIndex { get; set; }
    }
    public class tbl_GridListing<T>
    {
        public List<tbl_Pager> List_tbl_Pager { get; set; }
        public List<T> List_T { get; set; }
    }
    #endregion
}
