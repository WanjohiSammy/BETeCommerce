using BETeCommerce.BunessEntities.Requests;
using BETeCommerce.BunessEntities.Responses;
using BETeCommerce.BusinessLayer.Components;
using BETeCommerce.BusinessLayer.Services;
using BETeCommerce.UtilityLayer.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace BETeCommerce.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderedProductsComponent _orderedProductComponent;
        private readonly IEmailComponent _emailComponent;
        private readonly IUserIdentityService _userIdentityService;

        public OrdersController(IOrderedProductsComponent orderedProductComponent, IEmailComponent emailComponent, IUserIdentityService userIdentityService, ILogger<OrdersController> logger)
        {
            _orderedProductComponent = orderedProductComponent;
            _logger = logger;
            _userIdentityService = userIdentityService;
            _emailComponent = emailComponent;
        }

        /// <summary>
        /// Get Orders made
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns>ProductsOrderedResponse</returns>
        [HttpGet]
        public ActionResult<ProductsOrderedResponse> GetOrders([FromQuery]PaginationQueryParams queryParams)
        {
            var response = _orderedProductComponent.GetOrdersMade(queryParams);
            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        /// <summary>
        /// Get Orders by Buyers
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        [HttpGet("Buyers")]
        public ActionResult<ProductsOrderedResponse> GetBuyersOrders([FromQuery] PaginationQueryParams queryParams)
        {
            var response = _orderedProductComponent.GetBuyersOrders(queryParams);
            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        /// <summary>
        /// Get Single product orders
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>OrderDetailsResponse</returns>
        [HttpGet("{productId}")]
        public ActionResult<OrderDetailsResponse> GetOrderForProduct(Guid productId)
        {
            var response = _orderedProductComponent.GetOrdersByProduct(productId);
            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        /// <summary>
        /// Make a new order
        /// </summary>
        /// <param name="request"></param>
        /// <returns>OrderDetailsResponse</returns>
        [HttpPost]
        public ActionResult<OrderDetailsResponse> MakeNewOrder([FromBody] MakeOrderRequest request)
        {
            OrderDetailsResponse response;
            if (ModelState.IsValid)
            {
                response = _orderedProductComponent.MakeNewOrder(request);
            }
            else
            {
                var errors = ModelState
                   .Select(x => x.Value.Errors[0].ErrorMessage)
                   .ToList();

                response = new OrderDetailsResponse(HttpStatusCode.BadRequest)
                {
                    Message = string.Format(CustomMessages.InvalidRequestMessage, string.Join(" ", errors))
                };

                // Logs request
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                LoggingInvalidRequest(methodName, request, response.Message);
            }

            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        /// <summary>
        /// CHeckout and Send Email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Checkout")]
        public ActionResult<CheckoutResponse> Checkout([FromBody] CheckoutItemsRequest request)
        {
            CheckoutResponse response;
            if (ModelState.IsValid)
            {
                response = _emailComponent.SendEmail(request);
            }
            else
            {
                var errors = ModelState
                   .Select(x => x.Value.Errors[0].ErrorMessage)
                   .ToList();

                response = new CheckoutResponse(HttpStatusCode.BadRequest)
                {
                    Message = string.Format(CustomMessages.InvalidRequestMessage, string.Join(" ", errors))
                };

                // Logs request
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                LoggingInvalidRequest(methodName, request, response.Message);
            }

            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="request"></param>
        /// <returns>OrderDetailsResponse</returns>
        [HttpPut]
        public ActionResult<OrderDetailsResponse> UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            OrderDetailsResponse response;
            if (ModelState.IsValid)
            {
                response = _orderedProductComponent.UpdateOrder(request);
            }
            else
            {
                var errors = ModelState
                   .Select(x => x.Value.Errors[0].ErrorMessage)
                   .ToList();

                response = new OrderDetailsResponse(HttpStatusCode.BadRequest)
                {
                    Message = string.Format(CustomMessages.InvalidRequestMessage, string.Join(" ", errors))
                };

                // Logs request
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                LoggingInvalidRequest(methodName, request, response.Message);
            }

            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        /// <summary>
        /// Delete an Order
        /// </summary>
        /// <param name="request"></param>
        /// <returns>OrderDetailsResponse</returns>
        [HttpDelete]
        public ActionResult<OrderDetailsResponse> DeleteOrder([FromBody] DeleteItemRequest request)
        {
            OrderDetailsResponse response;
            if (ModelState.IsValid)
            {
                response = _orderedProductComponent.DeleteOrder(request);
            }
            else
            {
                var errors = ModelState
                   .Select(x => x.Value.Errors[0].ErrorMessage)
                   .ToList();

                response = new OrderDetailsResponse(HttpStatusCode.BadRequest)
                {
                    Message = string.Format(CustomMessages.InvalidRequestMessage, string.Join(" ", errors))
                };

                // Logs request
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                LoggingInvalidRequest(methodName, request, response.Message);
            }

            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        #region Private
        private void LoggingInvalidRequest<T>(string methodName, T request, string message)
        {
            var currentUser = new { UserName = _userIdentityService.GetUserName(), Email = _userIdentityService.GetEmail() };
            var req = JsonSerializer.Serialize(new { currentUser, request });
            _logger.LogError(CustomMessages.InvalidRequestLoggingError, methodName, req, message);
        }
        #endregion
    }
}
