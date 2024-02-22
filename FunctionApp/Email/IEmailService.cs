namespace FunctionApp.Email
{
    public interface IEmailService
    {
        public Task<bool> SendAsync(string recipientEmail, string fileLink);
    }
}
