
namespace BETeCommerce.UtilityLayer.Helpers
{
    public class GenerateJwtInputParamsHelper
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserFullName { get; set; }
        public string JwtKey { get; set; }
        public string JwtAudience { get; set; }
        public string JwtIssuer { get; set; }
    }
}
