using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NewsPortal.Models.ManageViewModels
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage identityMessage)
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
                smtpServer.EnableSsl = true;
                smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mailAccount"],
                    ConfigurationManager.AppSettings["mailPassword"]);
                smtpServer.Send(message);
            }
            return Task.FromResult(0);
        }
    }
}