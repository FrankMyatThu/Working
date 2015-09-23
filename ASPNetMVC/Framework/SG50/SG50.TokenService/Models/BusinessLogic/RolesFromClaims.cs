using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace SG50.TokenService.Models.BusinessLogic
{
    public class RolesFromClaims
    {
        static string ClaimType_FTE = "FTE";
        static string ClainValue_1 = "1";
        static string RoleAdmin = "Admin";
        static string RoleIncidentResolvers = "IncidentResolvers";
        
        public static IEnumerable<Claim> CreateRolesBasedOnClaims(ClaimsIdentity identity)
        {
            List<Claim> claims = new List<Claim>();

            if (identity.HasClaim(c => c.Type == ClaimType_FTE && c.Value == ClainValue_1) &&
                identity.HasClaim(ClaimTypes.Role, RoleAdmin))
            {
                claims.Add(new Claim(ClaimTypes.Role, RoleIncidentResolvers));
            }

            return claims;
        }
    }
}