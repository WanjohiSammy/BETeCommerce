using BETeCommerce.BunessEntities.DataEntities;
using BETeCommerce.BunessEntities.Requests;
using System;

namespace BETeCommerce.DataLayer.Repositories
{
    public interface ICategoriesRepository
    {
        DataModelsWithCountDto<ProductCategoryDto> GetCategories(PaginationQueryParams queryParams);
        DataModelsWithCountDto<ProductCategoryDto> GetAllCategories();
        DataModelsWithCountDto<ProductCategoryDto> GetProductsByCategories(Guid id, PaginationQueryParams queryParams);
        ProductCategoryDto QueryCategoryDetails(Guid id);
        ProductCategoryDto AddCategory(AddCategoryRequest request, string currentUser);
        ProductCategoryDto UpdateCategory(UpdateCategoryRequest request, string currentUser);
        int DeleteCategory(DeleteItemRequest request, string currentUser);
    }
}
