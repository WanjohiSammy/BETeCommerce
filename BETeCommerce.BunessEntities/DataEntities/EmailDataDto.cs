using System;

namespace BETeCommerce.BunessEntities.DataEntities
{
    public class EmailDataDto
    {
        public string ToAddress { get; set; }
        public string FromAddress { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
        public DateTime SentTime { get; set; }
        public bool SentStatus { get; set; }
    }
}
