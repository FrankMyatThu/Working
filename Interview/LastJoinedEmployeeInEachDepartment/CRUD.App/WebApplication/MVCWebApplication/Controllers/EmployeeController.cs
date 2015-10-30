using MVCWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWebApplication.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult Index()
        {
            List<tbl_EmpDep> List_tbl_EmpDep = new List<tbl_EmpDep>(); 
            using (InterviewDBEntities _InterviewDBEntities = new InterviewDBEntities())
            {
                List_tbl_EmpDep = _InterviewDBEntities.tbl_EmpDep.Select(x => x).ToList<tbl_EmpDep>();
            }

            return View(List_tbl_EmpDep);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}