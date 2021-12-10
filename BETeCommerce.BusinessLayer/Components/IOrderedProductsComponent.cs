using BETeCommerce.BunessEntities.Requests;
using BETeCommerce.BunessEntities.Responses;
using System;

namespace BETeCommerce.BusinessLayer.Components
{
    public interface IOrderedProductsComponent
    {
        ProductsOrderedResponse GetOrdersMade(PaginationQueryParams queryParams);
        BuyersOrdersResponse GetBuyersOrders(PaginationQueryParams queryParams);
        OrderDetailsResponse GetOrdersByProduct(Guid productId);
        OrderDetailsResponse MakeNewOrder(MakeOrderRequest request);
        OrderDetailsResponse UpdateOrder(UpdateOrderRequest request);
        OrderDetailsResponse DeleteOrder(DeleteItemRequest request);
    }
}
