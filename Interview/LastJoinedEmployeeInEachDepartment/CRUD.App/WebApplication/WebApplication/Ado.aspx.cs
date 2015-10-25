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
            BindData();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {   
            switch (btnSubmit.Text)
            {
                case "Save":
                    Insert();
                    BindData();
                    btnSubmit.Text = "Save";
                    break;

                case "Update":
                    Update();
                    BindData();
                    btnSubmit.Text = "Update";
                    break;
            }
        }

        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            int EmployeeID = Convert.ToInt32(e.CommandArgument.ToString());
            BindData(SelectData(EmployeeID));
        }

        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            int EmployeeID = Convert.ToInt32(e.CommandArgument.ToString());
        }
        #endregion

        #region Function
        public void BindData(DataTable _DataTable = null) 
        {
            /// Bind Grid
            grdEmployeeList.DataSource = SelectData();
            grdEmployeeList.DataBind();

            /// Bind From
            txtEmployeeID.Text = "";
            txtEmployeeName.Text = "";
            txtDepartmentID.Text = "";
            txtDepartmentName.Text = "";
            txtJoinDate.Value = "";
            if (_DataTable == null) return;
            foreach (DataRow _DataRow in _DataTable.Rows)
            {
                txtEmployeeID.Text = _DataRow["EmployeeID"].ToString();
                txtEmployeeName.Text = _DataRow["EmployeeName"].ToString();
                txtDepartmentID.Text = _DataRow["DepartmentID"].ToString();
                txtDepartmentName.Text = _DataRow["DepartmentName"].ToString();
                txtJoinDate.Value = Convert.ToDateTime(_DataRow["JoinDate"]).ToString("yyyy-MM-dd");
            }
        }

        public DataTable SelectData(int EmployeeID = 0)
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
        }

        public void Update()
        {
            int EmployeeID = Convert.ToInt32(txtEmployeeID.Text.Trim());
            string EmployeeName = txtEmployeeName.Text.Trim();
            int DepartmentID = Convert.ToInt32(txtDepartmentID.Text.Trim());
            string DepartmentName = txtDepartmentName.Text.Trim();
            DateTime JoinDate = Convert.ToDateTime(txtJoinDate.Value.Trim());

            string SqlCommandText =  " UPDATE tbl_EmpDep " +
                                        " SET  " +
                                        "     EmployeeID = @EmployeeID,  " +
                                        "     EmployeeName = @EmployeeName, " +
                                        "     DepartmentID = @DepartmentID, " +
                                        "     DepartmentName = @DepartmentName, " +
                                        "     JoinDate = @JoinDate " +
                                        " WHERE EmployeeID = @EmployeeID ";

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
        }

        public void Delete(int EmployeeID)
        {   
            string EmployeeName = txtEmployeeName.Text.Trim();
            int DepartmentID = Convert.ToInt32(txtDepartmentID.Text.Trim());
            string DepartmentName = txtDepartmentName.Text.Trim();
            DateTime JoinDate = Convert.ToDateTime(txtJoinDate.Value.Trim());

            string SqlCommandText = "DELETE FROM tbl_EmpDep WHERE EmployeeID = @EmployeeID";

            using (SqlConnection _SqlConnection = new SqlConnection(ConnetionString))
            {
                using (SqlCommand _SqlCommand = new SqlCommand(SqlCommandText, _SqlConnection))
                {
                    _SqlCommand.CommandType = CommandType.Text;
                    _SqlCommand.CommandText = SqlCommandText;
                    _SqlCommand.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = EmployeeID;
                    _SqlConnection.Open();
                    _SqlCommand.ExecuteNonQuery();
                }
            }
        }
        #endregion
    
    }
}