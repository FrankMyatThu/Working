using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCApp;

namespace MVCApp.Controllers
{
    public class EmployeeController : Controller
    {
        private InterviewDBEntities db = new InterviewDBEntities();

        // GET: Employee
        public ActionResult Index()
        {
            List<tbl_EmpDep> List_tbl_EmpDep =  db.tbl_EmpDep.
                                                        ToList().
                                                        GroupBy(x => x.DepartmentID).
                                                        Select(y => y.OrderByDescending(z => z.JoinDate)).
                                                        SelectMany(a => a).
                                                        ToList();

            return View(List_tbl_EmpDep);
        }

        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_EmpDep tbl_EmpDep = db.tbl_EmpDep.Find(id);
            if (tbl_EmpDep == null)
            {
                return HttpNotFound();
            }
            return View(tbl_EmpDep);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,EmployeeName,DepartmentID,DepartmentName,JoinDate")] tbl_EmpDep tbl_EmpDep)
        {
            if (ModelState.IsValid)
            {
                db.tbl_EmpDep.Add(tbl_EmpDep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_EmpDep);
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_EmpDep tbl_EmpDep = db.tbl_EmpDep.Find(id);
            if (tbl_EmpDep == null)
            {
                return HttpNotFound();
            }
            return View(tbl_EmpDep);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,EmployeeName,DepartmentID,DepartmentName,JoinDate")] tbl_EmpDep tbl_EmpDep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_EmpDep).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_EmpDep);
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_EmpDep tbl_EmpDep = db.tbl_EmpDep.Find(id);
            if (tbl_EmpDep == null)
            {
                return HttpNotFound();
            }
            return View(tbl_EmpDep);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_EmpDep tbl_EmpDep = db.tbl_EmpDep.Find(id);
            db.tbl_EmpDep.Remove(tbl_EmpDep);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
