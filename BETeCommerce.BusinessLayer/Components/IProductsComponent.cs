using BETeCommerce.BunessEntities.Requests;
using BETeCommerce.BunessEntities.Responses;
using System;

namespace BETeCommerce.BusinessLayer.Components
{
    public interface IProductsComponent
    {
        ProductsResponse GetProducts(PaginationQueryParams queryParams);
        //IList<ProductEntity> GetProductsOrdered();
        ProductDetailsResponse GetProductDetails(Guid id);
        ProductDetailsResponse AddProduct(AddProductRequest request);
        ProductDetailsResponse UpdateProduct(UpdateProductRequest request);
        ProductDetailsResponse UpdateProductImage(UpdateProductImageRequest request);
        ProductDetailsResponse DeleteProduct(DeleteItemRequest request);
    }
}
