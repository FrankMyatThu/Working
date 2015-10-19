using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace SG50.TokenService.Models.BusinessLogic
{
    public class ClaimManager
    {
        public static Claim GetClaim(string type, string value)
        {
            Claim _Claim = new Claim(type, value, ClaimValueTypes.String);
            return _Claim;
        }
        
        public static IEnumerable<Claim> GetClaim_IEnumerable(string type, string value)
        {
            List<Claim> List_Claim = new List<Claim>();
            List_Claim.Add(new Claim(type, value, ClaimValueTypes.String));
            return List_Claim;
        }
    }
}