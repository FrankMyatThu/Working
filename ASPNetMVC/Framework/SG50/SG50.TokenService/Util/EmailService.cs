using Microsoft.AspNet.Identity;
using SG50.Base.Email;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace SG50.TokenService.Util
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendasync(message);
        }

        private async Task configSendasync(IdentityMessage message)
        {   
            try
            {
                Mailer _Mailer = new Mailer();
                await _Mailer.sendMailasync(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }    
}