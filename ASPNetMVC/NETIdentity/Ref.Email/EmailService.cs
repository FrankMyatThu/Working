using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using MDL.Base.BaseExeption;

namespace MDL.Base.Email
{
    public enum EmailType
    {
        HTML,
        TEXT
    };
    public class EmailService
    {
        private void Invoke_SMTP_Client(MailMessage _MailMessage)
        {
            try
            {
                if (ApplicationSettings.IsDeveloperMode)
                {
                    /// Developer mode                    
                    var _SmtpClient = new SmtpClient()
                    {
                        Host = ApplicationSettings.SMTP_DEV_HostName,
                        Port = ApplicationSettings.SMTP_DEV_Port,
                        EnableSsl = ApplicationSettings.SMTP_DEV_EnableSSL,
                        Timeout = 100000, // 100 seconds || The default value is 100,000 (100 seconds). || http://msdn.microsoft.com/en-us/library/system.net.mail.smtpclient.timeout(v=vs.110).aspx
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential()
                        {
                            UserName = ApplicationSettings.SMTP_DEV_User,
                            Password = ApplicationSettings.SMTP_DEV_Password
                        }
                    };
                    _SmtpClient.Send(_MailMessage);
                }
                else
                {
                    /// Live mode
                    var _SmtpClient = new SmtpClient()
                    {
                        Host = ApplicationSettings.SMTP_MDL_HostName,
                        Port = ApplicationSettings.SMTP_MDL_Port,
                        EnableSsl = ApplicationSettings.SMTP_MDL_EnableSSL,
                        Timeout = 100000, // 100 seconds || The default value is 100,000 (100 seconds). || http://msdn.microsoft.com/en-us/library/system.net.mail.smtpclient.timeout(v=vs.110).aspx
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential()
                        {
                            Domain = ApplicationSettings.SMTP_MDL_Domain,
                            UserName = ApplicationSettings.SMTP_MDL_User,
                            Password = ApplicationSettings.SMTP_MDL_Password
                        }
                    };
                    _SmtpClient.Send(_MailMessage);
                }
            }
            catch (Exception ex)
            {   
                throw ex;
            }            
        }

        public bool SendEmail(string fromAddress, string toAddress, string Subject, string Body, EmailType type, ref string Message)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(fromAddress);
                msg.To.Add(new MailAddress(toAddress));
                msg.Subject = Subject;
                msg.Body = Body;
                if (type == EmailType.TEXT)
                    msg.IsBodyHtml = false;
                else
                    msg.IsBodyHtml = true;


                Invoke_SMTP_Client(msg);
                return true;
            }
            catch (Exception ex)
            {
                BaseException.LogError(ex);
                ApplicationLog.WriteInfo(ex.Message);
                Message = ex.Message;
                return false;
            }
        }

        public bool SendEmail(string fromAddress, string toAddress, string Subject, string Body, EmailType type)
        {
            try
            {

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(fromAddress);
                msg.To.Add(new MailAddress(toAddress));
                msg.Subject = Subject;
                msg.Body = Body;
                if (type == EmailType.TEXT) msg.IsBodyHtml = false;
                else msg.IsBodyHtml = true;

                Invoke_SMTP_Client(msg);

                return true;
            }
            catch (Exception ex)
            {
                BaseException.LogError(ex);
                return false;
            }
        }

        public bool SendEmail(string fromAddress, string[] toAddress, string Subject, string Body, EmailType type)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(fromAddress);
                for (int i = 0; i < toAddress.Length; i++)
                {
                    msg.To.Add(new MailAddress(toAddress[i]));
                }
                msg.Subject = Subject;
                msg.Body = Body;
                if (type == EmailType.TEXT) msg.IsBodyHtml = false;
                else msg.IsBodyHtml = true;

                Invoke_SMTP_Client(msg);

                return true;
            }
            catch (Exception ex)
            {
                BaseException.LogError(ex);
                return false;
            }
        }

        public bool SendEmail(string fromAddress, string[] toAddress, string[] ccAddress, string Subject, string Body, EmailType type)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(fromAddress);
                for (int i = 0; i < toAddress.Length; i++)
                {
                    msg.To.Add(new MailAddress(toAddress[i]));
                }

                for (int i = 0; i < ccAddress.Length; i++)
                {
                    msg.CC.Add(new MailAddress(ccAddress[i]));
                }

                msg.Subject = Subject;
                msg.Body = Body;
                if (type == EmailType.TEXT) msg.IsBodyHtml = false;
                else msg.IsBodyHtml = true;

                Invoke_SMTP_Client(msg);

                return true;
            }
            catch (Exception ex)
            {
                BaseException.LogError(ex);
                return false;
            }
        }

        public bool SendEmail(string toAddress, string Subject, string Body, EmailType type)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(ApplicationSettings.IsDeveloperMode == true ? ApplicationSettings.SMTP_DEV_Default : ApplicationSettings.SMTP_MDL_Default);
                msg.To.Add(new MailAddress(toAddress));
                msg.Subject = Subject;
                msg.Body = Body;
                if (type == EmailType.TEXT) msg.IsBodyHtml = false;
                else msg.IsBodyHtml = true;

                Invoke_SMTP_Client(msg);

                return true;
            }
            catch (Exception ex)
            {
                BaseException.LogError(ex);
                return false;
            }
        }

        /// <summary>
        /// This function is used by customized_mail_send service.
        /// </summary>
        /// <param name="FromAddress"></param>
        /// <param name="ToAddress"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="_EmailType"></param>
        /// <param name="FileCollection"></param>
        /// <returns></returns>
        public bool SendEmail(string FromAddress, string ToAddress, string Subject, string Body, EmailType _EmailType, ICollection<Attachment> FileCollection)
        {   
            try
            {
                MailMessage _MailMessage = new MailMessage();
                _MailMessage.From = new MailAddress(FromAddress);
                _MailMessage.To.Add(new MailAddress(ToAddress));
                _MailMessage.Subject = Subject;
                _MailMessage.Body = Body;
                _MailMessage.IsBodyHtml = _EmailType == EmailType.HTML ? true : false;

                foreach (Attachment File in FileCollection)
                    _MailMessage.Attachments.Add(File);

                Invoke_SMTP_Client(_MailMessage);
                
                return true;

            }
            catch (Exception ex)
            {
                BaseException.LogError(ex);
                return false;
            }
        }
        public bool SendEmail(string toAddress, string Subject, string Body, EmailType type, ICollection<Attachment> FileCollection)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(ApplicationSettings.IsDeveloperMode == true ? ApplicationSettings.SMTP_DEV_Default : ApplicationSettings.SMTP_MDL_Default);
                msg.To.Add(new MailAddress(toAddress));
                msg.Subject = Subject;
                msg.Body = Body;
                if (type == EmailType.TEXT) msg.IsBodyHtml = false;
                else msg.IsBodyHtml = true;

                foreach (Attachment File in FileCollection)
                    msg.Attachments.Add(File);

                Invoke_SMTP_Client(msg);

                return true;

            }
            catch (Exception ex)
            {
                BaseException.LogError(ex);
                return false;
            }
        }

        public bool SendEmail(string fromAddress, string toAddress, string ccAddress, string Subject, string Body, EmailType type)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(fromAddress);
                //msg.To.Add(new MailAddress(toAddress));
                if (toAddress.ToString().Length > 0)
                {
                    foreach (var toEmail in toAddress.Split(';'))
                    {
                        if (toEmail.ToString().Length > 0)
                            msg.To.Add(new MailAddress(toEmail));
                    }
                }
                if (ccAddress.ToString().Length > 0)
                {
                    foreach (var ccEmail in ccAddress.Split(';'))
                        if (ccEmail.ToString().Length > 0)
                            msg.CC.Add(new MailAddress(ccEmail));
                }
                msg.Subject = Subject;
                msg.Body = Body;
                if (type == EmailType.TEXT) msg.IsBodyHtml = false;
                else msg.IsBodyHtml = true;
                
                Invoke_SMTP_Client(msg);

                return true;

            }
            catch (Exception ex)
            {
                BaseException.LogError(ex);
                throw ex;
            }

        }

        public bool SendEmail(string fromAddress, string toAddress, string ccAddress, string Subject, string Body, EmailType type, ICollection<Attachment> FileCollection)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(fromAddress);

                //string[] toAdd = toAddress.Split(';');
                foreach (var toEmail in toAddress.Split(';'))
                    if (toEmail.ToString().Length > 0)
                        msg.To.Add(new MailAddress(toEmail));

                //string[] ccAdd = ccAddress.Split(';');
                if (ccAddress.ToString().Length > 0)
                {
                    foreach (var ccEmail in ccAddress.Split(';'))
                        if (ccEmail.ToString().Length > 0)
                            msg.CC.Add(new MailAddress(ccEmail));
                }
                MDL.Base.ApplicationLog.WriteInfo("In Base -> From Mail :" + msg.From.ToString(), "NewEMail_Services_Logger");
                MDL.Base.ApplicationLog.WriteInfo("In Base -> TO Mail :" + msg.To.ToString(), "NewEMail_Services_Logger");
                MDL.Base.ApplicationLog.WriteInfo("In Base -> CC Mail :" + msg.CC.ToString(), "NewEMail_Services_Logger");
                
                
                msg.Subject = Subject;
                msg.Body = Body;

                MDL.Base.ApplicationLog.WriteInfo("In Base -> Subject :" + msg.Subject.ToString(), "NewEMail_Services_Logger");
                MDL.Base.ApplicationLog.WriteInfo("In Base -> Body :" + msg.Body.ToString(), "NewEMail_Services_Logger");

                if (type == EmailType.TEXT) msg.IsBodyHtml = false;
                else msg.IsBodyHtml = true;

                foreach (Attachment File in FileCollection)
                {
                    msg.Attachments.Add(File);
                    MDL.Base.ApplicationLog.WriteInfo("In Base -> Attach :" + File.ToString(), "NewEMail_Services_Logger");
                }


                Invoke_SMTP_Client(msg);
                
                MDL.Base.ApplicationLog.WriteInfo("In Base -> Success :", "NewEMail_Services_Logger");                
                return true;

            }
            catch (Exception ex)
            {
                BaseException.LogError(ex);
                MDL.Base.ApplicationLog.WriteInfo("In Base -> Error :" + ex.Message, "NewEMail_Services_Logger");
                throw ex;
            }

        }
    }
}
