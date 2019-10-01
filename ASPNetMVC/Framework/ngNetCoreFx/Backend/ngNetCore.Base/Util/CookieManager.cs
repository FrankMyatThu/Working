using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ngNetCore.Base.Util
{
    public static class CookieManager
    {
        /// <summary>
        /// Adds a Set-Cookie HTTP header for the specified cookie.
        /// WARNING: support for cookie properties is currently VERY LIMITED.
        /// </summary>
        public static void SetCookie(this HttpResponseHeaders headers, Cookie cookie)
        {
            var cookieBuilder = new StringBuilder(HttpUtility.UrlEncode(cookie.Name) + "=" + HttpUtility.UrlEncode(cookie.Value));
            if (cookie.HttpOnly)
            {
                cookieBuilder.Append("; HttpOnly");
            }

            if (cookie.Secure)
            {
                cookieBuilder.Append("; Secure");
            }

            headers.Add("Set-Cookie", cookieBuilder.ToString());
        }
    }
}
