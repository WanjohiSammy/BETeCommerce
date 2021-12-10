using BETeCommerce.BunessEntities.DataEntities;
using BETeCommerce.BunessEntities.Requests;
using BETeCommerce.DataLayer.Data;
using BETeCommerce.DataLayer.Models;
using BETeCommerce.UtilityLayer.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BETeCommerce.DataLayer.Repositories
{
    public class OrderedProductsRepository : IOrderedProductsRepository, IDisposable
    {
        private readonly ApplicationDBContext _context;
        private readonly DbSet<OrderedProduct> _orderedProductsDbSet;
        private readonly DbSet<Product> _productsDbSet;
        private readonly DbSet<ApplicationUser> _applicationUserDbSet;
        private readonly DbSet<Order> _orderDbSet;

        public OrderedProductsRepository(ApplicationDBContext context)
        {
            _context = context;
            _orderedProductsDbSet = context.Set<OrderedProduct>();
            _productsDbSet = context.Set<Product>();
            _applicationUserDbSet = context.Set<ApplicationUser>();
            _orderDbSet = context.Set<Order>();
        }

        public DataModelsWithCountDto<OrderedProductEntityDto> GetOrdersMade(PaginationQueryParams queryParams)
        {
            var query = _productsDbSet
                .OrderByDescending(p => p.DateCreated)
                .Include(p => p.OrderedProducts)
                    .ThenInclude(p => p.Order)
                        .ThenInclude(p => p.Buyer)
                .AsQueryable();

            var skippedRecords = UtilHelper.CalculateSkippedRecords(queryParams.PageSize, queryParams.PageNumber);
            var result = query
                .Select(p => new OrderedProductEntityDto
                {
                    Product = FormProductEntityObject(p),
                    Orders = p.OrderedProducts.Select(o => FormOrderEntityObject(o.Order)).ToList()
                })
                .Skip(skippedRecords)
                .Take(queryParams.PageSize)
                .ToList();

            return new DataModelsWithCountDto<OrderedProductEntityDto>
            {
                DataEntity = result,
                Count = query.Count()
            };
        }

        public DataModelsWithCountDto<BuyersOrdersDto> GetBuyersOrders(PaginationQueryParams queryParams)
        {
            var query = _applicationUserDbSet
                .Include(p => p.Orders)
                .AsQueryable();

            query = query.Where(p => p.Orders.Count() > 0)
                .AsQueryable();

            var skippedRecords = UtilHelper.CalculateSkippedRecords(queryParams.PageSize, queryParams.PageNumber);
            var result = query
                .Select(b => new BuyersOrdersDto
                {
                    BuyerId = b.Id,
                    BuyerEmailAddress = b.Email,
                    Orders = b.Orders.Select(o => FormOrderEntityObject(o)).ToList()
                })
                .Skip(skippedRecords)
                .Take(queryParams.PageSize)
                .ToList();

            return new DataModelsWithCountDto<BuyersOrdersDto>
            {
                DataEntity = result,
                Count = query.Count()
            };
        }

        public OrderedProductEntityDto GetOrdersByProduct(Guid productId)
        {
            var query = _orderedProductsDbSet
                .OrderByDescending(p => p.DateCreated)
                .Where(p => p.ProductId == productId)
                .Include(p => p.Product)
                .Include(p => p.Order)
                .AsQueryable();

            var result = new OrderedProductEntityDto
            {
                Orders = new List<OrderMadeDto>()
            };

            foreach (var item in query)
            {
                if(result.Product == null)
                {
                    result.Product = FormProductEntityObject(item.Product);
                }

                result.Orders.Add(FormOrderEntityObject(item.Order));
            }

            return result;
        }

        public OrderedProductEntityDto MakeOrder(MakeOrderRequest request, string currentUser)
        {
            var addedOrder = _orderDbSet.Add(new Order
            {
                BillAmount = request.BillAmount,
                BuyerId = currentUser, 
                CreatedBy = currentUser
            });

            addedOrder.Entity.OrderedProducts.Add(new OrderedProduct
            {
                OrderId = addedOrder.Entity.Id,
                ProductId = request.ProductId,
                CreatedBy = currentUser
            });

            //var orderedProduct = _orderedProductsDbSet.Add(new OrderedProduct
            //{
            //    OrderId = addedOrder.Entity.Id,
            //    ProductId = request.ProductId,
            //    CreatedBy = "" // TODO: CurrentUser name and email
            //});

            CommitChanges();

            var product = _productsDbSet.Find(request.ProductId);
            return new OrderedProductEntityDto
            {
                Product = FormProductEntityObject(product),
                Orders = new List<OrderMadeDto>
                {
                    FormOrderEntityObject(addedOrder.Entity)
                }
            };
        }

        public OrderedProductEntityDto UpdateOrder(UpdateOrderRequest request, string currentUser)
        {
            var order = _orderDbSet.Find(request.OrderId);
            if (order == null) return null;

            order.BillAmount = request.BillAmount ?? order.BillAmount;
            order.OrderStatus = request.OrderStatus ?? order.OrderStatus;
            order.LastModifiedBy = currentUser;
            order.DateLastModified = DateTimeOffset.Now;

            _orderDbSet.Update(order);
            CommitChanges();

            var product = _productsDbSet.Find(request.ProductId);

            return new OrderedProductEntityDto
            {
                Product = FormProductEntityObject(product),
                Orders = new List<OrderMadeDto>
                {
                    FormOrderEntityObject(order)
                }
            };
        }

        public int DeleteOrder(DeleteItemRequest request, string currentUser)
        {
            var orderToDelete = _orderDbSet.Find(request.Id);
            if (orderToDelete == null) return 0;

            var currentDate = DateTimeOffset.Now;
            orderToDelete.IsDeleted = true;
            orderToDelete.LastModifiedBy = currentUser;
            orderToDelete.DateLastModified = currentDate;

            _orderDbSet.Update(orderToDelete);
            // Also remove it from OrderedProduct List
            _context.Entry(orderToDelete).Collection(p => p.OrderedProducts).Load();
            
            if(orderToDelete.OrderedProducts != null && orderToDelete.OrderedProducts.Count > 0)
            {
                foreach(var orderedProductToDelete in orderToDelete.OrderedProducts)
                {
                    orderedProductToDelete.LastModifiedBy = currentUser; 
                    orderedProductToDelete.DateLastModified = currentDate;

                    _orderedProductsDbSet.Update(orderedProductToDelete);
                }
            }

            CommitChanges();
            return 1;
        }

        #region Private
        private static ProductDto FormProductEntityObject(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                SellerId = product.SellerId,
                SellerEmailAddress = product.Seller.Email,
                Quantity = product.Quantity,
                ImageUrl = product.ImageUrl,
                Status = (bool)product.Status,
                CreatedBy = product.CreatedBy,
                DateCreated = product.DateCreated
            };
        }

        private static OrderMadeDto FormOrderEntityObject(Order order)
        {
            return new OrderMadeDto
            {
                Id = order.Id,
                BillAmount = order.BillAmount,
                OrderStatus = order.OrderStatus,
                Status = (bool)order.Status,
                BuyerId = order.BuyerId,
                BuyerEmailAddress = order.Buyer.Email,
                CreatedBy = order.CreatedBy,
                DateCreated = order.DateCreated
            };
        }

        private void CommitChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private bool _disposeValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposeValue)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                    }
                }

                _disposeValue = true;
            }
        }
        #endregion
    }
}
