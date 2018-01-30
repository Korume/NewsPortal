using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NewsPortal.Managers.Identity
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage identityMessage)
        {
            var smtpSettings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            using (var message = new MailMessage(smtpSettings.From, identityMessage.Destination))
            using (var smtpServer = new SmtpClient())
            {
                message.Subject = identityMessage.Subject;
                message.Body = identityMessage.Body;
                message.IsBodyHtml = true;

                smtpServer.Host = smtpSettings.Network.Host;
                smtpServer.Port = smtpSettings.Network.Port;
                smtpServer.EnableSsl = smtpSettings.Network.EnableSsl;
                smtpServer.DeliveryMethod = smtpSettings.DeliveryMethod;
                smtpServer.UseDefaultCredentials = smtpSettings.Network.DefaultCredentials;
                smtpServer.Credentials = new NetworkCredential(smtpSettings.Network.UserName, smtpSettings.Network.Password);
                await smtpServer.SendMailAsync(message);
            }
        }
    }
}
