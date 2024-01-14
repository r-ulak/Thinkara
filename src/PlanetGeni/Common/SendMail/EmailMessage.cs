using System;
namespace Common.SendMail
{
    public class EmailMessage
    {
        public string Body { get; set; }
        public string Destination { get; set; }
        public string Subject { get; set; }
    }
}
