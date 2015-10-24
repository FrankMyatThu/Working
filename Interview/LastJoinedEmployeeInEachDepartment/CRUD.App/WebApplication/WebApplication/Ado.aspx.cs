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
        string ConnetionString = "Data Source=MT-PC;Initial Catalog=InterviewDB;User ID=sa;Password=sa";

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void butSubmit_Click(object sender, EventArgs e)
        {
            int EmployeeID = Convert.ToInt32(txtEmployeeID.Text.Trim());
            string EmployeeName = txtEmployeeName.Text.Trim();
            int DepartmentID = Convert.ToInt32(txtDepartmentID.Text.Trim());
            string DepartmentName = txtDepartmentName.Text.Trim();
            DateTime JoinDate = Convert.ToDateTime(txtJoinDate.Value.Trim());

            string SqlCommandText = " INSERT INTO tbl_EmpDep (EmployeeID,EmployeeName,DepartmentID,DepartmentName,JoinDate) " +
                                        " VALUES (@EmployeeID, @EmployeeName, @DepartmentID, @DepartmentName, @JoinDate) ";

            using (SqlConnection _SqlConnection = new SqlConnection(ConnetionString))
            {
                using (SqlCommand _SqlCommand = new SqlCommand(SqlCommandText, _SqlConnection))
                {
                    _SqlCommand.CommandType = CommandType.Text;
                    _SqlCommand.CommandText = SqlCommandText;
                    _SqlCommand.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = EmployeeID;
                    _SqlCommand.Parameters.Add("@EmployeeName", SqlDbType.VarChar, 100).Value = EmployeeName;
                    _SqlCommand.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = DepartmentID;
                    _SqlCommand.Parameters.Add("@DepartmentName", SqlDbType.VarChar, 100).Value = DepartmentName;
                    _SqlCommand.Parameters.Add("@JoinDate", SqlDbType.DateTime).Value = JoinDate;

                    _SqlConnection.Open();
                    _SqlCommand.ExecuteNonQuery();
                }
            }

            BindGrid();
        }

        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            int EmployeeID = Convert.ToInt32(e.CommandArgument.ToString());
            BindData(GetData(EmployeeID));
        }

        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            int EmployeeID = Convert.ToInt32(e.CommandArgument.ToString());
        }
        #endregion

        #region Function
        public void BindData(DataTable _DataTable)
        {
            foreach (DataRow _DataRow in _DataTable.Rows)
            {
                txtEmployeeID.Text = _DataRow["EmployeeID"].ToString();
                txtEmployeeName.Text = _DataRow["EmployeeName"].ToString();
                txtDepartmentID.Text = _DataRow["DepartmentID"].ToString();
                txtDepartmentName.Text = _DataRow["DepartmentName"].ToString();                
                txtJoinDate.Value = Convert.ToDateTime( _DataRow["JoinDate"]).ToString("yyyy-MM-dd");
            }
        }

        public void BindGrid() 
        {
            grdEmployeeList.DataSource = GetData();
            grdEmployeeList.DataBind();
        }

        public DataTable GetData(int EmployeeID = 0)
        {   
            using (DataSet _DataSet = new DataSet())
            {
                string SqlCommand = "SELECT * " +
                                            " FROM (SELECT *, ROW_NUMBER() OVER(PARTITION BY DepartmentID ORDER BY JoinDate DESC) AS RowNumber " +
                                            "      FROM tbl_EmpDep) AS _tbl_EmpDep " +
                                            " WHERE RowNumber = 1 ";
                if(!EmployeeID.Equals(0))
                {
                    SqlCommand +=  " AND EmployeeID = "+EmployeeID;
                }                               

                using (SqlConnection _SqlConnection = new SqlConnection(ConnetionString))
                {  
                    using (SqlCommand _SqlCommand = new SqlCommand(SqlCommand, _SqlConnection))
                    {
                        using (SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter())
                        {   
                            _SqlDataAdapter.SelectCommand = _SqlCommand;
                            _SqlConnection.Open();
                            _SqlDataAdapter.Fill(_DataSet);
                            return _DataSet.Tables[0];
                        }
                    }
                }
            }
        }

        public void Insert()
        { 
        
        }

        public void Update()
        {

        }

        public void Delete()
        {

        }
        #endregion
    
    }
}