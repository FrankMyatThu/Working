using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using TestScript.Base.Logging;
using TestScript.Model.Entity;
using TestScript.Model.ViewModel;

namespace TestScript.Model.BusinessLogic
{
    public class Order_BL
    {
        string LoggerName = "TestScript_Appender_Logger";

        #region Create
        public void Create(OrderBindingModel _OrderBindingModel)
        {   
            using (TestScriptEntities _TestScriptEntities = new TestScriptEntities())
            {
                using (var transaction = _TestScriptEntities.Database.BeginTransaction())
                {
                    try
                    {
                        Order _Order = new Order()
                        {
                            Description = _OrderBindingModel.Description,
                            OrderDate = _OrderBindingModel.OrderDate
                        };
                        _TestScriptEntities.Orders.Add(_Order);
                        _TestScriptEntities.SaveChanges();

                        if (_OrderBindingModel.List_OrderDetailBindingModel.Count <= 0)
                            return;

                        foreach (OrderDetailBindingModel _OrderDetailBindingModel in _OrderBindingModel.List_OrderDetailBindingModel)
                        {
                            OrderDetail _OrderDetail = new OrderDetail()
                            {
                                OrderId = _Order.OrderId,
                                ProductID = _OrderDetailBindingModel.ProductID,
                                Quantity = _OrderDetailBindingModel.Quantity,
                                Total = _OrderDetailBindingModel.Total,
                                TotalGST = _OrderDetailBindingModel.TotalGST
                            };
                            _TestScriptEntities.OrderDetails.Add(_OrderDetail);
                            _TestScriptEntities.SaveChanges();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        BaseExceptionLogger.LogError(ex, LoggerName);
                        throw ex;
                    }
                }
            }            
        }
        #endregion

        #region Retrieve
        public List<tbl_GridListing<OrderBindingModel>> GetOrderList(Order_Criteria_Model _Order_Criteria_Model)
        {
            List<tbl_GridListing<OrderBindingModel>> List_Order = new List<tbl_GridListing<OrderBindingModel>>();
            try
            {
                using (TestScriptEntities _TestScriptEntities = new TestScriptEntities())
                {
                    tbl_GridListing<OrderBindingModel> List_tbl_GridListing = new tbl_GridListing<OrderBindingModel>();

                    var WhereAableQuery = _TestScriptEntities.Orders.OrderBy(_Order_Criteria_Model.OrderByClause);

                    if(_Order_Criteria_Model.OrderId !=null)
                        WhereAableQuery = WhereAableQuery.Where(x => x.OrderId.Equals(_Order_Criteria_Model.OrderId.Value));

                    if (!string.IsNullOrEmpty(_Order_Criteria_Model.Description))
                        WhereAableQuery = WhereAableQuery.Where(x => x.Description.Contains(_Order_Criteria_Model.Description));

                    if (_Order_Criteria_Model.OrderDate != null)
                        WhereAableQuery = WhereAableQuery.Where(x => x.OrderDate.Equals(_Order_Criteria_Model.OrderDate));

                    int TotalRecordCount = WhereAableQuery.Count();

                    #region Preparing tbl_Pager_To_Client
                    List<tbl_Pager_To_Client> List_tbl_Pager_To_Client = new List<tbl_Pager_To_Client>();
                    int TotalPage = (int)Math.Ceiling((double)TotalRecordCount / (double)_Order_Criteria_Model.RecordPerPage);
                    int Pager_BatchIndex = 1;
                    int Pager_BehindTheScenseIndex = 1;
                    for (int i = 1; i <= TotalPage; i++)
                    {
                        if (Pager_BehindTheScenseIndex > _Order_Criteria_Model.PagerShowIndexOneUpToX)
                            Pager_BehindTheScenseIndex = 1;

                        List_tbl_Pager_To_Client.Add(new tbl_Pager_To_Client
                        {
                            BatchIndex = Pager_BatchIndex,
                            DisplayPageIndex = i,
                            BehindTheScenesIndex = Pager_BehindTheScenseIndex
                        });

                        Pager_BehindTheScenseIndex++;

                        if ((i % _Order_Criteria_Model.PagerShowIndexOneUpToX) == 0)
                        {
                            Pager_BatchIndex++;
                        }

                    }
                    List_tbl_GridListing.List_tbl_Pager_To_Client = List_tbl_Pager_To_Client;
                    #endregion

                    #region Preparing data table
                    List<OrderBindingModel> List_OrderBindingModel = WhereAableQuery
                                                                .AsEnumerable()
                                                                .Skip((_Order_Criteria_Model.BatchIndex - 1) * _Order_Criteria_Model.RecordPerBatch)
                                                                .Take(_Order_Criteria_Model.RecordPerBatch)
                                                                .Select((x, i) => new OrderBindingModel
                                                                {
                                                                    SrNo = i + 1 + ((_Order_Criteria_Model.BatchIndex - 1) * _Order_Criteria_Model.RecordPerBatch),
                                                                    TotalRecordCount = TotalRecordCount,
                                                                    OrderId = x.OrderId,
                                                                    Description = x.Description,
                                                                    OrderDate = new DateTime(x.OrderDate.Value.Year, x.OrderDate.Value.Month, x.OrderDate.Value.Day)
                                                                })
                                                                .ToList();

                    List_tbl_GridListing.List_T = List_OrderBindingModel;
                    #endregion

                    List_Order.Add(List_tbl_GridListing);

                }
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                throw ex;
            }
            return List_Order;
        }

        public List<tbl_GridListing<OrderBindingModel>> GetOrderList_OrderDetailList(Order_Criteria_Model _Order_Criteria_Model)
        {
            List<tbl_GridListing<OrderBindingModel>> List_Order = new List<tbl_GridListing<OrderBindingModel>>();
            try
            {
                using (TestScriptEntities _TestScriptEntities = new TestScriptEntities())
                {
                    tbl_GridListing<OrderBindingModel> List_tbl_GridListing = new tbl_GridListing<OrderBindingModel>();

                    var WhereAableQuery = _TestScriptEntities.Orders.OrderBy(_Order_Criteria_Model.OrderByClause);

                    if (_Order_Criteria_Model.OrderId != null)
                        WhereAableQuery = WhereAableQuery.Where(x => x.OrderId.Equals(_Order_Criteria_Model.OrderId.Value));

                    if (!string.IsNullOrEmpty(_Order_Criteria_Model.Description))
                        WhereAableQuery = WhereAableQuery.Where(x => x.Description.Contains(_Order_Criteria_Model.Description));

                    if (_Order_Criteria_Model.OrderDate != null)
                        WhereAableQuery = WhereAableQuery.Where(x => x.OrderDate.Equals(_Order_Criteria_Model.OrderDate));

                    int TotalRecordCount = WhereAableQuery.Count();

                    #region Preparing tbl_Pager_To_Client
                    List<tbl_Pager_To_Client> List_tbl_Pager_To_Client = new List<tbl_Pager_To_Client>();
                    int TotalPage = (int)Math.Ceiling((double)TotalRecordCount / (double)_Order_Criteria_Model.RecordPerPage);
                    int Pager_BatchIndex = 1;
                    int Pager_BehindTheScenseIndex = 1;
                    for (int i = 1; i <= TotalPage; i++)
                    {
                        if (Pager_BehindTheScenseIndex > _Order_Criteria_Model.PagerShowIndexOneUpToX)
                            Pager_BehindTheScenseIndex = 1;

                        List_tbl_Pager_To_Client.Add(new tbl_Pager_To_Client
                        {
                            BatchIndex = Pager_BatchIndex,
                            DisplayPageIndex = i,
                            BehindTheScenesIndex = Pager_BehindTheScenseIndex
                        });

                        Pager_BehindTheScenseIndex++;

                        if ((i % _Order_Criteria_Model.PagerShowIndexOneUpToX) == 0)
                        {
                            Pager_BatchIndex++;
                        }

                    }
                    List_tbl_GridListing.List_tbl_Pager_To_Client = List_tbl_Pager_To_Client;
                    #endregion

                    #region Preparing data table (for loop approach)
                    ///// Order master binding
                    //List<OrderBindingModel> List_OrderBindingModel = WhereAableQuery
                    //                                            .AsEnumerable()
                    //                                            .Skip((_Order_Criteria_Model.BatchIndex - 1) * _Order_Criteria_Model.RecordPerBatch)
                    //                                            .Take(_Order_Criteria_Model.RecordPerBatch)
                    //                                            .Select((x, i) => new OrderBindingModel
                    //                                            {
                    //                                                SrNo = i + 1 + ((_Order_Criteria_Model.BatchIndex - 1) * _Order_Criteria_Model.RecordPerBatch),
                    //                                                TotalRecordCount = TotalRecordCount,
                    //                                                OrderId = x.OrderId,
                    //                                                Description = x.Description,
                    //                                                OrderDate = new DateTime(x.OrderDate.Value.Year, x.OrderDate.Value.Month, x.OrderDate.Value.Day)                                                                    
                    //                                            })
                    //                                            .ToList();

                    ///// Order detail binding
                    //for (int i = 0; i <= List_OrderBindingModel.Count; i++)
                    //{
                    //    List<OrderDetail> db_List_OrderDetail = _TestScriptEntities.OrderDetails.Where(x => x.OrderId.Equals(List_OrderBindingModel[i].OrderId.Value)).ToList();

                    //    for(int j=0; j <= db_List_OrderDetail.Count; j++ )
                    //    {
                    //        OrderDetailBindingModel _OrderDetailBindingModel = new OrderDetailBindingModel();
                    //        _OrderDetailBindingModel.OrderId = db_List_OrderDetail[j].OrderId.Value;
                    //        _OrderDetailBindingModel.ProductID = db_List_OrderDetail[j].ProductID.Value;
                    //        _OrderDetailBindingModel.Quantity = db_List_OrderDetail[j].Quantity.Value;
                    //        _OrderDetailBindingModel.Total = db_List_OrderDetail[j].Total.Value;
                    //        _OrderDetailBindingModel.TotalGST = db_List_OrderDetail[j].TotalGST.Value;
                    //        List_OrderBindingModel[i].List_OrderDetailBindingModel.Add(_OrderDetailBindingModel);    
                    //    }
                    //}
                    //List_tbl_GridListing.List_T = List_OrderBindingModel;
                    #endregion

                    #region Preparing data table (nested linq select approach)
                    /// Order master binding
                    List<OrderBindingModel> List_OrderBindingModel = WhereAableQuery
                                                                .AsEnumerable()
                                                                .Skip((_Order_Criteria_Model.BatchIndex - 1) * _Order_Criteria_Model.RecordPerBatch)
                                                                .Take(_Order_Criteria_Model.RecordPerBatch)
                                                                .Select((x, i) => new OrderBindingModel
                                                                {
                                                                    SrNo = i + 1 + ((_Order_Criteria_Model.BatchIndex - 1) * _Order_Criteria_Model.RecordPerBatch),
                                                                    TotalRecordCount = TotalRecordCount,
                                                                    OrderId = x.OrderId,
                                                                    Description = x.Description,
                                                                    OrderDate = new DateTime(x.OrderDate.Value.Year, x.OrderDate.Value.Month, x.OrderDate.Value.Day),
                                                                    List_OrderDetailBindingModel = (from _OrderDetail in x.OrderDetails
                                                                                                                            select new OrderDetailBindingModel { 
                                                                                                                                OrderDetailID = _OrderDetail.OrderDetailId,
                                                                                                                                OrderId = _OrderDetail.OrderId,
                                                                                                                                ProductID = _OrderDetail.ProductID.Value,
                                                                                                                                Quantity = _OrderDetail.Quantity.Value,
                                                                                                                                Total = _OrderDetail.Total.Value,
                                                                                                                                TotalGST = _OrderDetail.TotalGST.Value
                                                                                                                            }).ToList<OrderDetailBindingModel>()
                                                                })
                                                                .ToList();
                    List_tbl_GridListing.List_T = List_OrderBindingModel;
                    #endregion

                    List_Order.Add(List_tbl_GridListing);

                }
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                throw ex;
            }
            return List_Order;
        }
        #endregion

        #region Update
        public void Update(OrderBindingModel _OrderBindingModel)
        {
            using (TestScriptEntities _TestScriptEntities = new TestScriptEntities())
            {
                using (var transaction = _TestScriptEntities.Database.BeginTransaction())
                {
                    try
                    {
                        Order db_Order = _TestScriptEntities.Orders.Where(x => x.OrderId.Equals(_OrderBindingModel.OrderId.Value)).FirstOrDefault();
                        db_Order.Description = _OrderBindingModel.Description;
                        db_Order.OrderDate = _OrderBindingModel.OrderDate;
                        _TestScriptEntities.SaveChanges();

                        _TestScriptEntities.OrderDetails.RemoveRange(
                                _TestScriptEntities.OrderDetails.Where(x => x.OrderId.Equals(_OrderBindingModel.OrderId.Value))
                            );
                        _TestScriptEntities.SaveChanges();

                        if (_OrderBindingModel.List_OrderDetailBindingModel.Count <= 0)
                            return;

                        foreach (OrderDetailBindingModel _OrderDetailBindingModel in _OrderBindingModel.List_OrderDetailBindingModel)
                        {
                            OrderDetail _OrderDetail = new OrderDetail()
                            {
                                OrderId = db_Order.OrderId,
                                ProductID = _OrderDetailBindingModel.ProductID,
                                Quantity = _OrderDetailBindingModel.Quantity,
                                Total = _OrderDetailBindingModel.Total,
                                TotalGST = _OrderDetailBindingModel.TotalGST
                            };
                            _TestScriptEntities.OrderDetails.Add(_OrderDetail);
                            _TestScriptEntities.SaveChanges();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        BaseExceptionLogger.LogError(ex, LoggerName);
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region Delete
        public void Delete(List<Order_Criteria_Model> List_Order_Criteria_Model)
        {
            using (TestScriptEntities _TestScriptEntities = new TestScriptEntities())
            {
                using (var transaction = _TestScriptEntities.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (Order_Criteria_Model _Order_Criteria_Model in List_Order_Criteria_Model)
                        {
                            var WhereAableQuery = _TestScriptEntities.Orders.Select(x => x);

                            if(_Order_Criteria_Model.OrderId != null)
                                WhereAableQuery = WhereAableQuery.Where(x => x.OrderId.Equals(_Order_Criteria_Model.OrderId.Value));

                            if (!string.IsNullOrEmpty(_Order_Criteria_Model.Description))
                                WhereAableQuery = WhereAableQuery.Where(x => x.Description.Contains(_Order_Criteria_Model.Description));

                            if (_Order_Criteria_Model.OrderDate != null)
                                WhereAableQuery = WhereAableQuery.Where(x => x.OrderDate.Equals(_Order_Criteria_Model.OrderDate));

                            List<Order> List_Order = WhereAableQuery.Select(x => x).ToList();
                            foreach(Order _Order in List_Order)
                            {
                                _TestScriptEntities.OrderDetails.RemoveRange(_TestScriptEntities.OrderDetails.Where(x => x.OrderId.Equals(_Order.OrderId)));
                                _TestScriptEntities.SaveChanges();
                            }

                            _TestScriptEntities.Orders.RemoveRange(WhereAableQuery);
                            _TestScriptEntities.SaveChanges();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        BaseExceptionLogger.LogError(ex, LoggerName);
                        throw ex;
                    }
                }
            }
        }
        #endregion

    }
}
