using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;

namespace GeekQuiz.TokenServer.Utility
{
    public class TokenUtil
    {
        private string GetGeneratedToken()
        {
            string audienceId = "GeekQuizClientApp";
            string symmetricKeyAsBase64 = "5YV7M1r981yoGhELyB84aC+KiYksxZf1OY3++C1CtRM=";
            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

            SigningCredentials _SigningCredentials = new SigningCredentials(
                                                            new InMemorySymmetricSecurityKey(keyByteArray),
                                                            "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                                                            "http://www.w3.org/2001/04/xmlenc#sha256");

            var issued = DateTime.Now;
            var expires = DateTime.Now.AddMinutes(30);


            var token = new JwtSecurityToken("http://localhost:1622", audienceId, null, issued, expires, _SigningCredentials);
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);

            return jwt;
            //return "";        
        }
    }
}