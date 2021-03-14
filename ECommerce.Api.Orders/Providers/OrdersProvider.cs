using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {

        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await dbContext.Orders
                    .Where(o => o.CustomerId == customerId)
                    .Include(o => o.Items)
                    .ToListAsync();
                if(orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        private void SeedData()
        {
            if(!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Order()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Today.AddDays(-5),
                    Items = new List<OrderItem>()
                    {
                        new OrderItem() { OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 20 },
                        new OrderItem() { OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 20 },
                        new OrderItem() { OrderId = 2, ProductId = 6, Quantity = 10, UnitPrice = 100 },
                        new OrderItem() { OrderId = 3, ProductId = 3, Quantity = 12, UnitPrice = 52 }
                    },
                    Total = 500
                });

                dbContext.Orders.Add(new Order()
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate = DateTime.Today.AddDays(-5),
                    Items = new List<OrderItem>()
                    {
                        new OrderItem() { OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 20 },
                        new OrderItem() { OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 20 },
                        new OrderItem() { OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 20 },
                        new OrderItem() { OrderId = 2, ProductId = 1, Quantity = 1, UnitPrice = 40 },
                        new OrderItem() { OrderId = 2, ProductId = 6, Quantity = 10, UnitPrice = 100 },
                        new OrderItem() { OrderId = 3, ProductId = 3, Quantity = 12, UnitPrice = 52 }
                    },
                    Total = 500
                });

                dbContext.Orders.Add(new Order()
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = DateTime.Today.AddDays(-5),
                    Items = new List<OrderItem>()
                    {
                        new OrderItem() { OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 20 },
                        new OrderItem() { OrderId = 2, ProductId = 6, Quantity = 10, UnitPrice = 100 },
                        new OrderItem() { OrderId = 3, ProductId = 3, Quantity = 12, UnitPrice = 52 }
                    },
                    Total = 500
                });
                dbContext.SaveChanges();
            }
        }

    }
}
