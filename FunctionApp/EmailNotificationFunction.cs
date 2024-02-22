using FunctionApp.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Options;
using Azure.Storage.Blobs.Models;
using FunctionApp.Email;

namespace FunctionApp
{
    public class EmailNotificationFunction
    {
        private readonly EmailService _emailService;

        public EmailNotificationFunction(IOptions<SmtpData> options)
        {
            SmtpData smtpSecutiry = new SmtpData()
            {
                SMTPServer = options.Value.SMTPServer,
                Port = options.Value.Port,
                Login = options.Value.Login,
                Password = options.Value.Password,
            };
            _emailService = new EmailService(Options.Create(smtpSecutiry));
        }

        [Function(nameof(EmailNotificationFunction))]
        public async Task<bool> Run(
            [BlobTrigger("tesktask/{name}", Connection = "AzureWebJobsStorage")] Azure.Storage.Blobs.BlobClient blob)
        {
            BlobProperties properties = await blob.GetPropertiesAsync();

            var email = properties?.Metadata["email"] ?? "";
            var fileLink = properties?.Metadata["fileLink"] ?? "";

            await Task.Delay(4000);
            var result = await _emailService.SendAsync(email, fileLink);

            return result;
        }
    }
}
