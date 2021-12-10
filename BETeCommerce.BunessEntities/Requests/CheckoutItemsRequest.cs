using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BETeCommerce.BunessEntities.Requests
{
    [DataContract]
    public class CheckoutItemsRequest
    {
        [DataMember(Name = "buyerEmail", IsRequired = true)]
        [Required(AllowEmptyStrings = false)]
        public string BuyerEmail { get; set; }

        [DataMember(Name = "cartItems", IsRequired = true)]
        public IList<CartItemsDto> CartItems { get; set; }

        [DataMember(Name = "totalPrice", IsRequired = true)]
        public decimal TotalPrice { get; set; }
    }

    [DataContract]
    public class CartItemsDto
    {
        [DataMember(Name = "productName", IsRequired = true)]
        public string ProductName { get; set; }

        [DataMember(Name = "quantity", IsRequired = true)]
        public decimal Quantity { get; set; }

        [DataMember(Name = "price", IsRequired = true)]
        public decimal Price { get; set; }
    }
}
