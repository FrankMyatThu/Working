using SG50.TokenService.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace SG50.TokenService.Models.BusinessLogic
{
    public static class ExtendedClaimsProvider
    {
        static string ClaimType_FTE = "FTE";
        static string ClaimValue_1 = "1";
        static string ClaimValue_0 = "0";
        
        public static IEnumerable<Claim> GetClaims(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>();

            var daysInWork = (DateTime.Now.Date - user.JoinDate).TotalDays;            

            if (daysInWork > 90)
            {
                claims.Add(CreateClaim(ClaimType_FTE, ClaimValue_1));
            }
            else
            {
                claims.Add(CreateClaim(ClaimType_FTE, ClaimValue_0));
            }

            

            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }

    }
}