using Application.Core;
using FakeItEasy;
using FunctionApp.Data;
using FunctionApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace TestProject
{
    public class BlobAzureFunctionTest
    {
        private readonly IOptions<SmtpData> _emailOptions;

        //TODO
        public BlobAzureFunctionTest()
        {
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<EmailServiceTest>()
            .Build();

            SmtpSecutiry smtpSecurity = new SmtpSecutiry();
            configuration.GetSection("SmtpGmailSecurity").Bind(smtpSecurity);

            var emailOptions = new SmtpData()
            {
                SMTPServer = smtpSecurity.SMTPServer,
                Port = smtpSecurity.Port,
                Login = smtpSecurity.Login,
                Password = smtpSecurity.Password,
            };
            _emailOptions = Options.Create(emailOptions);
        }

        [Theory]
        [InlineData("tes", "link", "name")]
        [InlineData("tes@c.com", null, "name")]
        [InlineData(null, null, null)]
        public async Task FunctionRunFail(string email, string fileLink, string name)
        {
            EmailNotificationFunction emailNotificationFunction = new EmailNotificationFunction(_emailOptions);

            var stream = A.Fake<Stream>();
            Dictionary<string, string> meta = new Dictionary<string, string>();
            meta.Add("email", email);
            meta.Add("fileLink", fileLink);

            bool funcResult = true;
            try
            {
                funcResult = await emailNotificationFunction.Run(stream, name, meta);
            }
            catch (Exception ex)
            {
                funcResult = false;
            }
            finally
            {
                Assert.False(funcResult);
            }
        }

        [Theory]
        [InlineData("existingmail1@mail.com", "link1", "")]
        [InlineData("existingmail2@mail.com", "link2", "")]
        public async Task FunctionRunSuccess(string email, string fileLink, string name)
        {
            EmailNotificationFunction emailNotificationFunction = new EmailNotificationFunction(_emailOptions);

            var stream = A.Fake<Stream>();
            Dictionary<string, string> meta = new Dictionary<string, string>()
            {
                { "email", email},
                { "fileLink", fileLink}
            };

            bool funcResult = true;
            try
            {
                funcResult = await emailNotificationFunction.Run(stream, name, meta);
            }
            catch (Exception ex)
            {
                funcResult = false;
            }
            finally
            {
                Assert.True(funcResult);
            }
        }
    }
}
