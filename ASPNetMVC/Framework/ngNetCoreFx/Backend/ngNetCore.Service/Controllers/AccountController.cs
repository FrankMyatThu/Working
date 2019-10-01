using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ngNetCore.Base.Logging;
using Microsoft.AspNetCore.Authorization;
using ngNetCore.Model.ViewModel;
using ngNetCore.Base.Util;
using ngNetCore.Model.BusinessLogic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ngNetCore.Service.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        public const string Key_ModelStateInvalidError = "Key_ModelStateInvalidError";

        [HttpGet]
        [Route("GenerateToken")]
        public IActionResult GenerateToken()
        {
            ApplicationLogger.WriteTrace("Start GenerateToken");
            string _JwtTokenDummy = Guid.NewGuid().ToString() + " Developer testing";
            Response.Cookies.Append(
                "JWTToken",
                _JwtTokenDummy,
                new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = false,
                }
            );
            return new ObjectResult("GenerateToken._JwtTokenDummy: " + _JwtTokenDummy);
        }

        [HttpGet]
        [Route("RetrieveToken")]
        public IActionResult RetrieveToken()
        {
            return new ObjectResult("RetrieveToken._JwtTokenDummy: " + Request.Cookies["JWTToken"].ToString());
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("CreateUser")]
        public Common_Message_VM CreateUser([FromBody] CreateUser_Binding_VM _CreateUser_Binding_VM)
        {
            Common_Message_VM _Common_Message_VM = new Common_Message_VM();

            ApplicationLogger.WriteTrace("Start CreateUser");
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
                ModelState.AddModelError(Key_ModelStateInvalidError, messages);
                ApplicationLogger.WriteError(messages);

                _Common_Message_VM.IsOk = false;
                _Common_Message_VM.MessageType = AppConfiger.Msg_Exception_Biz;
                _Common_Message_VM.MessageDescription = messages;
                return _Common_Message_VM;
            }

            try
            {
                _Common_Message_VM = (new UserAccount()).RegisterUser(_CreateUser_Binding_VM);
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex);
                _Common_Message_VM.MessageType = AppConfiger.Msg_Exception_Sys;
                _Common_Message_VM.MessageDescription = ex.Message + ex.InnerException != null ? ex.InnerException.Message : string.Empty;
                _Common_Message_VM.IsOk = false;
            }

            ApplicationLogger.WriteTrace("End CreateUser");
            return _Common_Message_VM;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("UserLogin")]
        public Token_Message_VM UserLogin([FromBody] LoginUser_Binding_VM _LoginUser_Binding_VM)
        {
            ApplicationLogger.WriteTrace("Start UserLogin");
            Token_Message_VM _Token_Message_VM = new Token_Message_VM();
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                ModelState.AddModelError(Key_ModelStateInvalidError, messages);

                _Token_Message_VM.MessageType = AppConfiger.Msg_Exception_Biz;
                _Token_Message_VM.MessageDescription = messages;
                _Token_Message_VM.IsOk = false;

                ApplicationLogger.WriteError(messages);
                return _Token_Message_VM;
            }

            try
            {
                _Token_Message_VM = (new UserAccount()).UserLogin(_LoginUser_Binding_VM,
                                                        Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                                                        Request.Headers["User-Agent"].ToString());

                if (!_Token_Message_VM.IsOk)
                {
                    return _Token_Message_VM;
                }

                Response.Cookies.Append(
                    "JWTToken",
                    _Token_Message_VM.JWTToken,
                    new Microsoft.AspNetCore.Http.CookieOptions()
                    {
                        Path = "/",
                        HttpOnly = true,
                        Secure = Convert.ToBoolean(AppConfiger.IsUsedHTTPS),
                    }
                );

                _Token_Message_VM.JWTToken = "Transfered to Cookie.";
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex);
                _Token_Message_VM.MessageType = AppConfiger.Msg_Exception_Sys;
                _Token_Message_VM.MessageDescription = ex.Message + ex.InnerException != null ? ex.InnerException.Message : string.Empty;
                _Token_Message_VM.IsOk = false;
            }

            ApplicationLogger.WriteTrace("End UserLogin");
            return _Token_Message_VM;
        }
    }
}
