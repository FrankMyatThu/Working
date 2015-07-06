using Microsoft.AspNet.Identity;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
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
            var myMessage = new SendGridMessage();

            myMessage.From = new System.Net.Mail.MailAddress("taiseer@bitoftech.net", "Taiseer Joudeh");
            myMessage.AddTo(message.Destination);            
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailService:Account"],
                                                    ConfigurationManager.AppSettings["emailService:Password"]);

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                try
                {
                    await transportWeb.DeliverAsync(myMessage);
                }
                catch (Exception ex) {
                    /*
                     * later will change to gmail service
                     * 
                        SELECT * FROM dbo.AspNetUsers 
                        SELECT * FROM dbo.AspNetUserRoles 
                        SELECT * FROM dbo.AspNetUserLogins 
                        SELECT * FROM dbo.AspNetUserClaims 
                        SELECT * FROM dbo.AspNetRoles 

                        --DELETE FROM dbo.AspNetUsers WHERE dbo.AspNetUsers.Email = 'myatthu1986.developer@gmail.com'
                     */
                    throw ex;
                }
                
            }
            else
            {
                //Trace.TraceError("Failed to create Web transport.");
                await Task.FromResult(0);
            }
        }
    }
}