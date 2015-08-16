using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;


namespace GeekQuiz.TokenServer.Controllers
{
    public class AccountController : ApiController
    {
        public IHttpActionResult Test()
        {   
            return Ok();
        }
    }
}
