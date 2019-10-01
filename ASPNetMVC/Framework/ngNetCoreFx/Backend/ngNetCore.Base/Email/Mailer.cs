using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ngNetCore.Base.Email
{
    public class Mailer
    {
        //NameValueCollection AppSettings = ConfigurationManager.AppSettings;
        string DefaultFromEmailAddress = "DefaultFromEmailAddress";
        string SMTP_HostName = "SMTP_HostName";
        string SMTP_Port = "SMTP_Port";
        string SMTP_EnableSSL = "SMTP_EnableSSL";
        string SMTP_UserName = "SMTP_UserName";
        string SMTP_Password = "SMTP_Password";
        string True_Boolean = "true";


        public async Task sendMailasync(IdentityMessage message)
        {
            try
            {
                MailMessage _MailMessage = new MailMessage();
                _MailMessage.From = new MailAddress(AppSettings[DefaultFromEmailAddress]);
                _MailMessage.To.Add(new MailAddress(message.Destination));
                _MailMessage.Subject = message.Subject;
                _MailMessage.Body = message.Body;

                //if (type == EmailType.TEXT)
                //    _MailMessage.IsBodyHtml = false;
                //else
                //    _MailMessage.IsBodyHtml = true;

                var _SmtpClient = new SmtpClient()
                {
                    Host = AppConfiger. [SMTP_HostName],
                    Port = Convert.ToInt32(AppSettings[SMTP_Port]),
                    EnableSsl = AppSettings[SMTP_EnableSSL].ToString().Equals(True_Boolean, StringComparison.OrdinalIgnoreCase),
                    Timeout = 100000, // 100 seconds || The default value is 100,000 (100 seconds). || http://msdn.microsoft.com/en-us/library/system.net.mail.smtpclient.timeout(v=vs.110).aspx
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential()
                    {
                        UserName = AppSettings[SMTP_UserName],
                        Password = AppSettings[SMTP_Password]
                    }
                };
                await _SmtpClient.SendMailAsync(_MailMessage);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public enum EmailType
    {
        HTML,
        TEXT
    };
}
