using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BETeCommerce.DataLayer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool? Status { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateLastModified { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public ApplicationUser()
        {
            Orders = new HashSet<Order>();
            Products = new HashSet<Product>();
            DateCreated = DateTimeOffset.Now;
            Status = true;
            IsDeleted = false;
        }
    }
}
