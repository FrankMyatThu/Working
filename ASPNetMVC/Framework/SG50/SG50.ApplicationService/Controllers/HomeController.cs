using SG50.Base.ForgeryProtector;
using SG50.Base.Security;
using SG50.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SG50.ApplicationService.Controllers
{
    [RoutePrefix("api/home")]
    public class HomeController : ApiController
    {
        [HttpPost]        
        [CustomizedAuthorization]
        [AllowAnonymous]
        [Route("GetUserList")]
        [ValidateAntiForgeryToken]
        public IHttpActionResult GetUserList()
        {
            string ReturnString = string.Empty;
            try
            {
                ReturnString = "Returned UserList .... bla bla bla ...";
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(ReturnString);
        }
    }
}
