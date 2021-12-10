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
    public class CategoriesRepository : ICategoriesRepository, IDisposable
    {
        private readonly ApplicationDBContext _context;
        private readonly DbSet<Category> _categoryDbSet;

        public CategoriesRepository(ApplicationDBContext context)
        {
            _context = context;
            _categoryDbSet = context.Set<Category>();
        }

        public DataModelsWithCountDto<ProductCategoryDto> GetCategories(PaginationQueryParams queryParams)
        {
            var query = _categoryDbSet
                .OrderByDescending(c => c.DateCreated)
                .AsQueryable();

            var skippedRecords = UtilHelper.CalculateSkippedRecords(queryParams.PageSize, queryParams.PageNumber);
            var result =  query
                .Select(c => FormCategoryEntityObj(c))
                .Skip(skippedRecords)
                .Take(queryParams.PageSize)
                .ToList();

            return new DataModelsWithCountDto<ProductCategoryDto>
            {
                DataEntity = result,
                Count = query.Count()
            };
        }

        public DataModelsWithCountDto<ProductCategoryDto> GetAllCategories()
        {
            var query = _categoryDbSet
                .OrderByDescending(c => c.DateCreated)
                .AsQueryable();

            var result = query
                .Select(c => FormCategoryEntityObj(c))
                .ToList();

            return new DataModelsWithCountDto<ProductCategoryDto>
            {
                DataEntity = result,
                Count = query.Count()
            };
        }

        public DataModelsWithCountDto<ProductCategoryDto> GetProductsByCategories(Guid id, PaginationQueryParams queryParams)
        {
            var category = _categoryDbSet.Find(id);
           
            var query = _categoryDbSet
                .Include(c => c.Products)
                    .ThenInclude(p => p.Seller)
                .Where(c => c.Id == id)
                .AsQueryable();

            var skippedRecords = UtilHelper.CalculateSkippedRecords(queryParams.PageSize, queryParams.PageNumber);
            var result = query.Select(c => new ProductCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Status = (bool)c.Status,
                    CreatedBy = c.CreatedBy,
                    DateCreated = c.DateCreated,
                    Products = c.Products.Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        CategoryId = p.CategoryId,
                        SellerId = p.SellerId,
                        SellerEmailAddress = p.Seller.Email,
                        Quantity = p.Quantity,
                        Status = (bool)p.Status,
                        ImageUrl = p.ImageUrl,
                        CreatedBy = p.CreatedBy,
                        DateCreated = p.DateCreated
                    }).ToList()
                })
                .Skip(skippedRecords)
                .Take(queryParams.PageSize)
                .ToList();

            return new DataModelsWithCountDto<ProductCategoryDto>
            {
                DataEntity = result,
                Count = query.Count()
            };
        }

        public ProductCategoryDto QueryCategoryDetails(Guid id)
        {
            var category = _categoryDbSet.Find(id);
            if (category == null) return null;

            return FormCategoryEntityObj(category);
        }

        public ProductCategoryDto AddCategory(AddCategoryRequest request, string currentUser)
        {
            var category = _categoryDbSet.Add(new Category 
            {
                Name = request.Name,
                CreatedBy = currentUser
            });

            CommitChanges();

            return FormCategoryEntityObj(category.Entity);
        }

        public ProductCategoryDto UpdateCategory(UpdateCategoryRequest request, string currentUser)
        {
            var category = _categoryDbSet.Find(request.Id);
            if (category == null) return null;

            category.Name = request.UpdateDetails.Name;
            category.LastModifiedBy = currentUser;
            category.DateLastModified = DateTimeOffset.Now;

            _categoryDbSet.Update(category);
            CommitChanges();

            return FormCategoryEntityObj(category);
        }

        public int DeleteCategory(DeleteItemRequest request, string currentUser)
        {
            var catToDelete = _categoryDbSet.Find(request.Id);
            if (catToDelete == null) return 0;

            _context.Entry(catToDelete).Collection(c => c.Products).Load();
            // Cant delete category with products
            if (catToDelete.Products != null && catToDelete.Products.Count > 0) return 0;

            catToDelete.IsDeleted = true;
            catToDelete.LastModifiedBy = currentUser;
            catToDelete.DateLastModified = DateTimeOffset.Now;

            _categoryDbSet.Update(catToDelete);
            CommitChanges();

            return 1;
        }

        #region Private
        private static ProductCategoryDto FormCategoryEntityObj(Category category)
        {
            return new ProductCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Status = (bool)category.Status,
                CreatedBy = category.CreatedBy,
                DateCreated = category.DateCreated,
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
