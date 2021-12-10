using BETeCommerce.BunessEntities.DataEntities;
using BETeCommerce.BunessEntities.Requests;
using System;

namespace BETeCommerce.DataLayer.Repositories
{
    public interface IProductsRepository
    {
        DataModelsWithCountDto<ProductDto> GetProducts(PaginationQueryParams queryParams);
        //IList<ProductEntity> GetProductsOrdered();
        ProductDto GetSingleProduct(Guid id);
        ProductDto AddProduct(AddProductRequest request, string currentUser);
        ProductDto UpdateProduct(UpdateProductRequest request, string currentUser);
        int DeleteProduct(DeleteItemRequest request, string currentUser);
    }
}
