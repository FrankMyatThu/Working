using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public partial class Ado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            grdEmployeeList.DataSource = GetData();
        }

        public DataTable GetData()
        {
            DataTable _DataTable = new DataTable();
            using (DataSet _DataSet = new DataSet())
            {
                string ConnetionString = "Data Source=MT-PC;Initial Catalog=InterviewDB;User ID=sa;Password=sa";
                string SqlCommand = "SELECT * " +
                                            " FROM (SELECT *, ROW_NUMBER() OVER(PARTITION BY DepartmentID ORDER BY JoinDate DESC) AS RowNumber " +
                                            "      FROM tbl_EmpDep) AS _tbl_EmpDep " +
                                            " WHERE RowNumber = 1 ";

                using (SqlConnection _SqlConnection = new SqlConnection(ConnetionString))
                {
                    _SqlConnection.Open();
                    using (SqlCommand _SqlCommand = new SqlCommand(SqlCommand, _SqlConnection))
                    {
                        using (SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter())
                        {
                            _SqlDataAdapter.SelectCommand = _SqlCommand;
                            _SqlDataAdapter.Fill(_DataSet);
                            _DataTable = _DataSet.Tables[0];
                        }
                    }
                }
            }
            return _DataTable;
        }
    }
}