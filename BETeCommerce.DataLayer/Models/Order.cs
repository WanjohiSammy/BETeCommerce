using BETeCommerce.UtilityLayer.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BETeCommerce.DataLayer.Models
{
    public class Order : DbEntity
    {
        public double BillAmount { get; set; }
        public byte OrderStatus { get; set; }
        public string BuyerId { get; set; }
        public virtual ApplicationUser Buyer { get; set; }
        public virtual ICollection<OrderedProduct> OrderedProducts { get; set; }

        public Order()
        {
            OrderedProducts = new HashSet<OrderedProduct>();
            OrderStatus = (byte)OrderStatusEnum.Active;
        }
    }
}
