using System;
using System.Runtime.Serialization;

namespace BETeCommerce.BunessEntities.Requests
{
    [DataContract]
    public class DeleteItemRequest
    {
        [DataMember(Name = "id", IsRequired = true)]
        public Guid Id { get; set; }
    }
}
