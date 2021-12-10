using BETeCommerce.BunessEntities.DataEntities;
using System.Net;

namespace BETeCommerce.BunessEntities.Responses
{
    public class OrderDetailsResponse : ApiResponse
    {
        public OrderDetailsResponse(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public OrderedProductEntityDto Order { get; set; }
    }
}
