using BETeCommerce.BunessEntities.DataEntities;
using System.Collections.Generic;
using System.Net;

namespace BETeCommerce.BunessEntities.Responses
{
    public class CategoriesResponse : ApiResponse
    {
        public CategoriesResponse(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public IList<ProductCategoryDto> Categories { get; set; }
        public int Count { get; set; }
    }
}
