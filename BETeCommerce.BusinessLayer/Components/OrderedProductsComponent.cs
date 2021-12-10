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
    public class OrderedProductsComponent : IOrderedProductsComponent
    {
        private readonly ILogger<OrderedProductsComponent> _logger;
        private readonly IOrderedProductsRepository _orderedProductRepository;
        private readonly IUserIdentityService _userIdentityService;

        public OrderedProductsComponent(IOrderedProductsRepository orderedProductRepository, IUserIdentityService userIdentityService, ILogger<OrderedProductsComponent> logger)
        {
            _orderedProductRepository = orderedProductRepository;
            _logger = logger;
            _userIdentityService = userIdentityService;
        }

        public ProductsOrderedResponse GetOrdersMade(PaginationQueryParams queryParams)
        {
            var response = new ProductsOrderedResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var result = _orderedProductRepository.GetOrdersMade(queryParams);
                response.Orders = result.DataEntity;
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

        public BuyersOrdersResponse GetBuyersOrders(PaginationQueryParams queryParams)
        {
            var response = new BuyersOrdersResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var result = _orderedProductRepository.GetBuyersOrders(queryParams);
                response.Orders = result.DataEntity;
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

        public OrderDetailsResponse GetOrdersByProduct(Guid productId)
        {
            var response = new OrderDetailsResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var result = _orderedProductRepository.GetOrdersByProduct(productId);
                response.Order = result ?? new OrderedProductEntityDto();
                response.Message = CustomMessages.Success;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod()?.Name;
                response.Message = $"{CustomMessages.Error}: {ex.Message}";
                LoggingExceptions(methodName, new { productId }, ex.ToString());
            }

            return response;
        }

        public OrderDetailsResponse MakeNewOrder(MakeOrderRequest request)
        {
            var response = new OrderDetailsResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var result = _orderedProductRepository.MakeOrder(request, _userIdentityService.GetUserId());
                response.Order = result ?? new OrderedProductEntityDto();
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

        public OrderDetailsResponse UpdateOrder(UpdateOrderRequest request)
        {
            var response = new OrderDetailsResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var result = _orderedProductRepository.UpdateOrder(request, _userIdentityService.GetUserId());
                response.Order = result ?? new OrderedProductEntityDto();
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

        public OrderDetailsResponse DeleteOrder(DeleteItemRequest request)
        {
            var response = new OrderDetailsResponse(HttpStatusCode.InternalServerError)
            {
                CurrentUser = _userIdentityService.GetEmail()
            };

            try
            {
                var result = _orderedProductRepository.DeleteOrder(request, _userIdentityService.GetUserId());
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
