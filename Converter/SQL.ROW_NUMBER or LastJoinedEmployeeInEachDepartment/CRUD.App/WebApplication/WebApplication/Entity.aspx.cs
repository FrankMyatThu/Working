using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public partial class Entity : System.Web.UI.Page
    {
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetFormControl();
                BindGrid();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            switch (btnSubmit.Text)
            {
                case "Save":
                    Insert();
                    break;

                case "Update":
                    Update();
                    break;
            }
            ResetFormControl();
            BindGrid();
        }

        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            int EmployeeID = Convert.ToInt32(e.CommandArgument.ToString());
            BindFormControl(SelectData(EmployeeID));
            btnSubmit.Text = "Update";
        }

        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            int EmployeeID = Convert.ToInt32(e.CommandArgument.ToString());
            Delete(EmployeeID);
            ResetFormControl();
            BindGrid();
        }
        #endregion

        #region Function

        #region Binding
        public void ResetFormControl()
        {
            txtEmployeeID.Text = "";
            txtEmployeeName.Text = "";
            txtDepartmentID.Text = "";
            txtDepartmentName.Text = "";
            txtJoinDate.Value = "";
            btnSubmit.Text = "Save";
        }
        public void BindFormControl(List<tbl_EmpDep> List_tbl_EmpDep)
        {
            foreach (tbl_EmpDep _tbl_EmpDep in List_tbl_EmpDep)
            {
                txtEmployeeID.Text = _tbl_EmpDep.EmployeeID.ToString();
                txtEmployeeName.Text = _tbl_EmpDep.EmployeeName.ToString();
                txtDepartmentID.Text = _tbl_EmpDep.DepartmentID.ToString();
                txtDepartmentName.Text = _tbl_EmpDep.DepartmentName.ToString();
                txtJoinDate.Value = Convert.ToDateTime(_tbl_EmpDep.JoinDate.ToString()).ToString("yyyy-MM-dd");
            }
        }
        public void BindGrid()
        {
            grdEmployeeList.DataSource = SelectData();
            grdEmployeeList.DataBind();
        }
        #endregion

        #region CRUD
        public List<tbl_EmpDep> SelectData(int EmployeeID = 0)
        {
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities()) {

                var List_tblEmpDep_RowNo = _InterviewDBEntities.tbl_EmpDep
                                                            .ToList()
                                                            .OrderByDescending(x => x.JoinDate)
                                                            .GroupBy(x => x.DepartmentID)
                                                            .Select(group => new { Group = group, Count = group.Count() })                                                            
                                                            .SelectMany(groupWithCount =>
                                                                groupWithCount.Group.Select(b => b)
                                                                .Zip(
                                                                    Enumerable.Range(1, groupWithCount.Count),
                                                                    (j, i) => new { j.EmployeeID,
                                                                                    j.EmployeeName,
                                                                                    j.DepartmentID,
                                                                                    j.DepartmentName,
                                                                                    j.JoinDate,
                                                                                    RowNumber = i 
                                                                    }
                                                                )
                                                            );

                List<tbl_EmpDep> List_tbl_EmpDep = List_tblEmpDep_RowNo.Where(x => x.RowNumber.Equals(1))
                                                                                 .OrderBy(x => x.DepartmentID)
                                                                                 .Select(x => new tbl_EmpDep
                                                                                 {
                                                                                     EmployeeID = x.EmployeeID,
                                                                                     EmployeeName = x.EmployeeName,
                                                                                     DepartmentID = x.DepartmentID,
                                                                                     DepartmentName = x.DepartmentName,
                                                                                     JoinDate = x.JoinDate
                                                                                 }).ToList();

                return List_tbl_EmpDep;
            }
        }

        public void Insert()
        {   
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities()) {
                tbl_EmpDep _tbl_EmpDep = new tbl_EmpDep();
                _tbl_EmpDep.EmployeeID = Convert.ToInt32(txtEmployeeID.Text.Trim());
                _tbl_EmpDep.EmployeeName = txtEmployeeName.Text.Trim();
                _tbl_EmpDep.DepartmentID = Convert.ToInt32(txtDepartmentID.Text.Trim());
                _tbl_EmpDep.DepartmentName = txtDepartmentName.Text.Trim();
                _tbl_EmpDep.JoinDate = Convert.ToDateTime(txtJoinDate.Value.Trim());
                _InterviewDBEntities.tbl_EmpDep.Add(_tbl_EmpDep);
                _InterviewDBEntities.SaveChanges();
            }
        }

        public void Update()
        {
            int EmployeeID = Convert.ToInt32(txtEmployeeID.Text.Trim());            
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                tbl_EmpDep _tbl_EmpDep = _InterviewDBEntities.tbl_EmpDep.Where(x => x.EmployeeID.Equals(EmployeeID)).Select(x => x).FirstOrDefault();
                _tbl_EmpDep.EmployeeName = txtEmployeeName.Text.Trim();
                _tbl_EmpDep.DepartmentID = Convert.ToInt32(txtDepartmentID.Text.Trim());
                _tbl_EmpDep.DepartmentName = txtDepartmentName.Text.Trim();
                _tbl_EmpDep.JoinDate = Convert.ToDateTime(txtJoinDate.Value.Trim());
                _InterviewDBEntities.SaveChanges();
            }
            
        }

        public void Delete(int EmployeeID)
        {
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                tbl_EmpDep _tbl_EmpDep = _InterviewDBEntities.tbl_EmpDep.Where(x => x.EmployeeID.Equals(EmployeeID)).Select(x => x).FirstOrDefault();
                _InterviewDBEntities.tbl_EmpDep.Remove(_tbl_EmpDep);
                _InterviewDBEntities.SaveChanges();
            }
        }
        #endregion

        #endregion
    }
}