using FunctionApp.Data;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace FunctionApp.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpData _smtpSecutiry;

        public EmailService(IOptions<SmtpData> options)
        {
            _smtpSecutiry = options.Value;
        }

        private EmailMessage FormMessage(string recipientEmail, string fileLink)
        {
            var message = new EmailMessage()
            {
                SenderEmail = _smtpSecutiry.Login,
                SenderName = "Document Service",
                RecipientEmail = recipientEmail,
                RecipientName = "User",
                Subject = "File Uploading Notification",
                Content = $"Hello, your file is successfuly uploaded!\nThe file is available with 1 hour by link: {fileLink}",
            };

            return message;
        }

        public async Task<bool> SendAsync(string recipientEmail, string fileLink)
        {
            if (string.IsNullOrEmpty(recipientEmail))
                return false;
            if (string.IsNullOrEmpty(fileLink))
                return false;

            var emailMessage = FormMessage(recipientEmail, fileLink);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailMessage.SenderName, emailMessage.SenderEmail));
            message.To.Add(new MailboxAddress(emailMessage.RecipientName, emailMessage.RecipientEmail));
            message.Subject = emailMessage.Subject;

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };

            using (var emailClient = new SmtpClient())
            {
                try
                {
                    // SecureSocketOptions.Auto for SSL.
                    await emailClient.ConnectAsync(_smtpSecutiry.SMTPServer, _smtpSecutiry.Port, SecureSocketOptions.Auto);

                    //Remove any OAuth functionality as we won't be using it.
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    await emailClient.AuthenticateAsync(_smtpSecutiry.Login, _smtpSecutiry.Password);
                    await emailClient.SendAsync(message);
                    await emailClient.DisconnectAsync(true);

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
