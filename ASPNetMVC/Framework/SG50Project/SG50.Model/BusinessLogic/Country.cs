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

        public List<tbl_Country> GetCountry()
        {
            List<tbl_Country> List_tbl_Country = new List<tbl_Country>();
            try {
                using (ApplicationDbContext _ApplicationDbContext = new ApplicationDbContext())
                {
                    List_tbl_Country = _ApplicationDbContext.tbl_Country.Select(x => x)
                                                                                    .Take(10)
                                                                                    .ToList<tbl_Country>();
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
