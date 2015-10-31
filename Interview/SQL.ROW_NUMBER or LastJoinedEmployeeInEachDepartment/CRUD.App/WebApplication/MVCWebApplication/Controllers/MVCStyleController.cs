using MVCWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWebApplication.Controllers
{
    public class MVCStyleController : Controller
    {
        #region Create
        public ActionResult Create()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(tbl_EmpDep tbl_EmpDep)
        {
            if (!ModelState.IsValid)
                return View();

            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                _InterviewDBEntities.tbl_EmpDep.Add(tbl_EmpDep);
                _InterviewDBEntities.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Retrieve
        public ActionResult Index()
        {
            List<tbl_EmpDep> List_tbl_EmpDep = null;
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                var List_EmpDep_RowNo = _InterviewDBEntities.tbl_EmpDep.ToList()
                                            .OrderByDescending(x => x.JoinDate)
                                            .GroupBy(x => x.DepartmentID)
                                            .Select(x => new { Group = x, Count = x.Count() })
                                            .SelectMany(x => x.Group.Select(y => y).Zip(
                                                    Enumerable.Range(1, x.Count),
                                                    (z, i) => new { z.EmployeeID, z.EmployeeName, z.DepartmentID, z.DepartmentName, z.JoinDate, RowNumber = i }
                                                ));

                List_tbl_EmpDep = List_EmpDep_RowNo.Where(x => x.RowNumber.Equals(1))
                                                                    .OrderBy(x => x.DepartmentID)
                                                                    .Select(x => new tbl_EmpDep
                                                                    {
                                                                        EmployeeID = x.EmployeeID,
                                                                        EmployeeName = x.EmployeeName,
                                                                        DepartmentID = x.DepartmentID,
                                                                        DepartmentName = x.DepartmentName,
                                                                        JoinDate = x.JoinDate
                                                                    }).ToList();
            }
            return View(List_tbl_EmpDep);
        }
        #endregion

        #region Update
        public ActionResult Edit(int id)
        {
            tbl_EmpDep _tbl_EmpDep = new tbl_EmpDep();
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                _tbl_EmpDep = _InterviewDBEntities.tbl_EmpDep.Where(x => x.EmployeeID.Equals(id)).Select(x => x).FirstOrDefault();
            }

            return View(_tbl_EmpDep);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(tbl_EmpDep _tbl_EmpDep)
        {
            if (!ModelState.IsValid)
                return View();

            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                _InterviewDBEntities.tbl_EmpDep.Attach(_tbl_EmpDep);
                _InterviewDBEntities.Entry(_tbl_EmpDep).State = EntityState.Modified;
                _InterviewDBEntities.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete        
        public ActionResult Delete(int id)
        {
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                tbl_EmpDep _tbl_EmpDep =  _InterviewDBEntities.tbl_EmpDep.Where(x => x.EmployeeID.Equals(id)).Select(x => x).FirstOrDefault();
                _InterviewDBEntities.tbl_EmpDep.Remove(_tbl_EmpDep);
                _InterviewDBEntities.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Detial
        public ActionResult Details(int id)
        {
            tbl_EmpDep _tbl_EmpDep = null;
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
               _tbl_EmpDep = _InterviewDBEntities.tbl_EmpDep.Where(x => x.EmployeeID.Equals(id)).FirstOrDefault();
            }
            return View(_tbl_EmpDep);
        }
        #endregion
    }
}