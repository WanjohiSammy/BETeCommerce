using BETeCommerce.BunessEntities.DataEntities;
using BETeCommerce.BunessEntities.Requests;
using BETeCommerce.BunessEntities.Responses;
using BETeCommerce.BusinessLayer.Services;
using BETeCommerce.DataLayer.Repositories;
using BETeCommerce.UtilityLayer.Constants;
using BETeCommerce.UtilityLayer.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;

namespace BETeCommerce.BusinessLayer.Components
{
    public class ProductsComponent : IProductsComponent
    {
        private readonly ILogger<ProductsComponent> _logger;
        private readonly IProductsRepository _productRepository;
        private readonly IUserIdentityService _userIdentityService;

        public ProductsComponent(IProductsRepository productRepository, IUserIdentityService userIdentityService, ILogger<ProductsComponent> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
            _userIdentityService = userIdentityService;
        }

        public ProductsResponse GetProducts(PaginationQueryParams queryParams)
        {
            var response = new ProductsResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var result = _productRepository.GetProducts(queryParams);
                response.Products = result.DataEntity;
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

        public ProductDetailsResponse GetProductDetails(Guid id)
        {
            var response = new ProductDetailsResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var result = _productRepository.GetSingleProduct(id);
                response.Product = result ?? new ProductDto();
                response.Message = CustomMessages.Success;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                response.Message = $"{CustomMessages.Error}: {ex.Message}";
                LoggingExceptions(methodName, new { productId = id }, ex.ToString());
            }

            return response;
        }

        public ProductDetailsResponse AddProduct(AddProductRequest request)
        {
            var response = new ProductDetailsResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                // Save Product first
                var imageUrl = SaveOrUpdateImageInResources(request.ProductImage);
                request.ImageUrl = imageUrl;
                var result = _productRepository.AddProduct(request, _userIdentityService.GetUserId());
                response.Product = result ?? new ProductDto();
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

        public ProductDetailsResponse UpdateProduct(UpdateProductRequest request)
        {
            var response = new ProductDetailsResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var result = _productRepository.UpdateProduct(request, _userIdentityService.GetUserId());
                response.Product = result ?? new ProductDto();
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

        public ProductDetailsResponse UpdateProductImage(UpdateProductImageRequest request)
        {
            var response = new ProductDetailsResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var newImageUrl = SaveOrUpdateImageInResources(request.ProductImage);
                if(newImageUrl != null)
                {
                    var result = _productRepository.UpdateProduct(new UpdateProductRequest
                    {
                        Id = request.Id,
                        ImageUrl = newImageUrl
                    }, _userIdentityService.GetUserId());

                    if(result != null)
                    {
                        // Delete the old image
                        DeleteOldImageAfterSuccessUpdate(request.ImageUrl);
                    }

                    response.Product = result ?? new ProductDto();
                    response.Message = CustomMessages.Success;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = CustomMessages.UnableImageUpdate;
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                response.Message = $"{CustomMessages.Error}: {ex.Message}";
                LoggingExceptions(methodName, request, ex.ToString());
            }

            return response;
        }

        public ProductDetailsResponse DeleteProduct(DeleteItemRequest request)
        {
            var response = new ProductDetailsResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var result = _productRepository.DeleteProduct(request, _userIdentityService.GetUserId());
                response.Product = result == 1 ? new ProductDto { Id = request.Id } : new ProductDto();
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

        private string SaveOrUpdateImageInResources(IFormFile file)
        {
            var folderName = UtilHelper.GetResourceImagesFolderName;
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            string dbFilePath = null;
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fileExtension = Path.GetExtension(Path.Combine(pathToSave, fileName));

                // Generate a new name for the file
                var newFileName = $"{Guid.NewGuid()}{fileExtension}";
                var fullPath = Path.Combine(pathToSave, newFileName);
                dbFilePath = Path.Combine(folderName, newFileName);

                using (var stream = File.Create(fullPath))
                {
                    file.CopyTo(stream);
                }
            }

            return dbFilePath;
        }

        private void DeleteOldImageAfterSuccessUpdate(string imageUrl)
        {
            var arrImageUrl = imageUrl.Split("\\");
            var fileName = arrImageUrl[arrImageUrl.Length - 1];
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), UtilHelper.GetResourceImagesFolderName);
            var fullPath = Path.Combine(pathToSave, fileName);

            // Delete file
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        #endregion
    }
}
