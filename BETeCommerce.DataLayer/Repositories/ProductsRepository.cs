using BETeCommerce.BunessEntities.DataEntities;
using BETeCommerce.BunessEntities.Requests;
using BETeCommerce.DataLayer.Data;
using BETeCommerce.DataLayer.Models;
using BETeCommerce.UtilityLayer.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BETeCommerce.DataLayer.Repositories
{
    public class ProductsRepository : IProductsRepository, IDisposable
    {
        private readonly ApplicationDBContext _context;
        private readonly DbSet<Product> _productDbSet;
        private readonly DbSet<Category> _categoryDbSet;

        public ProductsRepository(ApplicationDBContext context)
        {
            _context = context;
            _productDbSet = context.Set<Product>();
            _categoryDbSet = context.Set<Category>();
        }

        public DataModelsWithCountDto<ProductDto> GetProducts(PaginationQueryParams queryParams)
        {
            var query = _productDbSet
                .OrderByDescending(p => p.DateCreated)
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .AsQueryable();

            var skippedRecords = UtilHelper.CalculateSkippedRecords(queryParams.PageSize, queryParams.PageNumber);
            var result = query.Select(p => FormProductEntityObject(p))
                .Skip(skippedRecords)
                .Take(queryParams.PageSize)
                .ToList();

            return new DataModelsWithCountDto<ProductDto>
            {
                DataEntity = result,
                Count = query.Count()
            };
        }

        //public IList<ProductEntity> GetProductsOrdered()
        //{
        //    var query = _productDbSet
        //        .Include(p => p.Category)
        //        .Include(p => p.Seller)
        //        .Include(p => p.OrderedProducts)
        //        .AsQueryable();

        //    return query.Select(p => FormProductEntityObject(p)).ToList();
        //}

        public ProductDto GetSingleProduct(Guid id)
        {
            var product = _productDbSet.Find(id);
            if (product == null) return null;

            _context.Entry(product).Reference(p => p.Category).Load();
            _context.Entry(product).Reference(p => p.Seller).Load();

            return FormProductEntityObject(product);
        }

        public ProductDto AddProduct(AddProductRequest request, string currentUser)
        {
            var category = _categoryDbSet.Find(request.CategoryId);
            if (category == null) return null;

            var product = _productDbSet.Add(new Product
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                SellerId = currentUser,
                Quantity = request.Quantity,
                ImageUrl = request.ImageUrl,
                Price = request.Price,
                CreatedBy = currentUser
            });;
            category.Products.Add(product.Entity);

            CommitChanges();

            var newProductEntry = product.Entity;
            _context.Entry(newProductEntry).Reference(p => p.Seller).Load();

            return FormProductEntityObject(newProductEntry);
        }

        public ProductDto UpdateProduct(UpdateProductRequest request, string currentUser)
        {
            var product = _productDbSet.Find(request.Id);
            if (product == null) return null;

            product.Name = request.Name ?? product.Name;
            product.CategoryId = request.CategoryId ?? product.CategoryId;
            product.Quantity = request.Quantity ?? product.Quantity;
            product.ImageUrl = request.ImageUrl ?? product.ImageUrl;
            product.Name = request.Name ?? product.Name;
            product.Price = request.Price == 0 ? product.Price : request.Price;
            product.LastModifiedBy = currentUser;
            product.DateLastModified = DateTimeOffset.Now;

            _productDbSet.Update(product);
            CommitChanges();

            _context.Entry(product).Reference(p => p.Category).Load();
            _context.Entry(product).Reference(p => p.Seller).Load();

            return FormProductEntityObject(product);
        }

        public int DeleteProduct(DeleteItemRequest request, string currentUser)
        {
            var productToDelete = _productDbSet.Find(request.Id);
            if (productToDelete == null) return 0;

            // Cant delete delete Product with active orders
            _context.Entry(productToDelete).Collection(p => p.OrderedProducts).Load();
            if (productToDelete.OrderedProducts != null && productToDelete.OrderedProducts.Count > 0) return 0;

            productToDelete.IsDeleted = true;
            productToDelete.LastModifiedBy = currentUser;
            productToDelete.DateLastModified = DateTimeOffset.Now;

            _productDbSet.Update(productToDelete);
            CommitChanges();

            return 1;
        }

        #region Private
        private static ProductDto FormProductEntityObject(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                SellerId = product.SellerId,
                SellerEmailAddress = product.Seller.Email,
                Quantity = product.Quantity,
                ImageUrl = product.ImageUrl,
                Status = (bool)product.Status,
                CreatedBy = product.CreatedBy,
                DateCreated = product.DateCreated
            };
        }
        private void CommitChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private bool _disposeValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposeValue)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                    }
                }

                _disposeValue = true;
            }
        }
        #endregion
    }
}
