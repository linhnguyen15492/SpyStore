﻿using Microsoft.EntityFrameworkCore;
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
    // The OrderDetail repository adds a new LINQ extension into your toolbox: ThenInclude. This method (introduced in EF Core) can simplify long LINQ queries as the queries walk the domain model.
    public class OrderDetailRepo : RepoBase<OrderDetail>, IOrderDetailRepo
    {
        public OrderDetailRepo(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public OrderDetailRepo()
        {
        }

        public override IEnumerable<OrderDetail> GetAll() => Table.OrderBy(x => x.Id);
        public override IEnumerable<OrderDetail> GetRange(int skip, int take) => GetRange(Table.OrderBy(x => x.Id), skip, take);


        // The internal GetRecords method starts with an IQueryable<OrderDetail> and then uses a projection to populate a list of OrderDetailWithProductInfo.
        internal IEnumerable<OrderDetailWithProductInfo> GetRecords(IQueryable<OrderDetail> query)
        {
            return query.Include(x => x.Product!)
                            .ThenInclude(p => p.Category)
                        .Select(x => new OrderDetailWithProductInfo
                        {
                            OrderId = x.OrderId,
                            ProductId = x.ProductId,
                            Quantity = x.Quantity,
                            UnitCost = x.UnitCost,
                            LineItemTotal = x.LineItemTotal,
                            Description = x.Product!.Description,
                            ModelName = x.Product.ModelName,
                            ProductImage = x.Product.ProductImage,
                            ProductImageLarge = x.Product.ProductImageLarge,
                            ProductImageThumb = x.Product.ProductImageThumb,
                            ModelNumber = x.Product.ModelNumber,
                            CategoryName = x.Product!.Category!.CategoryName
                        })
                        .OrderBy(x => x.ModelName);
        }

        // The GetCustomersOrdersWithDetails and GetSingleOrderWithDetails methods create the initial query(searching by CustomerId or OrderId, respectively).
        public IEnumerable<OrderDetailWithProductInfo> GetCustomersOrdersWithDetails(int customerId)
            => GetRecords(Table.Where(x => x.Order!.CustomerId == customerId));

        public IEnumerable<OrderDetailWithProductInfo> GetSingleOrderWithDetails(int orderId)
            => GetRecords(Table.Where(x => x.Order!.Id == orderId));
    }
}
