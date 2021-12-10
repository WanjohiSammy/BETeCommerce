using System;
using System.Text.Json.Serialization;

namespace BETeCommerce.BunessEntities.DataEntities
{
    public class UserDetailsDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        [JsonIgnore]
        public string Message { get; set; }
        [JsonIgnore]
        public bool IsSucceeded { get; set; }
    }
}
