using Microsoft.Extensions.Options;
using TechnoApp.Ext.Web.UI.Models;
using System.Net.Mail;

namespace TechnoApp.Ext.Web.UI.Manager
{
//    public class GEmailManager
//    {
//        private readonly SmtpConfig _smtpConfig;
//        public GEmailManager(IOptions<SmtpConfig> smtp)
//        {
//            _smtpConfig = smtp.Value;
//        }

//        /// <summary>
//        /// This function work for mail sending 
//        /// </summary>
//        /// <param name="emailModel">Contains all required field</param>
//        public bool SendEmail(EmailVm emailModel)
//        {
//            if (emailModel.To != null)
//            {
//                //Send Email               
//                SmtpClient SmtpServer = new SmtpClient(_smtpConfig.SmtpServer);
//                SmtpServer.Credentials = new System.Net.NetworkCredential(_smtpConfig.EmailFrom, _smtpConfig.Password);
//                SmtpServer.Port = Convert.ToInt32(_smtpConfig.Port);
//                SmtpServer.EnableSsl = false;

//                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
//                MailMessage mail = new MailMessage();
//                mail.From = new MailAddress(_smtpConfig.EmailFrom, _smtpConfig.DisplayName);
//                mail.To.Add(emailModel.To);

//#if (DEBUG)
//                if (emailModel.Cc != null && emailModel.Cc.Count > 0)
//                {
//                    foreach (string email in emailModel.Cc)
//                    {
//                        mail.CC.Add(email);
//                    }
//                }
//                if (emailModel.Bcc != null && emailModel.Bcc.Count > 0)
//                {
//                    foreach (string email in emailModel.Bcc)
//                    {
//                        mail.Bcc.Add(email);
//                    }
//                }
//#endif
//                mail.Subject = emailModel.Subject;
//                mail.IsBodyHtml = true;
//                mail.Body = emailModel.Body;
//                if (emailModel.AttachmentFileUrl != null)
//                {
//                    mail.Attachments.Add(new Attachment(emailModel.AttachmentFileUrl));
//                }
//                try
//                {
//                    SmtpServer.Send(mail);
//                    return true;
//                }
//                catch (Exception ex)
//                {
//                    return false;
//                }
//            }
//            return false;
//        }

//    }
}
