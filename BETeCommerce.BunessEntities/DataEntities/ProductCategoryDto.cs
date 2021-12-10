using System;
using System.Collections.Generic;

namespace BETeCommerce.BunessEntities.DataEntities
{
    public class ProductCategoryDto : ClientCommonDto
    {
        public string Name { get; set; }
        public IList<ProductDto> Products { get; set; }
    }
}
