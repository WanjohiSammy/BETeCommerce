using System.Collections.Generic;

namespace BETeCommerce.BunessEntities.DataEntities
{
    public class BuyersOrdersDto
    {
        public string BuyerId { get; set; }
        public string BuyerEmailAddress { get; set; }
        public IList<OrderMadeDto> Orders { get; set; }
    }
}
