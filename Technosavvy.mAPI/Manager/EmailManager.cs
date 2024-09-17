using Microsoft.Extensions.Options;
using NavExM.Int.Maintenance.APIs.Services;
using System.Net.Mail;

namespace NavExM.Int.Maintenance.APIs.Manager
{    
    public class EmailManager
    {
        private readonly SmtpConfig _smtpConfig;
        public EmailManager(IOptions<SmtpConfig> smtp)
        {
            _smtpConfig = smtp.Value;
        }
        public EmailManager(SmtpConfig smtp)
        {
            _smtpConfig = smtp;
        }

        /// <summary>
        /// This function work for mail sending 
        /// </summary>
        /// <param name="emailModel">Contains all required field</param>
        public bool SendEmail(mEmail emailModel)
        {
            if (emailModel.To != null)
            {
                //Send Email 
                SmtpClient SmtpServer = new SmtpClient(_smtpConfig.SmtpServer);
                SmtpServer.Credentials = new System.Net.NetworkCredential(_smtpConfig.EmailFrom, _smtpConfig.Password);
                SmtpServer.Port = Convert.ToInt32(_smtpConfig.Port);
                SmtpServer.EnableSsl = true;

                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(_smtpConfig.EmailFrom, _smtpConfig.DisplayName);
                mail.To.Add(emailModel.To);
                mail.Subject = emailModel.Subject;
                mail.IsBodyHtml = true;
                mail.Body = emailModel.Body;
                if (!string.IsNullOrEmpty(emailModel.AttachmentFileUrl))
                {
                    mail.Attachments.Add(new Attachment(emailModel.AttachmentFileUrl));
                }
                try
                {
                    SmtpServer.Send(mail);
                    return true;
                }
                catch (Exception ex)
                {
                    Console2.WriteLine_RED($"Error in SendEmail:{ex.GetDeepMsg()}");
                    return false;
                }
            }
            return false;
        }

    }
}
