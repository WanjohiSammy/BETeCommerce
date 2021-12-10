using BETeCommerce.BunessEntities.Requests;
using BETeCommerce.BunessEntities.Responses;
using BETeCommerce.DataLayer.Repositories;
using BETeCommerce.UtilityLayer.Constants;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Reflection;
using System.Security;
using System.Text.Json;
using System.Threading.Tasks;

namespace BETeCommerce.BusinessLayer.Components
{
    public class UsersComponent : IUsersComponent
    {
        private readonly ILogger<UsersComponent> _logger;
        private readonly IUsersRepository _usersRespository;

        public UsersComponent(IUsersRepository usersRespository, ILogger<UsersComponent> logger)
        {
            _usersRespository = usersRespository;
            _logger = logger;
        }

        public async Task<UserDetailsResponse> RegisterUser(AddOrUdateUserRequest request, SecureString securedPassword)
        {
            var response = new UserDetailsResponse(HttpStatusCode.InternalServerError) { CurrentUser = request.Email };
            try
            {
                var result = await _usersRespository.RegisterUser(request, securedPassword);
                if(result.IsSucceeded)
                {
                    response.UserDetails = result;
                    response.Message = CustomMessages.Success;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = $"{CustomMessages.Error}: {result.Message}";
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch(Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                response.Message = $"{CustomMessages.Error}: {ex.Message}";
                LoggingExceptions(methodName, new { request.Email, request.FullName }, ex.ToString());
            }

            return response;
        }

        public async Task<UserDetailsResponse> LoginUser(LoginUserRequest request, SecureString securedPassword)
        {
            var response = new UserDetailsResponse(HttpStatusCode.InternalServerError) { CurrentUser = request.Email };
            try
            {
                var result = await _usersRespository.LoginUser(request, securedPassword);
                if (result.IsSucceeded)
                {
                    response.UserDetails = result;
                    response.Message = CustomMessages.Success;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = $"{CustomMessages.Error}: {CustomMessages.InvalidLoginCred}";
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                response.Message = $"{CustomMessages.Error}: {ex.Message}";
                LoggingExceptions(methodName, new { request.Email }, ex.ToString());
            }

            return response;
        }

        #region Private
        private void LoggingExceptions<T>(string methodName, T request, string message)
        {
            var req = JsonSerializer.Serialize(request);
            _logger.LogError(CustomMessages.ExceptionLoggingError, methodName, req, message);
        }
        #endregion
    }
}
