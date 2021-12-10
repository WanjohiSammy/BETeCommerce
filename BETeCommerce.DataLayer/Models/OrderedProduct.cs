using System;

namespace BETeCommerce.DataLayer.Models
{
    public class OrderedProduct : DbEntity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
