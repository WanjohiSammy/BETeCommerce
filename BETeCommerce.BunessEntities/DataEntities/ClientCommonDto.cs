using System;

namespace BETeCommerce.BunessEntities.DataEntities
{
    public class ClientCommonDto
    {
        public Guid Id { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}
