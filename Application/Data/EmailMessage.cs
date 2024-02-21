namespace Application.Data
{
    public class EmailMessage
    {
        public string? SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string? RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
