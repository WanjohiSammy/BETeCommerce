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
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductsComponent _productComponent;
        private readonly IUserIdentityService _userIdentityService;

        public ProductsController(IProductsComponent productComponent, IUserIdentityService userIdentityService, ILogger<ProductsController> logger)
        {
            _productComponent = productComponent;
            _logger = logger;
            _userIdentityService = userIdentityService;
        }

        /// <summary>
        /// Get list of products
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns>ProductsResponse</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<ProductsResponse> GetProducts([FromQuery] PaginationQueryParams queryParams)
        {
            var response = _productComponent.GetProducts(queryParams);
            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        /// <summary>
        /// Get Details of a product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ProductDetailsResponse</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<ProductDetailsResponse> GetProductDetails(Guid id)
        {
            var response = _productComponent.GetProductDetails(id);
            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        /// <summary>
        /// Add product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, DisableRequestSizeLimit]
        public ActionResult<ProductDetailsResponse> PostProduct([FromForm] AddProductRequest request)
        {
            var response = new ProductDetailsResponse(HttpStatusCode.Unauthorized)
            {
                Message = HttpStatusCode.Unauthorized.ToString()
            };

            if (ModelState.IsValid)
            {
                response = _productComponent.AddProduct(request);
            }
            else
            {
                var errors = ModelState
                    .Select(x => x.Value.Errors[0].ErrorMessage)
                    .ToList();

                response = new ProductDetailsResponse(HttpStatusCode.BadRequest)
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
        /// Update product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public ActionResult<ProductDetailsResponse> PutProduct([FromBody] UpdateProductRequest request)
        {
            var response = new ProductDetailsResponse(HttpStatusCode.Unauthorized)
            {
                Message = HttpStatusCode.Unauthorized.ToString()
            };

            if (ModelState.IsValid)
            {
                response = _productComponent.UpdateProduct(request);
            }
            else
            {
                var errors = ModelState
                    .Select(x => x.Value.Errors[0].ErrorMessage)
                    .ToList();

                response = new ProductDetailsResponse(HttpStatusCode.BadRequest)
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
        /// Update product image
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateImage")]
        public ActionResult<ProductDetailsResponse> UpdateProductImage([FromForm] UpdateProductImageRequest request)
        {
            var response = new ProductDetailsResponse(HttpStatusCode.Unauthorized)
            {
                Message = HttpStatusCode.Unauthorized.ToString()
            };

            if (ModelState.IsValid)
            {
                response = _productComponent.UpdateProductImage(request);
            }
            else
            {
                var errors = ModelState
                    .Select(x => x.Value.Errors[0].ErrorMessage)
                    .ToList();

                response = new ProductDetailsResponse(HttpStatusCode.BadRequest)
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
        /// Delete a product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult<ProductDetailsResponse> Delete([FromBody]DeleteItemRequest request)
        {
            ProductDetailsResponse response;
            if (ModelState.IsValid)
            {
                response = _productComponent.DeleteProduct(request);
            }
            else
            {
                var errors = ModelState
                    .Select(x => x.Value.Errors[0].ErrorMessage)
                    .ToList();

                response = new ProductDetailsResponse(HttpStatusCode.BadRequest)
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
