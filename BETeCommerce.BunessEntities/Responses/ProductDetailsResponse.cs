using BETeCommerce.BunessEntities.DataEntities;
using System.Net;

namespace BETeCommerce.BunessEntities.Responses
{
    public class ProductDetailsResponse : ApiResponse
    {
        public ProductDetailsResponse(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public ProductDto Product { get; set; }
    }
}
