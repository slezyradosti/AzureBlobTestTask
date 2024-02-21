namespace Application.Core
{
    public record SmtpSecutiry
    {
        public string SMTPServer { get; init; }
        public int Port { get; init; }
        public string Login { get; init; }
        public string Password { get; init; }
    }
}
