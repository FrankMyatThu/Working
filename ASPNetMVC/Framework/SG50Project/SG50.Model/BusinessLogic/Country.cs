﻿using SG50.Base.Logging;
using SG50.Model.Entity;
using SG50.Model.POCO;
using SG50.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG50.Model.BusinessLogic
{
    public class Country
    {
        string LoggerName = "SG50Project_Appender_Logger";

        public void Create(CountryBindingModel _CountryBindingModel, string _CurrentUserID)
        {
            try
            {
                using (ApplicationDbContext _ApplicationDbContext = new ApplicationDbContext())
                {
                    Guid UserID = _ApplicationDbContext.tbl_ActiveUser.Where(x => x.Id.Equals(new Guid(_CurrentUserID))).Select(x => x.UserId).FirstOrDefault();
                    Guid KeyId = Guid.NewGuid();
                    tbl_Country _tbl_Country = new tbl_Country()
                    {
                        Id = KeyId,
                        Name = _CountryBindingModel.Name,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedBy = UserID,
                        UpdatedDate = null,
                        UpdatedBy = null
                    };
                    _ApplicationDbContext.tbl_Country.Add(_tbl_Country);
                    _ApplicationDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                throw ex;
            }
        }

        //public List<tbl_GridListing<CountryBindingModel>> GetCountryList(int PagerShowIndexOneUpToX = 5, int RecordPerPage = 5, int BatchIndex = 2, int RecordPerBatch = 25)
        public List<tbl_GridListing<CountryBindingModel>> GetCountryList(Country_Criteria_Model _Country_Criteria_Model)
        {
            List<tbl_GridListing<CountryBindingModel>> List_tbl_Country = new List<tbl_GridListing<CountryBindingModel>>();
            try {
                using (ApplicationDbContext _ApplicationDbContext = new ApplicationDbContext())
                {
                    tbl_GridListing<CountryBindingModel> List_tbl_GridListing = new tbl_GridListing<CountryBindingModel>();
                    int TotalRecordCount = _ApplicationDbContext.tbl_Country.Count();

                    #region Preparing tbl_Pager_To_Client
                    List<tbl_Pager_To_Client> List_tbl_Pager_To_Client = new List<tbl_Pager_To_Client>();
                    int TotalPage = (int)Math.Ceiling((double)TotalRecordCount / (double)_Country_Criteria_Model.RecordPerPage);
                    int Pager_BatchIndex = 1;
                    int Pager_BehindTheScenseIndex = 1;                    
                    for (int i = 1; i <= TotalPage; i++)
                    {
                        if (Pager_BehindTheScenseIndex > _Country_Criteria_Model.PagerShowIndexOneUpToX)
                            Pager_BehindTheScenseIndex = 1;

                        List_tbl_Pager_To_Client.Add(new tbl_Pager_To_Client { 
                            BatchIndex = Pager_BatchIndex,
                            DisplayPageIndex = i,
                            BehindTheScenesIndex = Pager_BehindTheScenseIndex
                        });

                        Pager_BehindTheScenseIndex++;

                        if ((i % _Country_Criteria_Model.PagerShowIndexOneUpToX) == 0) {
                            Pager_BatchIndex++;
                        }
                        
                    }
                    List_tbl_GridListing.List_tbl_Pager_To_Client = List_tbl_Pager_To_Client;
                    #endregion

                    #region Preparing data table
                    List<CountryBindingModel> List_CountryBindingModel = _ApplicationDbContext.tbl_Country
                                                                .OrderBy(x => x.CreatedDate)
                                                                .Skip((_Country_Criteria_Model.BatchIndex - 1) * _Country_Criteria_Model.RecordPerBatch)
                                                                .Take(_Country_Criteria_Model.RecordPerBatch)
                                                                .AsEnumerable()
                                                                .Select((x, i) => new CountryBindingModel
                                                                {
                                                                    SrNo = i + 1 + ((_Country_Criteria_Model.BatchIndex - 1) * _Country_Criteria_Model.RecordPerBatch),
                                                                    TotalRecordCount = TotalRecordCount,
                                                                    Id = x.Id.ToString(),
                                                                    Name = x.Name,
                                                                    IsActive = x.IsActive,
                                                                    CreatedBy = x.CreatedBy,
                                                                    CreatedDate = x.CreatedDate,
                                                                    UpdatedBy = x.UpdatedBy,
                                                                    UpdatedDate = x.UpdatedDate
                                                                })
                                                                .OrderBy(x => x.CreatedDate)
                                                                .ToList();

                    List_tbl_GridListing.List_T = List_CountryBindingModel;
                    #endregion

                    List_tbl_Country.Add(List_tbl_GridListing);
                    
                }
            }
            catch (Exception ex) {
                BaseExceptionLogger.LogError(ex, LoggerName);
                throw ex;
            }
            return List_tbl_Country;
        }
    }
}
