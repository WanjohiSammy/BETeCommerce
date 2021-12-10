using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BETeCommerce.DataLayer.Models
{
    public class Category : DbEntity
    {
        public string Name { get; set; }
        //public string ParentCategory { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public Category()
        {
            Products = new HashSet<Product>();
        }
    }
}
