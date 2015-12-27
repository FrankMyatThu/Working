using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public ActionResult Create(tbl_EmpDep _tbl_EmpDep)
        {
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
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
            List<tbl_EmpDep> List_tbl_EmpDep;
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities()) {
                List_tbl_EmpDep = (from _tblEmpDep in _InterviewDBEntities.tbl_EmpDep
                                                    group _tblEmpDep by _tblEmpDep.DepartmentID into group_tblEmpDep
                                                    select group_tblEmpDep.OrderByDescending(x => x.JoinDate).FirstOrDefault()).ToList<tbl_EmpDep>();

            }
            return View(List_tbl_EmpDep);
        }
        #endregion

        #region Update
        [HttpGet]
        public ActionResult Edit(int id)
        {
            tbl_EmpDep _tbl_EmpDep;
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities()) {
                _tbl_EmpDep = _InterviewDBEntities.tbl_EmpDep.Where(x => x.EmployeeID.Equals(id)).FirstOrDefault();
            }
            return View(_tbl_EmpDep);
        }

        [HttpPost]
        public ActionResult Edit(tbl_EmpDep _tbl_EmpDep)
        {
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                _InterviewDBEntities.Entry(_tbl_EmpDep).State = EntityState.Modified;
                _InterviewDBEntities.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete        
        [HttpGet]
        public ActionResult Delete(int id)
        {   
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                _InterviewDBEntities.tbl_EmpDep.RemoveRange(
                        _InterviewDBEntities.tbl_EmpDep.Where(x => x.EmployeeID.Equals(id))
                    );
                _InterviewDBEntities.SaveChanges();
                
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Detail
        [HttpGet]
        public ActionResult Detail(int id)
        {
            tbl_EmpDep _tbl_EmpDep;
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                _tbl_EmpDep = _InterviewDBEntities.tbl_EmpDep.Where(x => x.EmployeeID.Equals(id)).FirstOrDefault();

            }
            return View(_tbl_EmpDep);
        }
        #endregion 
    }
}