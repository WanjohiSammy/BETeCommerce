using BETeCommerce.BunessEntities.Requests;
using BETeCommerce.BunessEntities.Responses;
using BETeCommerce.BusinessLayer.Components;
using BETeCommerce.UtilityLayer.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace BETeCommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersComponent _usersComponent;

        public UsersController(IUsersComponent usersComponent, ILogger<UsersController> logger)
        {
            _usersComponent = usersComponent;
            _logger = logger;
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("RegisterUser")]
        public ActionResult<UserDetailsResponse> RegisterUser([FromBody] AddOrUdateUserRequest request)
        {
            UserDetailsResponse response;
            if (ModelState.IsValid)
            {
                var securedPassword = new NetworkCredential(request.Email, request.Password).SecurePassword;
                request.Password = string.Empty;
                request.ConfirmPassword = string.Empty;
                response = _usersComponent.RegisterUser(request, securedPassword).Result;
            }
            else
            {
                var errors = ModelState
                    .Select(x => x.Value.Errors[0].ErrorMessage)
                    .ToList();

                response = new UserDetailsResponse(HttpStatusCode.BadRequest)
                {
                    Message = string.Format(CustomMessages.InvalidRequestMessage, string.Join(" ", errors)),
                    CurrentUser = request.Email
                };

                // Logs request
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                LoggingInvalidRequest(methodName, new { request.Email, request.FullName }, response.Message);
            }

            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        /// <summary>
        /// Login a User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public ActionResult<UserDetailsResponse> LoginUser([FromBody] LoginUserRequest request)
        {
            UserDetailsResponse response;
            if (ModelState.IsValid)
            {
                var securedPassword = new NetworkCredential(request.Email, request.Password).SecurePassword;
                request.Password = string.Empty;
                response = _usersComponent.LoginUser(request, securedPassword).Result;
            }
            else
            {
                var errors = ModelState
                    .Select(x => x.Value.Errors[0].ErrorMessage)
                    .ToList();

                response = new UserDetailsResponse(HttpStatusCode.BadRequest)
                {
                    Message = string.Format(CustomMessages.InvalidRequestMessage, string.Join(" ", errors)),
                    CurrentUser = request.Email
                };

                // Logs request
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                LoggingInvalidRequest(methodName, new { request.Email }, response.Message);
            }

            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        #region Private
        private void LoggingInvalidRequest<T>(string methodName, T request, string message)
        {
            var req = JsonSerializer.Serialize(request);
            _logger.LogError(CustomMessages.InvalidRequestLoggingError, methodName, req, message);
        }
        #endregion
    }
}
