using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BETeCommerce.BunessEntities.Requests
{
    [DataContract]
    public class AddProductRequest
    {
        [DataMember(Name = "name", IsRequired = true)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [DataMember(Name = "quantity", IsRequired = true)]
        [Range(1.0, Double.MaxValue)]
        public double Quantity { get; set; }

        [DataMember(Name = "price", IsRequired = true)]
        public decimal Price { get; set; }

        [DataMember(Name = "productImage", IsRequired = true)]
        public IFormFile ProductImage { get; set; }

        [JsonIgnore]
        public string ImageUrl { get; set; }

        [DataMember(Name = "categoryId", IsRequired = true)]
        public Guid CategoryId { get; set; }
    }

    [DataContract]
    public class UpdateProductRequest
    {
        [DataMember(Name = "id", IsRequired = true)]
        public Guid Id { get; set; }

        [DataMember(Name = "name", IsRequired = true)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [DataMember(Name = "quantity")]
        public double? Quantity { get; set; }

        [DataMember(Name = "price", IsRequired = true)]
        public decimal Price { get; set; }

        [JsonIgnore]
        public string ImageUrl { get; set; }

        [DataMember(Name = "categoryId")]
        public Guid? CategoryId { get; set; }
    }

    [DataContract]
    public class UpdateProductImageRequest
    {
        [DataMember(Name = "id", IsRequired = true)]
        public Guid Id { get; set; }

        [DataMember(Name = "productImage", IsRequired = true)]
        public IFormFile ProductImage { get; set; }

        [DataMember(Name = "imageUrl", IsRequired = true)]
        [Required(AllowEmptyStrings = false)]
        public string ImageUrl { get; set; }
    }
}
