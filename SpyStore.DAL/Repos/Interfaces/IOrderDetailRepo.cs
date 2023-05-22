using SpyStore.DAL.Repos.Base;
using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.DAL.Repos.Interfaces
{
    public interface IOrderDetailRepo : IRepo<OrderDetailRepo>
    {
        IEnumerable<OrderDetailWithProductInfo> GetCustomersOrdersWithDetails(int customerId);
        IEnumerable<OrderDetailWithProductInfo> GetSingleOrderWithDetails(int orderId);
    }
}
