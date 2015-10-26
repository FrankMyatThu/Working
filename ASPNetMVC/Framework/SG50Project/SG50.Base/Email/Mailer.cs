using SG50.Base.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SG50.Base.Email
{
    public class Mailer
    {
        string True_Boolean = "true";
        public async Task sendMailasync(string _Destination, string _Subject, string _Body)
        {
            try
            {
                MailMessage _MailMessage = new MailMessage();
                _MailMessage.From = new MailAddress(AppConfiger.DefaultFromEmailAddress);
                _MailMessage.To.Add(new MailAddress(_Destination));
                _MailMessage.Subject = _Subject;
                _MailMessage.Body = _Body;

                var _SmtpClient = new SmtpClient()
                {
                    Host = AppConfiger.SMTP_HostName,
                    Port = Convert.ToInt32(AppConfiger.SMTP_Port),
                    EnableSsl = AppConfiger.SMTP_EnableSSL.ToString().Equals(True_Boolean, StringComparison.OrdinalIgnoreCase),
                    Timeout = 100000, // 100 seconds || The default value is 100,000 (100 seconds). || http://msdn.microsoft.com/en-us/library/system.net.mail.smtpclient.timeout(v=vs.110).aspx
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential()
                    {
                        UserName = AppConfiger.SMTP_UserName,
                        Password = AppConfiger.SMTP_Password
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
