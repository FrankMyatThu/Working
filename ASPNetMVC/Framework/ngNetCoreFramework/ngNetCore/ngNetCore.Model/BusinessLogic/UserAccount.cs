using ngNetCore.Base.Logging;
using ngNetCore.Base.Security;
using ngNetCore.Base.Util;
using ngNetCore.Model.Entity;
using ngNetCore.Model.ViewModel;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.DataHandler.Encoder;

namespace ngNetCore.Model.BusinessLogic
{
    public class UserAccount
    {
        string UserPasswordNotCorrect = "User name or password is incorrect.";
        int SaltKeyLength = 16;

        #region User Registration
        public Common_Message_VM RegisterUser(CreateUser_Binding_VM _CreateUser_Binding_VM)
        {
            Common_Message_VM _Common_Message_VM = new Common_Message_VM();
            try
            {
                using (TestScriptEntities _TestScriptEntities = new TestScriptEntities())
                {
                    var SaltKey_ByteArray = Security.GetSaltKey(SaltKeyLength);
                    Guid KeyId = Guid.NewGuid();
                    var _tbl_User = new tbl_User()
                    {
                        Id = KeyId,
                        UserName = _CreateUser_Binding_VM.UserName,
                        SaltKey = Convert.ToBase64String(SaltKey_ByteArray),
                        PasswordHashed = Security.HashCode(Converter.GetBytes(_CreateUser_Binding_VM.Password), SaltKey_ByteArray),
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedBy = KeyId,
                        UpdatedDate = DateTime.Now,
                        UpdatedBy = KeyId
                    };
                    _TestScriptEntities.tbl_User.Add(_tbl_User);
                    _TestScriptEntities.SaveChanges();
                    _Common_Message_VM.MessageType = AppConfiger.Msg_Info;
                    _Common_Message_VM.MessageDescription = "Registration success.";
                    _Common_Message_VM.IsOk = true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder _ExceptionMessageList = new StringBuilder();
                foreach (var _ex in ex.EntityValidationErrors)
                {
                    ApplicationLogger.WriteError(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        _ex.Entry.Entity.GetType().Name, _ex.Entry.State));

                    foreach (var __ex in _ex.ValidationErrors)
                    {
                        ApplicationLogger.WriteError(string.Format("- Property: \"{0}\", Error: \"{1}\"", __ex.PropertyName, __ex.ErrorMessage));
                        _ExceptionMessageList.Append(__ex.ErrorMessage);
                    }
                }
                BaseExceptionLogger.LogError(ex);
                _Common_Message_VM.MessageType = AppConfiger.Msg_Exception_Sys;
                _Common_Message_VM.MessageDescription = ex.Message + ex.InnerException != null ? ex.InnerException.Message : string.Empty;
                _Common_Message_VM.IsOk = false;
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex);
                _Common_Message_VM.MessageType = AppConfiger.Msg_Exception_Sys;
                _Common_Message_VM.MessageDescription = ex.Message + ex.InnerException != null ? ex.InnerException.Message : string.Empty;
                _Common_Message_VM.IsOk = false;
            }
            return _Common_Message_VM;
        }
        #endregion

        #region User Log in or (TOKEN Generator)
        public Token_Message_VM UserLogin(LoginUser_Binding_VM _LoginUser_Binding_VM, string ClientIPAddress, string ClientBrowserAgentInfo)
        {
            Token_Message_VM _Token_Message_VM = new Token_Message_VM();
            _Token_Message_VM.IsOk = false;
            _Token_Message_VM.JWTToken = string.Empty;
            string JWTToken = string.Empty;
            try
            {
                using (TestScriptEntities _TestScriptEntities = new TestScriptEntities())
                {
                    //List<tbl_AppSetting> List_tbl_AppSetting = _TestScriptEntities.tbl_AppSetting.Where(x => x.IsActive == true).ToList();
                    //double TokenLifeTime = Convert.ToDouble(List_tbl_AppSetting.Where(x => x.AppKey == "TokenLifeTime").Select(x => x.AppValue).FirstOrDefault());
                    double TokenLifeTime = 900000; // 15 Minutes hard coded.

                    tbl_User _tbl_User = _TestScriptEntities.tbl_User.FirstOrDefault(x => x.UserName.Equals(_LoginUser_Binding_VM.UserName));
                    if (_tbl_User == null)
                    {
                        ApplicationLogger.WriteTrace(UserPasswordNotCorrect);
                        _Token_Message_VM.MessageType = AppConfiger.Msg_Exception_Biz;
                        _Token_Message_VM.MessageDescription = UserPasswordNotCorrect;
                        return _Token_Message_VM;
                    }

                    string Hashed_Password = Security.HashCode(Converter.GetBytes(_LoginUser_Binding_VM.Password), Convert.FromBase64String(_tbl_User.SaltKey));

                    if (!_tbl_User.PasswordHashed.Equals(Hashed_Password))
                    {
                        ApplicationLogger.WriteTrace(UserPasswordNotCorrect);
                        _Token_Message_VM.MessageType = AppConfiger.Msg_Exception_Biz;
                        _Token_Message_VM.MessageDescription = UserPasswordNotCorrect;
                        return _Token_Message_VM;
                    }

                    tbl_LoggedInUser _tbl_LoggedInUser = _TestScriptEntities.tbl_LoggedInUser.Where(x =>
                                                                                                    x.UserId == _tbl_User.Id
                                                                                                    && x.IP == ClientIPAddress
                                                                                                    && x.UserAgent == ClientBrowserAgentInfo).FirstOrDefault();
                    /// Checking if user is already login at the same ip and same browser.
                    if (_tbl_LoggedInUser != null)
                    {
                        _TestScriptEntities.tbl_LoggedInUser.Remove(_tbl_LoggedInUser);
                    }

                    _tbl_LoggedInUser = CreateActiveUser(_tbl_User, TokenLifeTime, ClientIPAddress, ClientBrowserAgentInfo);
                    _TestScriptEntities.tbl_LoggedInUser.Add(_tbl_LoggedInUser);

                    string audienceId = _tbl_LoggedInUser.Id.ToString();
                    string symmetricKeyAsBase64 = _tbl_LoggedInUser.JwtHMACKey;

                    JWTToken = GenerateToken(symmetricKeyAsBase64, audienceId, TokenLifeTime);
                    _Token_Message_VM.IsOk = true;
                    _Token_Message_VM.MessageType = AppConfiger.Msg_Info;
                    _Token_Message_VM.MessageDescription = "Login Sucess";
                    _Token_Message_VM.JWTToken = JWTToken;
                    _TestScriptEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex);
                _Token_Message_VM.MessageType = AppConfiger.Msg_Exception_Sys;
                _Token_Message_VM.MessageDescription = ex.Message + ex.InnerException != null ? ex.InnerException.Message : string.Empty;
            }
            return _Token_Message_VM;
        }
        private tbl_LoggedInUser CreateActiveUser(tbl_User _tbl_User, double TokenLifeTime, string ClientIPAddress, string ClientBrowserAgentInfo)
        {
            tbl_LoggedInUser _tbl_LoggedInUser = new tbl_LoggedInUser();
            _tbl_LoggedInUser.Id = Guid.NewGuid();
            _tbl_LoggedInUser.UserId = _tbl_User.Id;
            _tbl_LoggedInUser.IP = ClientIPAddress;
            _tbl_LoggedInUser.UserAgent = ClientBrowserAgentInfo;
            _tbl_LoggedInUser.JwtHMACKey = Convert.ToBase64String((new AesManaged()).Key);
            _tbl_LoggedInUser.IssuedAt = DateTime.Now;
            _tbl_LoggedInUser.ExpiredAt = DateTime.Now.AddMilliseconds(TokenLifeTime);
            _tbl_LoggedInUser.IsActive = true;
            _tbl_LoggedInUser.CreatedDate = DateTime.Now;
            _tbl_LoggedInUser.CreatedBy = _tbl_User.Id;
            _tbl_LoggedInUser.UpdatedDate = DateTime.Now;
            _tbl_LoggedInUser.UpdatedBy = _tbl_User.Id;
            return _tbl_LoggedInUser;
        }
        private string GenerateToken(string symmetricKeyAsBase64, string audienceId, double TokenLifeTime)
        {
            string Return_Token = string.Empty;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var now = DateTime.UtcNow;

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = AppConfiger.UrlTokenIssuer,
                    Audience = audienceId,
                    IssuedAt = now,
                    Expires = now.AddMilliseconds(TokenLifeTime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(TextEncodings.Base64Url.Decode(symmetricKeyAsBase64)), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                Return_Token = tokenString;
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex);
            }
            return Return_Token;
        }
        #endregion
        
        #region User Logging Out
        #endregion
    }
}
