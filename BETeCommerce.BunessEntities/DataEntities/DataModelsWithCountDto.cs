
using System.Collections.Generic;

namespace BETeCommerce.BunessEntities.DataEntities
{
    public class DataModelsWithCountDto<T> 
    {
        public IList<T> DataEntity { get; set; }
        public int Count { get; set; }
    }
}
