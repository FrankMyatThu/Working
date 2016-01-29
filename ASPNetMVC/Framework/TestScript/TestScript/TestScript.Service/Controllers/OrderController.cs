﻿using System;
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
    [RoutePrefix("api/order")]
    public class OrderController : BasedController
    {
        #region Create
        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create(OrderBindingModel _OrderBindingModel)
        {
            ApplicationLogger.WriteTrace("Start OrderController Create", LoggerName);
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
                (new Order_BL()).Create(_OrderBindingModel);
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End OrderController Create", LoggerName);
            return Ok();
        }
        #endregion

        #region Retrieve
        [HttpPost]
        [Route("GetOrder")]
        public IHttpActionResult GetOrder(Order_Criteria_Model _Order_Criteria_Model)
        {
            ApplicationLogger.WriteTrace("Start OrderController GetOrder", LoggerName);
            List<tbl_GridListing<OrderBindingModel>> List_Product = new List<tbl_GridListing<OrderBindingModel>>();
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
                List_Product = (new Order_BL()).GetOrderList(_Order_Criteria_Model);
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End OrderController GetOrder", LoggerName);
            return Ok(List_Product);
        }
        [HttpPost]
        [Route("GetOrderDetail")]
        public IHttpActionResult GetOrderDetail(Order_Criteria_Model _Order_Criteria_Model)
        {
            ApplicationLogger.WriteTrace("Start OrderController GetOrderDetail", LoggerName);
            List<tbl_GridListing<OrderBindingModel>> List_Product = new List<tbl_GridListing<OrderBindingModel>>();
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
                List_Product = (new Order_BL()).GetOrderList_OrderDetailList(_Order_Criteria_Model);
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End OrderController GetOrderDetail", LoggerName);
            return Ok(List_Product);
        }
        #endregion

        #region Update
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(OrderBindingModel _OrderBindingModel)
        {
            ApplicationLogger.WriteTrace("Start OrderController Update", LoggerName);
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
                (new Order_BL()).Update(_OrderBindingModel);
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End OrderController Update", LoggerName);
            return Ok();
        }
        #endregion

        #region Delete
        [HttpPost]
        [Route("Delete")]
        public IHttpActionResult Delete(List<Order_Criteria_Model> List_Order_Criteria_Model)
        {
            ApplicationLogger.WriteTrace("Start OrderController Delete", LoggerName);
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
                (new Order_BL()).Delete(List_Order_Criteria_Model);
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End OrderController Delete", LoggerName);
            return Ok();
        }
        #endregion
    }
}