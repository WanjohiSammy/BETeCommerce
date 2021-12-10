using BETeCommerce.BunessEntities.DataEntities;
using BETeCommerce.BunessEntities.Requests;
using BETeCommerce.BunessEntities.Responses;
using BETeCommerce.BusinessLayer.Services;
using BETeCommerce.DataLayer.Repositories;
using BETeCommerce.UtilityLayer.Constants;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace BETeCommerce.BusinessLayer.Components
{
    public class CategoriesComponent : ICategoriesComponent
    {
        private readonly ILogger<CategoriesComponent> _logger;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IUserIdentityService _userIdentityService;

        public CategoriesComponent(ICategoriesRepository categoriesRepository, IUserIdentityService userIdentityService, ILogger<CategoriesComponent> logger)
        {
            _logger = logger;
            _categoriesRepository = categoriesRepository;
            _userIdentityService = userIdentityService;
        }

        public CategoriesResponse GetCategories(PaginationQueryParams queryParams)
        {
            var response = new CategoriesResponse(HttpStatusCode.InternalServerError);
            try
            {
                var result = _categoriesRepository.GetCategories(queryParams);
                response.Categories = result.DataEntity;
                response.Count = result.Count;
                response.Message = CustomMessages.Success;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch(Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                response.Message = $"{CustomMessages.Error}: {ex.Message}";
                LoggingExceptions(methodName, queryParams, ex.ToString());
            }

            return response;
        }

        public CategoriesResponse GetAllCategories()
        {
            var response = new CategoriesResponse(HttpStatusCode.InternalServerError);
            try
            {
                var result = _categoriesRepository.GetAllCategories();
                response.Categories = result.DataEntity;
                response.Count = result.Count;
                response.Message = CustomMessages.Success;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                response.Message = $"{CustomMessages.Error}: {ex.Message}";
                LoggingExceptions(methodName, new { }, ex.ToString());
            }

            return response;
        }

        public CategoriesResponse GetProductsByCategories(Guid id, PaginationQueryParams queryParams)
        {
            var response = new CategoriesResponse(HttpStatusCode.InternalServerError);

            try
            {
                var result = _categoriesRepository.GetProductsByCategories(id, queryParams);
                response.Categories = result.DataEntity;
                response.Count = result.Count;
                response.Message = CustomMessages.Success;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                response.Message = $"{CustomMessages.Error}: {ex.Message}";
                LoggingExceptions(methodName, queryParams, ex.ToString());
            }

            return response;
        }

        public CategoryDetailsResponse GetCategoryDetails(Guid id)
        {
            var response = new CategoryDetailsResponse(HttpStatusCode.InternalServerError);

            try
            {
                var result = _categoriesRepository.QueryCategoryDetails(id);
                response.Category = result ?? new ProductCategoryDto();
                response.Message = CustomMessages.Success;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                response.Message = $"{CustomMessages.Error}: {ex.Message}";
                LoggingExceptions(methodName, new { categoryId = id }, ex.ToString());
            }

            return response;
        }

        public CategoryDetailsResponse AddCategory(AddCategoryRequest request)
        {
            var response = new CategoryDetailsResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var result = _categoriesRepository.AddCategory(request, _userIdentityService.GetUserId());
                response.Category = result ?? new ProductCategoryDto();
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

        public CategoryDetailsResponse UpdateCategory(UpdateCategoryRequest request)
        {
            var response = new CategoryDetailsResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var result = _categoriesRepository.UpdateCategory(request, _userIdentityService.GetUserId());
                response.Category = result ?? new ProductCategoryDto();
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

        public CategoryDetailsResponse DeleteCategory(DeleteItemRequest request)
        {
            var response = new CategoryDetailsResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var result = _categoriesRepository.DeleteCategory(request, _userIdentityService.GetUserId());
                response.Category = new ProductCategoryDto { Id = request.Id };
                response.Message = result == 1 ? CustomMessages.Success : CustomMessages.FailedDelete;
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
