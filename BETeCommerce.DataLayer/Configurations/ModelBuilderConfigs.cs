using BETeCommerce.DataLayer.Models;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace BETeCommerce.DataLayer.Configurations
{
    public static class ModelBuilderConfigs
    {
        public static void SetDefaultValues(this ModelBuilder builder)
        {
            builder.Entity<Category>()
               .Property(b => b.Status).HasDefaultValue(true);
            builder.Entity<Category>()
                .Property(b => b.IsDeleted).HasDefaultValue(false);
            builder.Entity<Category>()
                .Property(b => b.DateCreated).HasDefaultValue(DateTimeOffset.Now);

            builder.Entity<Order>()
                .Property(b => b.Status).HasDefaultValue(true);
            builder.Entity<Order>()
                .Property(b => b.IsDeleted).HasDefaultValue(false);
            builder.Entity<Order>()
                .Property(b => b.DateCreated).HasDefaultValue(DateTimeOffset.Now);

            builder.Entity<OrderedProduct>()
                .Property(b => b.Status).HasDefaultValue(true);
            builder.Entity<OrderedProduct>()
                .Property(b => b.IsDeleted).HasDefaultValue(false);
            builder.Entity<OrderedProduct>()
                .Property(b => b.DateCreated).HasDefaultValue(DateTimeOffset.Now);

            builder.Entity<Product>()
                .Property(b => b.Status).HasDefaultValue(true);
            builder.Entity<Product>()
                .Property(b => b.IsDeleted).HasDefaultValue(false);
            builder.Entity<Product>()
                .Property(b => b.DateCreated).HasDefaultValue(DateTimeOffset.Now);
        }

        public static void SetKeyLessEntities(this ModelBuilder builder)
        {
            builder.Entity<DeviceFlowCodes>().HasNoKey();
            builder.Entity<PersistedGrant>().HasNoKey();
            builder.Entity<IdentityUserLogin<string>>().HasNoKey();
            builder.Entity<IdentityUserRole<string>>().HasNoKey();
            builder.Entity<IdentityUserToken<string>>().HasNoKey();
        }

        public static void AddGlobalQueryFilters(this ModelBuilder builder)
        {
            builder.Entity<Category>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Order>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<OrderedProduct>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
