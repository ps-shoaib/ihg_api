using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IGHportalAPI.Services.Common
{
    public class EmailService : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailAddress = _emailSettings.UserName;
            var password = _emailSettings.Password;
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(emailAddress, "coofficeAdminApi-Updates");
            msg.To.Add(new MailAddress(email));
            msg.Subject = subject;
            msg.Body = htmlMessage;
            msg.IsBodyHtml = true;
            msg.Bcc.Add("shoaib@purgesol.com");  //for verification purposes, added my email address

            SmtpClient smtp = new SmtpClient();
            smtp.Host = _emailSettings.Host;
            smtp.Port = _emailSettings.Port;
            smtp.Credentials = new NetworkCredential(emailAddress, password);
            smtp.Timeout = 20000;
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(msg);
        }

    }

    public class EmailSettings
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
    }
}
