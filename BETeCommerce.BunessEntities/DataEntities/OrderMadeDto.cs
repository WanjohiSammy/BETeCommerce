
namespace BETeCommerce.BunessEntities.DataEntities
{
    public class OrderMadeDto : ClientCommonDto
    {
        public double BillAmount { get; set; }
        public byte OrderStatus { get; set; }
        public string BuyerId { get; set; }
        public string BuyerEmailAddress { get; set; }
    }
}
