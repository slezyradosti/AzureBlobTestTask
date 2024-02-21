using Application.Core;
using Application.Data;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Application.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSecutiry _smtpSecutiry;

        public EmailService(IOptions<SmtpSecutiry> options)
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

        public async Task<Result<string>> SendAsync(string recipientEmail, string fileLink)
        {
            if (string.IsNullOrEmpty(recipientEmail))
                return Result<string>.Failure("Recipient email value cannot be null/empty");
            if (string.IsNullOrEmpty(fileLink))
                return Result<string>.Failure("File link value cannot be null/empty");

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

                    return Result<string>.Success("");
                }
                catch (Exception ex)
                {
                    return Result<string>.Failure(ex.Message);
                }
            }
        }
    }
}
