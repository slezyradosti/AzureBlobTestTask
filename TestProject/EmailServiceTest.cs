using Application.Core;
using Application.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
namespace TestProject
{
    public class EmailServiceTest
    {
        private readonly IEmailService _emailService;

        public EmailServiceTest()
        {
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<EmailServiceTest>()
            .Build();

            SmtpSecutiry smtpSecurity = new SmtpSecutiry();
            configuration.GetSection("SmtpGmailSecurity").Bind(smtpSecurity);

            _emailService = new EmailService(Options.Create(smtpSecurity));
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("sss", "sss")]
        [InlineData("sss", null)]
        [InlineData(null, "sss")]
        [InlineData("sss", "")]
        [InlineData("", "sss")]
        public async Task SendMailsFail(string recipientEmail, string fileLink)
        {
            var result1 = await _emailService.SendAsync(recipientEmail, fileLink);
            Assert.False(result1.IsSuccess);
        }

        [Theory]
        [InlineData("existingmail1@mail.com", "testlink")]
        [InlineData("existingmail2@mail.com", "link2")]
        public async Task SendMailsSuccess(string recipientEmail, string fileLink)
        {
            var result1 = await _emailService.SendAsync(recipientEmail, fileLink);
            Assert.True(result1.IsSuccess);
        }
    }
}
