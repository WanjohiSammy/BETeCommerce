using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BETeCommerce.BunessEntities.Requests
{
    [DataContract]
    public class MakeOrderRequest
    {
        [DataMember(Name = "billAmount", IsRequired = true)]
        [Range(1.0, Double.MaxValue)]
        public double BillAmount { get; set; }

        [DataMember(Name = "productId", IsRequired = true)]
        public Guid ProductId { get; set; }
    }

    public class UpdateOrderRequest
    {
        [DataMember(Name = "orderId", IsRequired = true)]
        public Guid OrderId { get; set; }

        [DataMember(Name = "productId", IsRequired = true)]
        public Guid ProductId { get; set; }

        [DataMember(Name = "billAmount")]
        [Range(1.0, Double.MaxValue)]
        public double? BillAmount { get; set; }

        [DataMember(Name = "orderStatus")]
        public byte? OrderStatus { get; set; }
    }
}
