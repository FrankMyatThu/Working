using Microsoft.AspNet.Identity;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace WebApiProject.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridasync(IdentityMessage message)
        {
            MailMessage _MailMessage = new MailMessage();
            _MailMessage.From = new MailAddress("taiseer@bitoftech.net");
            _MailMessage.To.Add(new MailAddress(message.Destination));
            _MailMessage.Subject = message.Subject;
            _MailMessage.Body = message.Body;
            _MailMessage.IsBodyHtml = true;

            var _SmtpClient = new SmtpClient()
            {
                Host = ConfigurationManager.AppSettings["developer.smtp.host"].ToString(),
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["developer.smtp.port"].ToString()),
                EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["developer.smtp.enableSsl"].ToString()),
                Timeout = 100000, // 100 seconds || The default value is 100,000 (100 seconds). || http://msdn.microsoft.com/en-us/library/system.net.mail.smtpclient.timeout(v=vs.110).aspx
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = ConfigurationManager.AppSettings["developer.smtp.user"].ToString(),
                    Password = ConfigurationManager.AppSettings["developer.smtp.password"].ToString(),
                }
            };
            await _SmtpClient.SendMailAsync(_MailMessage);
        }
    }
}