using SG50.Base.ForgeryProtector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SG50.BusinessService.Controllers
{
    [RoutePrefix("api/home")]
    public class HomeController : ApiController
    {
        [HttpPost]
        [Authorize]
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
