using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AngularJSAuthentication.API.Providers
{
    public class SimpleAccessTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            //string _access_token = context.Token;
            //context.SetToken("Test1");
            string test = context.SerializeTicket();

            return;
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            return;
        }

    }
}