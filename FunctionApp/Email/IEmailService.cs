using Application.Core;

namespace FunctionApp.Email
{
    public interface IEmailService
    {
        public Task<string> SendAsync(string recipientEmail, string fileLink);
    }
}
