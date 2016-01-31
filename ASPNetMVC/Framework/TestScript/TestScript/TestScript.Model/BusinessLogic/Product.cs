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
    public class Product_BL
    {
        string LoggerName = "TestScript_Appender_Logger";

        #region Create
        public void Create(ProductBindingModel _ProductBindingModel)
        {
            try
            {                   
                using (TestScriptEntities _TestScriptEntities = new TestScriptEntities())
                {   
                    Product _Product = new Product()
                    {   
                        ProductName = _ProductBindingModel.ProductName,
                        Description = _ProductBindingModel.Description,
                        Price = _ProductBindingModel.Price,
                        Active = true
                    };
                    _TestScriptEntities.Products.Add(_Product);
                    _TestScriptEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                throw ex;
            }
        }
        #endregion

        #region Retrieve
        public List<tbl_GridListing<ProductBindingModel>> GetProductList(Product_Criteria_Model _Product_Criteria_Model)
        {
            List<tbl_GridListing<ProductBindingModel>> List_Product = new List<tbl_GridListing<ProductBindingModel>>();
            try
            {
                using (TestScriptEntities _TestScriptEntities = new TestScriptEntities())
                {
                    tbl_GridListing<ProductBindingModel> List_tbl_GridListing = new tbl_GridListing<ProductBindingModel>();

                    var WhereAableQuery = _TestScriptEntities.Products.OrderBy(_Product_Criteria_Model.OrderByClause);

                    if(_Product_Criteria_Model.ProductID != null)
                        WhereAableQuery = WhereAableQuery.Where(x => x.ProductID.Equals(_Product_Criteria_Model.ProductID.Value));

                    if (!string.IsNullOrEmpty(_Product_Criteria_Model.ProductName))
                        WhereAableQuery = WhereAableQuery.Where(x => x.ProductName.Contains(_Product_Criteria_Model.ProductName));

                    if (!string.IsNullOrEmpty(_Product_Criteria_Model.Description))
                        WhereAableQuery = WhereAableQuery.Where(x => x.Description.Contains(_Product_Criteria_Model.Description));

                    int TotalRecordCount = WhereAableQuery.Count();

                    #region Preparing tbl_Pager_To_Client
                    List<tbl_Pager_To_Client> List_tbl_Pager_To_Client = new List<tbl_Pager_To_Client>();
                    int TotalPage = (int)Math.Ceiling((double)TotalRecordCount / (double)_Product_Criteria_Model.RecordPerPage);
                    int Pager_BatchIndex = 1;
                    int Pager_BehindTheScenseIndex = 1;
                    for (int i = 1; i <= TotalPage; i++)
                    {
                        if (Pager_BehindTheScenseIndex > _Product_Criteria_Model.PagerShowIndexOneUpToX)
                            Pager_BehindTheScenseIndex = 1;

                        List_tbl_Pager_To_Client.Add(new tbl_Pager_To_Client
                        {
                            BatchIndex = Pager_BatchIndex,
                            DisplayPageIndex = i,
                            BehindTheScenesIndex = Pager_BehindTheScenseIndex
                        });

                        Pager_BehindTheScenseIndex++;

                        if ((i % _Product_Criteria_Model.PagerShowIndexOneUpToX) == 0)
                        {
                            Pager_BatchIndex++;
                        }

                    }
                    List_tbl_GridListing.List_tbl_Pager_To_Client = List_tbl_Pager_To_Client;
                    #endregion

                    #region Preparing data table
                    List<ProductBindingModel> List_ProductBindingModel = WhereAableQuery
                                                                .AsEnumerable()
                                                                .Skip((_Product_Criteria_Model.BatchIndex - 1) * _Product_Criteria_Model.RecordPerBatch)
                                                                .Take(_Product_Criteria_Model.RecordPerBatch)
                                                                .Select((x, i) => new ProductBindingModel
                                                                {
                                                                    SrNo = i + 1 + ((_Product_Criteria_Model.BatchIndex - 1) * _Product_Criteria_Model.RecordPerBatch),
                                                                    TotalRecordCount = TotalRecordCount,
                                                                    ProductID = x.ProductID,
                                                                    ProductName = x.ProductName,
                                                                    Description = x.Description,
                                                                    Price = x.Price.Value
                                                                })
                                                                .ToList();

                    List_tbl_GridListing.List_T = List_ProductBindingModel;
                    #endregion

                    List_Product.Add(List_tbl_GridListing);

                }
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                throw ex;
            }
            return List_Product;
        }
        public List<tbl_GridListing<ProductBindingModel>> GetProductList_WithoutPager(Product_Criteria_Model _Product_Criteria_Model)
        {
            List<tbl_GridListing<ProductBindingModel>> List_Product = new List<tbl_GridListing<ProductBindingModel>>();
            try
            {
                using (TestScriptEntities _TestScriptEntities = new TestScriptEntities())
                {
                    tbl_GridListing<ProductBindingModel> List_tbl_GridListing = new tbl_GridListing<ProductBindingModel>();

                    var WhereAableQuery = _TestScriptEntities.Products.OrderBy(_Product_Criteria_Model.OrderByClause);

                    if (_Product_Criteria_Model.ProductID != null)
                        WhereAableQuery = WhereAableQuery.Where(x => x.ProductID.Equals(_Product_Criteria_Model.ProductID.Value));

                    if (!string.IsNullOrEmpty(_Product_Criteria_Model.ProductName))
                        WhereAableQuery = WhereAableQuery.Where(x => x.ProductName.Contains(_Product_Criteria_Model.ProductName));

                    if (!string.IsNullOrEmpty(_Product_Criteria_Model.Description))
                        WhereAableQuery = WhereAableQuery.Where(x => x.Description.Contains(_Product_Criteria_Model.Description));

                    int TotalRecordCount = WhereAableQuery.Count();

                    #region Preparing data table
                    List<ProductBindingModel> List_ProductBindingModel = WhereAableQuery
                                                                .AsEnumerable()                                                                
                                                                .Select((x, i) => new ProductBindingModel
                                                                {   
                                                                    ProductID = x.ProductID,
                                                                    ProductName = x.ProductName,
                                                                    Description = x.Description,
                                                                    Price = x.Price.Value
                                                                })
                                                                .ToList();

                    List_tbl_GridListing.List_T = List_ProductBindingModel;
                    #endregion

                    List_Product.Add(List_tbl_GridListing);

                }
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                throw ex;
            }
            return List_Product;
        }
        #endregion

        #region Update
        public void Update(ProductBindingModel _ProductBindingModel)
        {
            try
            {
                using (TestScriptEntities _TestScriptEntities = new TestScriptEntities())
                {   
                    Product db_Product = _TestScriptEntities.Products.Where(x => x.ProductID.Equals(_ProductBindingModel.ProductID.Value)).FirstOrDefault();
                    db_Product.ProductName = _ProductBindingModel.ProductName;
                    db_Product.Description = _ProductBindingModel.Description;
                    db_Product.Price = _ProductBindingModel.Price;
                    db_Product.Active = true;
                    _TestScriptEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                throw ex;
            }
        }
        #endregion

        #region Delete
        public void Delete(List<Product_Criteria_Model> List_Product_Criteria_Model)
        {
            try
            {
                using (TestScriptEntities _TestScriptEntities = new TestScriptEntities())
                {
                    foreach (Product_Criteria_Model _Product_Criteria_Model in List_Product_Criteria_Model)
                    {
                        var WhereAableQuery = _TestScriptEntities.Products.Select(x => x);

                        if(_Product_Criteria_Model.ProductID != null)
                            WhereAableQuery = WhereAableQuery.Where(x => x.ProductID.Equals(_Product_Criteria_Model.ProductID.Value));

                        if (!string.IsNullOrEmpty(_Product_Criteria_Model.ProductName))
                            WhereAableQuery = WhereAableQuery.Where(x => x.ProductName.Contains(_Product_Criteria_Model.ProductName));

                        if (!string.IsNullOrEmpty(_Product_Criteria_Model.Description))
                            WhereAableQuery = WhereAableQuery.Where(x => x.Description.Contains(_Product_Criteria_Model.Description));

                        _TestScriptEntities.Products.RemoveRange(WhereAableQuery);
                    }
                    _TestScriptEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                throw ex;
            }
        }
        #endregion

    }
}
