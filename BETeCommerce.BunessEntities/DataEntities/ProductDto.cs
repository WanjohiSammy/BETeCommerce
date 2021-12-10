using System;

namespace BETeCommerce.BunessEntities.DataEntities
{
    public class ProductDto : ClientCommonDto
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SellerId { get; set; }
        public string SellerEmailAddress { get; set; }
        public double Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
