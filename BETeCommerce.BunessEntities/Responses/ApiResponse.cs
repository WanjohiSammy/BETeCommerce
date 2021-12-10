using System.Net;
using System.Runtime.Serialization;

namespace BETeCommerce.BunessEntities.Responses
{
    [DataContract]
    public abstract class ApiResponse
    {
        [DataMember]
        public HttpStatusCode StatusCode { get; set; }

        [DataMember]
        public string CurrentUser { get; set; }

        [DataMember]
        public string Message { get; set; }

        protected ApiResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
