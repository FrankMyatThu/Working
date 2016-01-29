using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestScript.Base.Logging;
using TestScript.Model.BusinessLogic;
using TestScript.Model.ViewModel;

namespace TestScript.Service.Controllers
{
    [RoutePrefix("api/product")]
    public class ProductController : BasedController
    {
        #region Create
        [HttpPost]
        [Route("Create")]        
        public IHttpActionResult Create(ProductBindingModel _ProductBindingModel)
        {
            ApplicationLogger.WriteTrace("Start ProductController Create", LoggerName);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                ModelState.AddModelError(Key_ModelStateInvalidError, messages);
                ApplicationLogger.WriteError(messages, LoggerName);
                return BadRequest(ModelState);
            }

            try
            {
                (new Product_BL()).Create(_ProductBindingModel);
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End ProductController Create", LoggerName);
            return Ok();
        }
        #endregion

        #region Retrieve
        [HttpPost]
        [Route("GetProduct")]        
        public IHttpActionResult GetProduct(Product_Criteria_Model _Product_Criteria_Model)
        {
            ApplicationLogger.WriteTrace("Start ProductController GetProduct", LoggerName);
            List<tbl_GridListing<ProductBindingModel>> List_Product = new List<tbl_GridListing<ProductBindingModel>>();
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                ModelState.AddModelError(Key_ModelStateInvalidError, messages);
                ApplicationLogger.WriteError(messages, LoggerName);
                return BadRequest(ModelState);
            }

            try
            {
                List_Product = (new Product_BL()).GetProductList(_Product_Criteria_Model);
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End ProductController GetProduct", LoggerName);
            return Ok(List_Product);
        }
        #endregion

        #region Update
        [HttpPost]
        [Route("Update")]        
        public IHttpActionResult Update(ProductBindingModel _ProductBindingModel)
        {
            ApplicationLogger.WriteTrace("Start ProductController Update", LoggerName);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                ModelState.AddModelError(Key_ModelStateInvalidError, messages);
                ApplicationLogger.WriteError(messages, LoggerName);
                return BadRequest(ModelState);
            }

            try
            {
                (new Product_BL()).Update(_ProductBindingModel);
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End ProductController Update", LoggerName);
            return Ok();
        }
        #endregion

        #region Delete
        [HttpPost]
        [Route("Delete")]        
        public IHttpActionResult Delete(List<Product_Criteria_Model> List_Product_Criteria_Model)
        {
            ApplicationLogger.WriteTrace("Start ProductController Delete", LoggerName);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage + " " + x.Exception));
                ModelState.AddModelError(Key_ModelStateInvalidError, messages);
                ApplicationLogger.WriteError(messages, LoggerName);
                return BadRequest(ModelState);
            }

            try
            {
                (new Product_BL()).Delete(List_Product_Criteria_Model);
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End ProductController Delete", LoggerName);
            return Ok();
        }
        #endregion
    }
}
