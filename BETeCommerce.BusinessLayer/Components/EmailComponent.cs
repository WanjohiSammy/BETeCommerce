using BETeCommerce.BunessEntities.Requests;
using BETeCommerce.BunessEntities.Responses;
using BETeCommerce.BusinessLayer.Services;
using BETeCommerce.UtilityLayer.Constants;
using BETeCommerce.UtilityLayer.Email;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BETeCommerce.BusinessLayer.Components
{
    public class EmailComponent : IEmailComponent
    {
        private readonly ILogger<EmailComponent> _logger;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IEmailHelper _emailHelper;

        public EmailComponent(IEmailHelper emailHelper, ILogger<EmailComponent> logger, IUserIdentityService userIdentityService)
        {
            _emailHelper = emailHelper;
            _logger = logger;
            _userIdentityService = userIdentityService;
        }

        public CheckoutResponse SendEmail(CheckoutItemsRequest request)
        {
            var response = new CheckoutResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var result = _emailHelper.SendEmail(request);
                response.EmailData = result;
                response.Message = CustomMessages.Success;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                response.Message = $"{CustomMessages.Error}: {ex.Message}";
                LoggingExceptions(methodName, request, ex.ToString());
            }

            return response;
        }

        #region Private
        private void LoggingExceptions<T>(string methodName, T request, string message)
        {
            var currentUser = new { UserName = _userIdentityService.GetUserName(), Email = _userIdentityService.GetEmail() };
            var req = JsonSerializer.Serialize(new { currentUser, request });
            _logger.LogError(CustomMessages.ExceptionLoggingError, methodName, req, message);
        }
        #endregion
    }
}
