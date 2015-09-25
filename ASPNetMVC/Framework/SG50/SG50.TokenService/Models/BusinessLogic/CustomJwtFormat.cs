using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using SG50.TokenService.Models.Entities;
using SG50.TokenService.Models.POCO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;


namespace SG50.TokenService.Models.BusinessLogic
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string _issuer = string.Empty;
        string Claim_Column_AppUserID = "AppUserID";        
        string Param_ArgumentNullException = "data";
        string SignatureAlgorithm = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256";
        string DigestAlgorithm = "http://www.w3.org/2001/04/xmlenc#sha256";

        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(Param_ArgumentNullException);
            }

            string AppUserID = data.Identity.Claims.FirstOrDefault(x => x.Type == Claim_Column_AppUserID).Value;
            data.Identity.RemoveClaim(data.Identity.Claims.FirstOrDefault(x => x.Type == Claim_Column_AppUserID));
            using (ApplicationDbContext _ApplicationDbContext = new ApplicationDbContext())
            {
                ApplicationUser _ApplicationUser = _ApplicationDbContext.Users.FirstOrDefault(x => x.Id.Equals(Convert.ToInt32(AppUserID)));                
                ActiveUser _ActiveUser = CreateActiveUser(_ApplicationUser);                
                //_ApplicationDbContext.ActiveUser.Remove(_ActiveUser);
                _ApplicationDbContext.ActiveUser.Add(_ActiveUser);

                string audienceId = _ActiveUser.Id.ToString();
                string symmetricKeyAsBase64 = _ActiveUser.JwtHMACKey;

                var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

                SigningCredentials _SigningCredentials = new SigningCredentials(
                                                                new InMemorySymmetricSecurityKey(keyByteArray),
                                                                SignatureAlgorithm,
                                                                DigestAlgorithm);

                var issued = data.Properties.IssuedUtc;
                var expires = data.Properties.ExpiresUtc;
                var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, _SigningCredentials);
                var handler = new JwtSecurityTokenHandler();

                var jwt = handler.WriteToken(token);
                _ApplicationDbContext.SaveChanges();
                return jwt;
            }
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }

        private ActiveUser CreateActiveUser(ApplicationUser _ApplicationUser)
        {
            /// One to one relationship
            //ActiveUser _ActiveUser = _ApplicationUser.ActiveUser;

            /// One to many relationship, for the future requirement, to be able to change without having bad consequences.
            /// But, according to specific requirement, one user can only have one active record.
            ActiveUser _ActiveUser = _ApplicationUser.ActiveUser.FirstOrDefault();
            _ActiveUser.AppUserId = _ApplicationUser.Id;
            _ActiveUser.IP = HttpContext.Current.Request.UserHostAddress;
            _ActiveUser.UserAgent = HttpContext.Current.Request.UserAgent;
            _ActiveUser.JwtHMACKey = Convert.ToBase64String((new AesManaged()).Key);
            return _ActiveUser;
        }
    }
}