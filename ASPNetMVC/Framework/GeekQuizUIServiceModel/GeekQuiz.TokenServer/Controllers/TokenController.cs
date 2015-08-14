using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace GeekQuiz.TokenServer.Controllers
{
    [RoutePrefix("api/Token")]
    public class TokenController : ApiController
    {
        [AllowAnonymous]
        [Route("GetTokenByUserInfo")]
        [HttpGet]
        public HttpResponseMessage GetTokenByUserInfo()
        {
            var ReturnTest = "this is return data";
            return Request.CreateResponse(HttpStatusCode.OK, GetGeneratedToken());
        }

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
