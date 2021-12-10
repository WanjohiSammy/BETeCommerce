using BETeCommerce.BunessEntities.DataEntities;
using System.Collections.Generic;
using System.Net;

namespace BETeCommerce.BunessEntities.Responses
{
    public class BuyersOrdersResponse : ApiResponse
    {
        public BuyersOrdersResponse(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public IList<BuyersOrdersDto> Orders { get; set; }
        public int Count { get; set; }
    }
}
