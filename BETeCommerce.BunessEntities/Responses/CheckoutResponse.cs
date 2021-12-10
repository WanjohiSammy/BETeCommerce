using BETeCommerce.BunessEntities.DataEntities;
using System.Net;

namespace BETeCommerce.BunessEntities.Responses
{
    public class CheckoutResponse : ApiResponse
    {
        public CheckoutResponse(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public EmailDataDto EmailData { get; set; }
    }
}
