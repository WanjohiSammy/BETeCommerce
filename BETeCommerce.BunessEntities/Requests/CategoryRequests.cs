using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BETeCommerce.BunessEntities.Requests
{
    [DataContract]
    public class AddCategoryRequest
    {
        [DataMember(Name = "name", IsRequired = true)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
    }

    [DataContract]
    public class UpdateCategoryRequest
    {
        [DataMember(Name = "id", IsRequired = true)]
        public Guid Id { get; set; }

        public AddCategoryRequest UpdateDetails { get; set; }
    }
}
