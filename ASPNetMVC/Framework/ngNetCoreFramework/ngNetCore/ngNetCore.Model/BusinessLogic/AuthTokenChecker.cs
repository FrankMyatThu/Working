using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.DataHandler.Encoder;
using ngNetCore.Base.Logging;
using ngNetCore.Base.Util;
using ngNetCore.Model.Entity;
using ngNetCore.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngNetCore.Model.BusinessLogic
{
    public class AuthTokenChecker
    {
        public string GetAudienceId(string EncodedJWTToken)
        {
            string AudienceId = string.Empty;
            try
            {
                var _JwtSecurityToken = new JwtSecurityToken(EncodedJWTToken);
                AudienceId = _JwtSecurityToken.Audiences.First();
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex);
            }
            return AudienceId;
        }

        public Token_Message_VM IsTokenAuthorized(string EncodedJWTToken, string ClientIPAddress, string ClientBrowserAgentInfo)
        {
            Token_Message_VM _Token_Message_VM = new Token_Message_VM();
            _Token_Message_VM.IsOk = false;
            try
            {
                var _JwtSecurityToken = new JwtSecurityToken(EncodedJWTToken);
                string AudienceId = _JwtSecurityToken.Audiences.First();
                string SecurityKey = string.Empty;
                using (TestScriptEntities _TestScriptEntities = new TestScriptEntities())
                {
                    tbl_LoggedInUser _tbl_LoggedInUser = _TestScriptEntities.tbl_LoggedInUser.Where(x => x.Id.Equals(new Guid(AudienceId))).FirstOrDefault();
                    if (_tbl_LoggedInUser == null)
                    {
                        ApplicationLogger.WriteTrace(string.Format("No record(s) found in tbl_LoggedInUser by AudienceId = {0}.", AudienceId));
                        _Token_Message_VM.MessageType = AppConfiger.Msg_Exception_Biz;
                        _Token_Message_VM.MessageDescription = "No registered user found.";
                        return _Token_Message_VM;
                    }
                    SecurityKey = _tbl_LoggedInUser.JwtHMACKey;

                    var _JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                    //var _JwtSecurityTokenHandler_JWTToken = _JwtSecurityTokenHandler.ReadToken(EncodedJWTToken);

                    /// (1) Validate Token
                    Microsoft.IdentityModel.Tokens.SecurityToken _SecurityToken = null;
                    try
                    {
                        _JwtSecurityTokenHandler.ValidateToken(
                        EncodedJWTToken,
                        new TokenValidationParameters()
                        {
                            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(TextEncodings.Base64Url.Decode(SecurityKey)),
                            ValidAudience = _tbl_LoggedInUser.Id.ToString(),
                            ValidIssuer = AppConfiger.UrlTokenIssuer,
                            ValidateLifetime = true,
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ValidateIssuerSigningKey = true,
                            RequireExpirationTime = true,
                        }, out _SecurityToken);

                        if (_SecurityToken == null)
                        {
                            ApplicationLogger.WriteTrace("Invalid authentication token found.");
                            _Token_Message_VM.MessageType = AppConfiger.Msg_Exception_Biz;
                            _Token_Message_VM.MessageDescription = "Incorrect token found.";
                            return _Token_Message_VM;
                        }
                    }
                    catch (Exception ex)
                    {
                        BaseExceptionLogger.LogError(ex);
                        // Token can be only 2 things.
                        // 1.Fake token
                        // 2.Expired token
                        // [Action] delete

                        string ExceptionMessage = string.Empty;
                        if (ex.Message.Contains("Signature validation failed"))
                        {
                            ExceptionMessage = "Incorrect token found.";
                        }
                        else if (ex.Message.Contains("Lifetime validation failed"))
                        {
                            ExceptionMessage = "Expired token found.";
                        }

                        _TestScriptEntities.tbl_LoggedInUser.Remove(_tbl_LoggedInUser);
                        _TestScriptEntities.SaveChanges();

                        if (ExceptionMessage != string.Empty)
                        {
                            _Token_Message_VM.MessageType = AppConfiger.Msg_Exception_Biz;
                            _Token_Message_VM.MessageDescription = ExceptionMessage;
                            return _Token_Message_VM;
                        }

                        _Token_Message_VM.MessageType = AppConfiger.Msg_Exception_Sys;
                        _Token_Message_VM.MessageDescription = ex.Message + ex.InnerException != null ? ex.InnerException.Message : string.Empty;
                        return _Token_Message_VM;
                    }

                    /// (2) Validate IP & User Agent
                    if (!_tbl_LoggedInUser.IP.Equals(ClientIPAddress, StringComparison.InvariantCultureIgnoreCase) ||
                        !_tbl_LoggedInUser.UserAgent.Equals(ClientBrowserAgentInfo, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ApplicationLogger.WriteTrace("Invalid IP or User Agent found.");
                        _Token_Message_VM.MessageType = AppConfiger.Msg_Exception_Biz;
                        _Token_Message_VM.MessageDescription = "Invalid IP or User Agent found.";
                        return _Token_Message_VM;
                    }


                    ApplicationLogger.WriteTrace("Valid token found.");
                    _Token_Message_VM.IsOk = true;
                    _Token_Message_VM.MessageType = AppConfiger.Msg_Info;
                    _Token_Message_VM.MessageDescription = "Valid token found.";
                }
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex);
            }
            return _Token_Message_VM;
        }
    }
}
