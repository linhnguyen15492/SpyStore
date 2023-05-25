using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SpyStore.DAL.EF;
using SpyStore.DAL.Repos.Base;
using SpyStore.DAL.Repos.Interfaces;
using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.DAL.Repos
{
    public class OrderRepo : RepoBase<Order>, IOrderRepo
    {
        private readonly IOrderDetailRepo _orderDetailRepo;

        // The constructors also take IOrderDetailRepo as a parameter. This will be instantiated by the DI Framework in ASP.NET Core(or manually in unit test classes).
        public OrderRepo(DbContextOptions<StoreContext> options, IOrderDetailRepo orderDetailRepo) : base(options)
        {
            _orderDetailRepo = orderDetailRepo;
        }

        public OrderRepo(IOrderDetailRepo orderDetailRepo)
        {
            _orderDetailRepo = orderDetailRepo;
        }

        public override IEnumerable<Order> GetAll() => Table.OrderByDescending(x => x.OrderDate);

        public override IEnumerable<Order> GetRange(int skip, int take)
            => GetRange(Table.OrderByDescending(x => x.OrderDate), skip, take);

        // The GetOrderHistory gets all of the orders for a customer, removing the navigation properties from the result set.
        public IEnumerable<Order> GetOrderHistory(int customerId)
            => Table.Where(x => x.CustomerId == customerId)
            .Select(x => new Order
            {
                Id = x.Id,
                TimeStamp = x.TimeStamp,
                CustomerId = customerId,
                OrderDate = x.OrderDate,
                OrderTotal = x.OrderTotal,
                ShipDate = x.ShipDate,
            });

        // The GetOneWithDetails() method uses OrderDetailRepo to retrieve the OrderDetails with Product and Category information, which is then added to the view model.
        public OrderWithDetailsAndProductInfo? GetOneWithDetails(int customerId, int orderId)
        {
            return Table.Include(x => x.OrderDetails)
                    .Where(x => x.CustomerId == customerId && x.Id == orderId)
                    .Select(x => new OrderWithDetailsAndProductInfo
                    {
                        Id = x.Id,
                        CustomerId = customerId,
                        OrderDate = x.OrderDate,
                        OrderTotal = x.OrderTotal,
                        ShipDate = x.ShipDate,
                        OrderDetails = _orderDetailRepo.GetSingleOrderWithDetails(orderId).ToList()
                    })
                    .FirstOrDefault();
        }
    }
}
