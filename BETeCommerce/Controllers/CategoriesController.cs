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
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoriesComponent _categoriesComponent;
        private readonly IUserIdentityService _userIdentityService;

        public CategoriesController(ICategoriesComponent categoriesComponent, IUserIdentityService userIdentityService, ILogger<CategoriesController> logger)
        {
            _categoriesComponent = categoriesComponent;
            _logger = logger;
            _userIdentityService = userIdentityService;
        }

        /// <summary>
        /// Get Categories
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns>CategoriesResponse</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<CategoriesResponse> GetCategories([FromQuery] PaginationQueryParams queryParams)
        {
            var response = _categoriesComponent.GetCategories(queryParams);
            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        /// <summary>
        /// Get all Categores
        /// </summary>
        /// <returns>CategoriesResponse</returns>
        [AllowAnonymous]
        [HttpGet("all")]
        public ActionResult<CategoriesResponse> GetAllCategories()
        {
            var response = _categoriesComponent.GetAllCategories();
            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        /// <summary>
        /// Get Products by Category
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns>CategoriesResponse</returns>
        [AllowAnonymous]
        [HttpGet("Products/{id}")]
        public ActionResult<CategoriesResponse> GetProductsByCategory(Guid id, [FromQuery] PaginationQueryParams queryParams)
        {
            var response = _categoriesComponent.GetProductsByCategories(id, queryParams);
            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        /// <summary>
        /// Get Single Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CategoryDetailsResponse</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<CategoryDetailsResponse> GetCategory(Guid id)
        {
            var response = _categoriesComponent.GetCategoryDetails(id);
            return CustomActionResult.CreateResponse(response.StatusCode, response);
        }

        /// <summary>
        /// Add new Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns>CategoryDetailsResponse</returns>
        [HttpPost]
        public ActionResult<CategoryDetailsResponse> PostCategory([FromBody] AddCategoryRequest request)
        {
            CategoryDetailsResponse response;
            if(ModelState.IsValid)
            {
                response = _categoriesComponent.AddCategory(request);
            }
            else
            {
                var errors = ModelState
                    .Select(x => x.Value.Errors[0].ErrorMessage)
                    .ToList();

                response = new CategoryDetailsResponse(HttpStatusCode.BadRequest)
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
        /// Update Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns>CategoryDetailsResponse</returns>
        [HttpPut]
        public ActionResult<CategoryDetailsResponse> PutCategory([FromBody] UpdateCategoryRequest request)
        {
            CategoryDetailsResponse response;
            if (ModelState.IsValid)
            {
                response = _categoriesComponent.UpdateCategory(request);
            }
            else
            {
                var errors = ModelState
                    .Select(x => x.Value.Errors[0].ErrorMessage)
                    .ToList();

                response = new CategoryDetailsResponse(HttpStatusCode.BadRequest)
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
        /// Delete Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns>CategoryDetailsResponse</returns>
        [HttpDelete]
        public ActionResult<CategoryDetailsResponse> DeleteCategory([FromBody] DeleteItemRequest request)
        {
            CategoryDetailsResponse response;
            if (ModelState.IsValid)
            {
                response = _categoriesComponent.DeleteCategory(request);
            }
            else
            {
                var errors = ModelState
                    .Select(x => x.Value.Errors[0].ErrorMessage)
                    .ToList();

                response = new CategoryDetailsResponse(HttpStatusCode.BadRequest)
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
