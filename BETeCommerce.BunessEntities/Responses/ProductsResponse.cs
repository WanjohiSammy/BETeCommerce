using BETeCommerce.BunessEntities.DataEntities;
using System.Collections.Generic;
using System.Net;

namespace BETeCommerce.BunessEntities.Responses
{
    public class ProductsResponse : ApiResponse
    {
        public ProductsResponse(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public IList<ProductDto> Products { get; set; }
        public int Count { get; set; }
    }
}
