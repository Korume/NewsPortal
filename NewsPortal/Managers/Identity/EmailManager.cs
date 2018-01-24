using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NewsPortal.Managers.Identity
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage identityMessage)
        {
            using (MailMessage message = new MailMessage(ConfigurationManager.AppSettings["mailAccount"],
                identityMessage.Destination))
            using (SmtpClient smtpServer = new SmtpClient())
            {
                message.Subject = identityMessage.Subject;
                message.Body = identityMessage.Body;
                message.IsBodyHtml = true;

                smtpServer.Host = ConfigurationManager.AppSettings["mailHost"];
                smtpServer.Port = int.Parse(ConfigurationManager.AppSettings["mailPort"]);
                smtpServer.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["mailEnableSsl"]);
                smtpServer.DeliveryMethod = (SmtpDeliveryMethod)Enum.Parse(typeof(SmtpDeliveryMethod),
                    ConfigurationManager.AppSettings["mailDeliveryMethod"]);
                smtpServer.UseDefaultCredentials = bool.Parse(ConfigurationManager.AppSettings["mailUseDefaultCredentials"]);
                smtpServer.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mailAccount"],
                    ConfigurationManager.AppSettings["mailPassword"]);
                await smtpServer.SendMailAsync(message);
            }
        }
    }
}
