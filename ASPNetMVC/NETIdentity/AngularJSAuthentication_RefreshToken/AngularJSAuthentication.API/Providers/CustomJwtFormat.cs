using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IdentityModel.Tokens;
using AngularJSAuthentication.API.Entities;

namespace AngularJSAuthentication.API.Providers
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private const string AudiencePropertyKey = "as:client_id";

        private readonly string _issuer = string.Empty;

        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            string audienceId = data.Properties.Dictionary.ContainsKey(AudiencePropertyKey) ? data.Properties.Dictionary[AudiencePropertyKey] : null;
            if (string.IsNullOrWhiteSpace(audienceId)) throw new InvalidOperationException("AuthenticationTicket.Properties does not include client_id");

            string symmetricKeyAsBase64 = string.Empty;
            using (AuthRepository _repo = new AuthRepository())
            {
                Client _Client = _repo.FindClient(audienceId);
                symmetricKeyAsBase64 = _Client.Secret;
            }

            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);
            
            SigningCredentials _SigningCredentials = new SigningCredentials(
                                                            new InMemorySymmetricSecurityKey(keyByteArray),
                                                            "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                                                            "http://www.w3.org/2001/04/xmlenc#sha256");

            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            

            var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, _SigningCredentials);
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);

            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}