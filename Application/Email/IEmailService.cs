using Application.Core;

namespace Application.Email
{
    public interface IEmailService
    {
        public Task<Result<string>> SendAsync(string recipientEmail, string fileLink);
    }
}
