using BETeCommerce.BunessEntities.Requests;
using BETeCommerce.BunessEntities.Responses;
using System;

namespace BETeCommerce.BusinessLayer.Components
{
    public interface ICategoriesComponent
    {
        CategoriesResponse GetCategories(PaginationQueryParams queryParams);
        CategoriesResponse GetAllCategories();
        CategoriesResponse GetProductsByCategories(Guid id, PaginationQueryParams queryParams);
        CategoryDetailsResponse GetCategoryDetails(Guid id);
        CategoryDetailsResponse AddCategory(AddCategoryRequest request);
        CategoryDetailsResponse UpdateCategory(UpdateCategoryRequest request);
        CategoryDetailsResponse DeleteCategory(DeleteItemRequest request);
    }
}
