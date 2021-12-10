using BETeCommerce.BunessEntities.DataEntities;
using System.Net;

namespace BETeCommerce.BunessEntities.Responses
{
    public class CategoryDetailsResponse : ApiResponse
    {
        public CategoryDetailsResponse(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public ProductCategoryDto Category { get; set; }
    }
}
