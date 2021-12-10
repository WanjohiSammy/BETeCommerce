using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BETeCommerce.BunessEntities.Responses
{
    public static class CustomActionResult
    {
        public static ActionResult CreateResponse<T>(HttpStatusCode statusCode, T response)
        {
            return new ResponseMessageResult(new HttpResponseMessage(statusCode))
            {
                StatusCode = (int)statusCode,
                Value = response
            };
        }
    }
}
