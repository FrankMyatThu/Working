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
    }
    public class tbl_GridListing<T>
    {
        public List<tbl_Pager_To_Client> List_tbl_Pager_To_Client { get; set; }
        public List<T> List_T { get; set; }
    }
    #endregion
}
