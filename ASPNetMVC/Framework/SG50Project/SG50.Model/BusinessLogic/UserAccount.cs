using SG50.Base.Logging;
using SG50.Base.Security;
using SG50.Base.Util;
using SG50.Base.ViewModel;
using SG50.Model.Entity;
using SG50.Model.POCO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SG50.Model.BusinessLogic
{
    public class UserAccount
    {
        string ClaimType_Dummy = "Dummy Type";
        string ClaimValue_Dummy = "Dummy Value";
        string UserPasswordNotCorrect = "Email address or password is incorrect.";
        string SignatureAlgorithm = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256";
        string DigestAlgorithm = "http://www.w3.org/2001/04/xmlenc#sha256";
        string LoggerName = "SG50_TokenService_Appender_Logger";
        int SaltKeyLength = 16;

        public void RegisterUser(CreateUserBindingModel _CreateUserBindingModel)
        {
            try
            {
                using (ApplicationDbContext _ApplicationDbContext = new ApplicationDbContext())
                {
                    var SaltKey_ByteArray = Security.GetSaltKey(SaltKeyLength);
                    var _tbl_User = new tbl_User()
                    {
                        FirstName = _CreateUserBindingModel.FirstName,
                        LastName = _CreateUserBindingModel.LastName,
                        Email = _CreateUserBindingModel.Email,                        
                        SaltKey = Convert.ToBase64String(SaltKey_ByteArray),
                        HashedPassword = Security.HashCode(Converter.GetBytes(_CreateUserBindingModel.Password), SaltKey_ByteArray),
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedBy = _CreateUserBindingModel.UserName,
                        UpdatedDate = null,
                        UpdatedBy = null
                    };
                    _ApplicationDbContext.tbl_User.Add(_tbl_User);
                    _ApplicationDbContext.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder _ExceptionMessageList = new StringBuilder();
                foreach (var _ex in ex.EntityValidationErrors)
                {
                    ApplicationLogger.WriteError(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        _ex.Entry.Entity.GetType().Name, _ex.Entry.State),
                        LoggerName);

                    foreach (var __ex in _ex.ValidationErrors)
                    {
                        ApplicationLogger.WriteError(string.Format("- Property: \"{0}\", Error: \"{1}\"", __ex.PropertyName, __ex.ErrorMessage), LoggerName);
                        _ExceptionMessageList.Append(__ex.ErrorMessage);
                    }
                }
                throw new Exception(_ExceptionMessageList.ToString());
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                throw ex;
            }
        }

        public void RemoveActiveUser(string EncodedJWTToken)
        {
            try
            {
                var _JwtSecurityToken = new JwtSecurityToken(EncodedJWTToken);
                string AudienceId = _JwtSecurityToken.Audiences.First();

                using (ApplicationDbContext _ApplicationDbContext = new ApplicationDbContext())
                {
                    tbl_ActiveUser _tbl_ActiveUser = _ApplicationDbContext.tbl_ActiveUser.Where(x => x.Id.Equals(new Guid(AudienceId))).FirstOrDefault();
                    if (_tbl_ActiveUser == null)
                        return;

                    _ApplicationDbContext.tbl_ActiveUser.Remove(_tbl_ActiveUser);
                    _ApplicationDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                throw ex;
            }
        }

        public string GetJWTToken(LoginUserBindingModel _LoginUserBindingModel, string ClientIPAddress, string ClientBrowserAgentInfo)
        {
            string JWTToken = string.Empty;
            try
            {
                using (ApplicationDbContext _ApplicationDbContext = new ApplicationDbContext())
                {
                    tbl_User _tbl_User = _ApplicationDbContext.tbl_User.FirstOrDefault(x => x.Email.Equals(_LoginUserBindingModel.Email));
                    if (_tbl_User == null)
                    {
                        throw new Exception(UserPasswordNotCorrect);
                    }

                    string Hashed_Password = Security.HashCode(Converter.GetBytes(_LoginUserBindingModel.Password), Convert.FromBase64String(_tbl_User.SaltKey));

                    if (!_tbl_User.HashedPassword.Equals(Hashed_Password))
                    {
                        throw new Exception(UserPasswordNotCorrect);
                    }

                    tbl_ActiveUser _tbl_ActiveUser = _ApplicationDbContext.tbl_ActiveUser.Where(x => x.UserId.Equals(_tbl_User.Id)).FirstOrDefault();
                    /// Checking if user is already login.
                    if (_tbl_ActiveUser != null)
                    {
                        /// Checking if user is Idle.
                        if ((new LoginChecker()).IsUserIdle(_tbl_ActiveUser.LastRequestedTime))
                        {
                            /// Kick out user who is Idle.
                            _ApplicationDbContext.tbl_ActiveUser.Remove(_tbl_ActiveUser);
                        }
                        else
                        {
                            /// Current user is using on other machine or on the same machine without logging out properly.
                            /// So raise error for those people/person who want to use the same account at the same moment.
                            string _ExceptionMessage = string.Format(AppConfiger.LoginNotificationMessage, AppConfiger.ApplicationTokenLifeTime);
                            throw new Exception(_ExceptionMessage);
                        }
                    }

                    _tbl_ActiveUser = CreateActiveUser(_tbl_User, ClientIPAddress, ClientBrowserAgentInfo);
                    _ApplicationDbContext.tbl_ActiveUser.Add(_tbl_ActiveUser);

                    string audienceId = _tbl_ActiveUser.Id.ToString();
                    string symmetricKeyAsBase64 = _tbl_ActiveUser.JwtHMACKey;

                    var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

                    SigningCredentials _SigningCredentials = new SigningCredentials(
                                                                    new InMemorySymmetricSecurityKey(keyByteArray),
                                                                    SignatureAlgorithm,
                                                                    DigestAlgorithm);



                    var token = new JwtSecurityToken(AppConfiger.UrlTokenIssuer,
                                                        audienceId,
                                                        null,
                                                        DateTime.Now,
                                                        DateTime.Now.AddSeconds(1),
                                                        _SigningCredentials);

                    var handler = new JwtSecurityTokenHandler();

                    JWTToken = handler.WriteToken(token);
                    _ApplicationDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                throw ex;
            }
            return JWTToken;
        }

        private tbl_ActiveUser CreateActiveUser(tbl_User _tbl_User, string ClientIPAddress, string ClientBrowserAgentInfo)
        {
            tbl_ActiveUser _tbl_ActiveUser = new tbl_ActiveUser();
            _tbl_ActiveUser.Id = Guid.NewGuid();
            _tbl_ActiveUser.tbl_User = _tbl_User;
            _tbl_ActiveUser.UserId = _tbl_User.Id;
            _tbl_ActiveUser.IP = ClientIPAddress;
            _tbl_ActiveUser.UserAgent = ClientBrowserAgentInfo;
            _tbl_ActiveUser.JwtHMACKey = Convert.ToBase64String((new AesManaged()).Key);
            _tbl_ActiveUser.IsActive = true;
            _tbl_ActiveUser.CreatedDate = DateTime.Now;
            _tbl_ActiveUser.CreatedBy = _tbl_User.Email;
            _tbl_ActiveUser.UpdatedDate = null;
            _tbl_ActiveUser.UpdatedBy = null;
            _tbl_ActiveUser.LastRequestedTime = DateTime.Now;
            return _tbl_ActiveUser;
        }
    }
}
