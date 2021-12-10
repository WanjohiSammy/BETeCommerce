using BETeCommerce.DataLayer.Configurations;
using BETeCommerce.DataLayer.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BETeCommerce.DataLayer.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
            
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<OrderedProduct> OrderedProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Keyless Entities
            builder.SetKeyLessEntities();

            // Set Default Values
            builder.SetDefaultValues();

            // Add global query filters
            builder.AddGlobalQueryFilters();
        }
    }
}
