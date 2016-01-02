using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class EmployeeController : Controller
    {   
        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(tbl_EmpDep _tbl_EmpDep, HttpPostedFileBase txtFile)
        {
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                if (txtFile != null)
                {
                    txtFile.SaveAs(Server.MapPath("~/Image/") + txtFile.FileName);
                    _tbl_EmpDep.EmployeePhoto = txtFile.FileName;
                }

                _InterviewDBEntities.tbl_EmpDep.Add(_tbl_EmpDep);
                _InterviewDBEntities.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Retrieve
        [HttpGet]
        public ActionResult Index()
        {
            List<tbl_EmpDep> List_tbl_EmpDep = null; 
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                List_tbl_EmpDep = (from _tbl_EmpDep in _InterviewDBEntities.tbl_EmpDep
                                   group _tbl_EmpDep by _tbl_EmpDep.DepartmentID into group_tbl_EmpDep
                                   select group_tbl_EmpDep.OrderByDescending(x => x.JoinDate).FirstOrDefault()).ToList<tbl_EmpDep>();
            }
            return View(List_tbl_EmpDep);
        }
        #endregion

        #region Update
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            tbl_EmpDep _tbl_EmpDep = null;
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                _tbl_EmpDep = _InterviewDBEntities.tbl_EmpDep.Where(x => x.EmployeeID.Equals(Id)).FirstOrDefault();
            }
            return View(_tbl_EmpDep);
        }
        [HttpPost]
        public ActionResult Edit(tbl_EmpDep _tbl_EmpDep, HttpPostedFileBase txtFile)
        {
            using(InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                 tbl_EmpDep db_tbl_EmpDep = _InterviewDBEntities.tbl_EmpDep.Where(x => x.EmployeeID.Equals(_tbl_EmpDep.EmployeeID)).FirstOrDefault();
                db_tbl_EmpDep.EmployeeID = _tbl_EmpDep.EmployeeID;
                db_tbl_EmpDep.EmployeeName = _tbl_EmpDep.EmployeeName;
                db_tbl_EmpDep.DepartmentID = _tbl_EmpDep.DepartmentID;
                db_tbl_EmpDep.DepartmentName  = _tbl_EmpDep.DepartmentName;
                
                if(txtFile != null)
                {
                    txtFile.SaveAs(Server.MapPath("~/Image/")+ txtFile.FileName);
                    db_tbl_EmpDep.EmployeePhoto = txtFile.FileName;
                }
                _InterviewDBEntities.SaveChanges();                
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                _InterviewDBEntities.tbl_EmpDep.RemoveRange(
                        _InterviewDBEntities.tbl_EmpDep.Where(x => x.EmployeeID.Equals(Id))                        
                    );
                _InterviewDBEntities.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Detail
        [HttpGet]
        public ActionResult Detail(int Id)
        {
            tbl_EmpDep _tbl_EmpDep = null;
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                _tbl_EmpDep = _InterviewDBEntities.tbl_EmpDep.Where(x => x.EmployeeID.Equals(Id)).FirstOrDefault();
            }
            return View(_tbl_EmpDep);
        }
        #endregion
    }
}