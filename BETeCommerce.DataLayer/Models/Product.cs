using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BETeCommerce.DataLayer.Models
{
    public class Product : DbEntity
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public string SellerId { get; set; }
        public double Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser Seller { get; set; }
        public virtual ICollection<OrderedProduct> OrderedProducts { get; set; }

        public Product()
        {
            OrderedProducts = new HashSet<OrderedProduct>();
        }
    }
}
