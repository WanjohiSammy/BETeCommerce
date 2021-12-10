using BETeCommerce.BunessEntities.DataEntities;
using System.Collections.Generic;
using System.Net;

namespace BETeCommerce.BunessEntities.Responses
{
    public class ProductsOrderedResponse : ApiResponse
    {
        public ProductsOrderedResponse(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public IList<OrderedProductEntityDto> Orders { get; set; }
        public int Count { get; set; }
    }
}
