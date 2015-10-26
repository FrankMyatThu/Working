using Microsoft.Owin.Security.DataHandler.Encoder;
using SG50.Base.Logging;
using SG50.Base.Security;
using SG50.Base.Util;
using SG50.Model.Entity;
using SG50.Model.POCO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG50.Model.BusinessLogic
{
    public class AuthTokenChecker
    {
        string LoggerName = "SG50Project_Appender_Logger";

        public bool IsTokenAuthorized(string EncodedJWTToken, string ClientIPAddress, string ClientBrowserAgentInfo)
        {
            bool IsValidToken = false;
            try
            {
                var _JwtSecurityToken = new JwtSecurityToken(EncodedJWTToken);
                string AudienceId = _JwtSecurityToken.Audiences.First();
                string SecurityKey = string.Empty;
                using (ApplicationDbContext _ApplicationDbContext = new ApplicationDbContext())
                {
                    tbl_ActiveUser _tbl_ActiveUser = _ApplicationDbContext.tbl_ActiveUser.Where(x => x.Id.Equals(new Guid(AudienceId))).FirstOrDefault();
                    if (_tbl_ActiveUser == null)
                    {
                        ApplicationLogger.WriteTrace(string.Format("No record(s) found in tbl_ActiveUser by AudienceId = {0}.", AudienceId), LoggerName);
                        return IsValidToken;
                    }
                    SecurityKey = _tbl_ActiveUser.JwtHMACKey;

                    var _JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                    var _JwtSecurityTokenHandler_JWTToken = _JwtSecurityTokenHandler.ReadToken(EncodedJWTToken);

                    /// (1) Validate Token
                    SecurityToken _SecurityToken = null;
                    _JwtSecurityTokenHandler.ValidateToken(
                        EncodedJWTToken,
                        new TokenValidationParameters()
                        {
                            IssuerSigningKey = new InMemorySymmetricSecurityKey(TextEncodings.Base64Url.Decode(SecurityKey)),
                            ValidAudience = _tbl_ActiveUser.Id.ToString(),
                            ValidIssuer = AppConfiger.UrlTokenIssuer,
                            ValidateLifetime = false,
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ValidateIssuerSigningKey = true
                        }, out _SecurityToken);

                    if (_SecurityToken == null)
                    {
                        ApplicationLogger.WriteTrace("Invalid authentication token found.", LoggerName);
                        return IsValidToken;
                    }

                    /// (2) Validate if token is already expired.
                    if ((new LoginChecker()).IsUserIdle(_tbl_ActiveUser.LastRequestedTime))
                    {
                        /// Kick out user who is Idle or whose token is expired.
                        _ApplicationDbContext.tbl_ActiveUser.Remove(_tbl_ActiveUser);
                        _ApplicationDbContext.SaveChanges();
                        ApplicationLogger.WriteTrace("Token is already expired. System automatically removed it.", LoggerName);
                        return IsValidToken;
                    }

                    /// (3) Validate IP & User Agent
                    if (!_tbl_ActiveUser.IP.Equals(ClientIPAddress, StringComparison.InvariantCultureIgnoreCase) ||
                        !_tbl_ActiveUser.UserAgent.Equals(ClientBrowserAgentInfo, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ApplicationLogger.WriteTrace("Invalid IP or User Agent found.", LoggerName);
                        return IsValidToken;
                    }

                    /// All Validation are passed.
                    /// So, Set current time to _tbl_ActiveUser.LastRequestedTime
                    /// in order to extend token expireation time.
                    _tbl_ActiveUser.LastRequestedTime = DateTime.Now;
                    _ApplicationDbContext.SaveChanges();                    
                    IsValidToken = true;
                    ApplicationLogger.WriteTrace("Valid token found.", LoggerName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IsValidToken;
        }
    }
}
