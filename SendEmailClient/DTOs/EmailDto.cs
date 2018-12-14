namespace SendEmailClient
{
    public class EmailDto
    {
        public EmailBodyDto EmailBody { get; set; }

        public string SendTo { get; set; }

        public string SentFrom { get; set; }
    }
}