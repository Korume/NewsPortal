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
            using (var message = new MailMessage())
            using (var smtpServer = new SmtpClient())
            {
                message.To.Add(identityMessage.Destination);
                message.Subject = identityMessage.Subject;
                message.Body = identityMessage.Body;
                message.IsBodyHtml = true;

                await smtpServer.SendMailAsync(message);
            }
        }
    }
}
