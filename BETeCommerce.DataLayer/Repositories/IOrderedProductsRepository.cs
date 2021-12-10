using BETeCommerce.BunessEntities.DataEntities;
using BETeCommerce.BunessEntities.Requests;
using System;

namespace BETeCommerce.DataLayer.Repositories
{
    public interface IOrderedProductsRepository
    {
        DataModelsWithCountDto<OrderedProductEntityDto> GetOrdersMade(PaginationQueryParams queryParams);
        DataModelsWithCountDto<BuyersOrdersDto> GetBuyersOrders(PaginationQueryParams queryParams);
        OrderedProductEntityDto GetOrdersByProduct(Guid productId);
        OrderedProductEntityDto MakeOrder(MakeOrderRequest request, string currentUser);
        OrderedProductEntityDto UpdateOrder(UpdateOrderRequest request, string currentUser);
        int DeleteOrder(DeleteItemRequest request, string currentUser);
    }
}
