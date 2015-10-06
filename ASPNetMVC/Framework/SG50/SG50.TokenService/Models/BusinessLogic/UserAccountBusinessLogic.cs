using Microsoft.Owin.Security.DataHandler.Encoder;
using SG50.Base.Logging;
using SG50.Base.Security;
using SG50.Base.Util;
using SG50.TokenService.Models.Entities;
using SG50.TokenService.Models.POCO;
using SG50.TokenService.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace SG50.TokenService.Models.BusinessLogic
{
    public class UserAccountBusinessLogic
    {
        string ClaimType_Dummy = "Dummy Type";
        string ClaimValue_Dummy = "Dummy Value";
        string UserPasswordNotCorrect = "The user name or password is incorrect.";
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
                    var _ApplicationUser = new ApplicationUser()
                    {
                        FirstName = _CreateUserBindingModel.FirstName,
                        LastName = _CreateUserBindingModel.LastName,
                        Email = _CreateUserBindingModel.Email,
                        UserName = _CreateUserBindingModel.UserName,
                        SaltKey = Convert.ToBase64String(SaltKey_ByteArray),
                        PasswordHash = Security.HashCode(Converter.GetBytes(_CreateUserBindingModel.Password), SaltKey_ByteArray),
                        JoinDate = _CreateUserBindingModel.JoinDate,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedBy = _CreateUserBindingModel.UserName,
                        UpdateDate = null,
                        UpdateBy = null
                    };
                    _ApplicationDbContext.Users.Add(_ApplicationUser);
                    _ApplicationDbContext.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);                
            }
        }

        public string GetJWTToken(LoginUserBindingModel _LoginUserBindingModel)
        {
            string JWTToken = string.Empty;
            try
            {
                using (ApplicationDbContext _ApplicationDbContext = new ApplicationDbContext())
                {
                    ApplicationUser _ApplicationUser = _ApplicationDbContext.Users.FirstOrDefault(x => x.UserName.Equals(_LoginUserBindingModel.UserName));
                    if (_ApplicationUser == null) 
                    {
                        throw new Exception(UserPasswordNotCorrect);
                    }                        

                    string Hashed_Password = Security.HashCode(Converter.GetBytes(_LoginUserBindingModel.Password), Convert.FromBase64String(_ApplicationUser.SaltKey));

                    if (!_ApplicationUser.PasswordHash.Equals(Hashed_Password))
                    {
                        throw new Exception(UserPasswordNotCorrect);
                    }

                    ActiveUser _ActiveUser = _ApplicationDbContext.ActiveUser.Where(x => x.AppUserId.Equals(_ApplicationUser.Id)).FirstOrDefault();
                    if (_ActiveUser != null) 
                    {
                        _ApplicationDbContext.ActiveUser.Remove(_ActiveUser);
                    }

                    _ActiveUser = CreateActiveUser(_ApplicationUser);
                    _ApplicationDbContext.ActiveUser.Add(_ActiveUser);

                    string audienceId = _ActiveUser.Id.ToString();                    
                    string symmetricKeyAsBase64 = _ActiveUser.JwtHMACKey;

                    var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

                    SigningCredentials _SigningCredentials = new SigningCredentials(
                                                                    new InMemorySymmetricSecurityKey(keyByteArray),
                                                                    SignatureAlgorithm,
                                                                    DigestAlgorithm);



                    var token = new JwtSecurityToken(AppConfiger.UrlTokenIssuer, 
                                                        audienceId,
                                                        ClaimManager.GetClaim_IEnumerable(ClaimType_Dummy, ClaimValue_Dummy), 
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
            }
            return JWTToken;
        }

        private ActiveUser CreateActiveUser(ApplicationUser _ApplicationUser)
        {
            /// One to one relationship
            //ActiveUser _ActiveUser = _ApplicationUser.ActiveUser;

            /// One to many relationship, for the future requirement, to be able to change without having bad consequences.
            /// But, according to specific requirement, one user can only have one active record.
            ActiveUser _ActiveUser = new ActiveUser();
            _ActiveUser.Id = Guid.NewGuid();
            _ActiveUser.ApplicationUser = _ApplicationUser;
            _ActiveUser.AppUserId = _ApplicationUser.Id;
            _ActiveUser.IP = HttpContext.Current.Request.UserHostAddress;
            _ActiveUser.UserAgent = HttpContext.Current.Request.UserAgent;
            _ActiveUser.JwtHMACKey = Convert.ToBase64String((new AesManaged()).Key);
            _ActiveUser.IsActive = true;
            _ActiveUser.CreatedDate = DateTime.Now;
            _ActiveUser.CreatedBy = _ApplicationUser.UserName;
            _ActiveUser.UpdateDate = null;
            _ActiveUser.UpdateBy = null;
            return _ActiveUser;
        }
    }
}