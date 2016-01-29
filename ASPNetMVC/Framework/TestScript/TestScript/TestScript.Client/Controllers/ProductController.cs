using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestScript.Client.Controllers
{
    public class ProductController : Controller
    {
        #region Create
        public ActionResult Create()
        {
            return View();
        }
        #endregion

        #region Retrieve
        public ActionResult List()
        {
            return View();
        }
        #endregion

        #region Update
        public ActionResult Edit()
        {
            return View();
        }
        #endregion

        #region Delete/Detail
        public ActionResult Detail()
        {
            return View();
        }
        #endregion
    }
}