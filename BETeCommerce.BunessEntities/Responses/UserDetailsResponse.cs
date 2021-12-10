using BETeCommerce.BunessEntities.DataEntities;
using System.Net;

namespace BETeCommerce.BunessEntities.Responses
{
    public class UserDetailsResponse : ApiResponse
    {
        public UserDetailsResponse(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public UserDetailsDto UserDetails { get; set; }
    }
}
