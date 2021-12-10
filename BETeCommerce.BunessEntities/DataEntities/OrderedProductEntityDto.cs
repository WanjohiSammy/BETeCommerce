
using System.Collections.Generic;

namespace BETeCommerce.BunessEntities.DataEntities
{
    public class OrderedProductEntityDto
    {
        public ProductDto Product { get; set; }
        public IList<OrderMadeDto> Orders { get; set; }
    }
}
