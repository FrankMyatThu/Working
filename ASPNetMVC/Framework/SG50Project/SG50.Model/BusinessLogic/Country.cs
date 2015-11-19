using SG50.Base.Logging;
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

        public List<tbl_GridListing<CountryBindingModel>> GetCountryList(int PagerShowIndexOneUpToX = 5, int RecordPerPage = 5, int BatchIndex = 2, int RecordPerBatch = 25)
        {
            List<tbl_GridListing<CountryBindingModel>> List_tbl_Country = new List<tbl_GridListing<CountryBindingModel>>();
            try {
                using (ApplicationDbContext _ApplicationDbContext = new ApplicationDbContext())
                {
                    tbl_GridListing<CountryBindingModel> List_tbl_GridListing = new tbl_GridListing<CountryBindingModel>();
                    int TotalRecordCount = _ApplicationDbContext.tbl_Country.Count();

                    #region Preparing tbl_Pager
                    List<tbl_Pager> List_tbl_Pager = new List<tbl_Pager>();
                    int TotalPage = (int)Math.Ceiling((double)TotalRecordCount / (double)RecordPerPage);
                    int Pager_BatchIndex = 1;
                    int Pager_BehindTheScenseIndex = 1;
                    for (int i = 0; i < TotalPage; i++)
                    {
                        if (Pager_BehindTheScenseIndex > PagerShowIndexOneUpToX)
                            Pager_BehindTheScenseIndex = 1;

                        if (i > PagerShowIndexOneUpToX)
                            Pager_BatchIndex++;

                        List_tbl_Pager.Add(new tbl_Pager { 
                            BatchIndex = Pager_BatchIndex,
                            DisplayPageIndex = (i+1),
                            BehindTheScenesIndex = Pager_BehindTheScenseIndex
                        });

                        Pager_BehindTheScenseIndex++;
                    }
                    List_tbl_GridListing.List_tbl_Pager = List_tbl_Pager;
                    #endregion

                    #region Preparing data table
                    List<CountryBindingModel> List_CountryBindingModel = _ApplicationDbContext.tbl_Country
                                                                .OrderBy(x => x.CreatedDate)
                                                                .Skip((BatchIndex - 1) * RecordPerBatch)
                                                                .Take(RecordPerBatch)
                                                                .AsEnumerable()
                                                                .Select((x, i) => new CountryBindingModel
                                                                {
                                                                    SrNo = i + 1 + ((BatchIndex - 1) * RecordPerBatch),
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

                    //// *********************************
                    //// Still need to modify order ....
                    //// *********************************
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
