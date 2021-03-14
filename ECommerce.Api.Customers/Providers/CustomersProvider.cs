using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {

        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {

                var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);

                if (customer != null)
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(customer);
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

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {

                var customers = await dbContext.Customers.ToListAsync();

                if (customers != null && customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);
                }

                return (false, null, "Not Found");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        //SEED DATA - If no db then just add data to local memory

        private void SeedData()
        {
            if(!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer() { 
                    Id = 1, Name = "Young Chang", 
                    Address = "15 Constya St. Vista, CA 92083", 
                    Email = "ylchang@live.com" });

                dbContext.Customers.Add(new Db.Customer()
                {
                    Id = 2,
                    Name = "sokol@hotmail.com",
                    Address = "4 Ivy Way Dr. Sunnyside, NY 11104",
                    Email = "Constantine Sokol"
                });

                dbContext.Customers.Add(new Db.Customer()
                {
                    Id = 3,
                    Name = "rfisher@msn.com",
                    Address = "28 Lafayette Street. Mount Holly, NJ 08060",
                    Email = "Rich Fisher"
                });

                dbContext.Customers.Add(new Db.Customer()
                {
                    Id = 4,
                    Name = "Cedrik Silvers",
                    Address = "9521 Meadowbrook Street. Spartanburg, SC 29301",
                    Email = "csilvers78@mac.com"
                });

                dbContext.Customers.Add(new Db.Customer()
                {
                    Id = 5,
                    Name = "Mario Phearsom",
                    Address = "894 Gregory Court. Chesterton, IN 46304",
                    Email = "mariothefear@gmail.com"
                });

                dbContext.SaveChanges();

            }
        }
        
    }
}
