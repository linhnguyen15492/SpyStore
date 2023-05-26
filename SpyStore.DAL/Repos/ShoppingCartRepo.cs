using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.DAL.Exceptions;
using SpyStore.DAL.Repos.Base;
using SpyStore.DAL.Repos.Interfaces;
using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.DAL.Repos
{
    public class ShoppingCartRepo : RepoBase<ShoppingCartRecord>, IShoppingCartRepo
    {
        private readonly IProductRepo _productRepo;
        public ShoppingCartRepo(DbContextOptions<StoreContext> options,
        IProductRepo productRepo) : base(options)
        {
            _productRepo = productRepo;
        }
        public ShoppingCartRepo(IProductRepo productRepo) : base()
        {
            _productRepo = productRepo;
        }


        public override IEnumerable<ShoppingCartRecord> GetAll() => Table.OrderByDescending(x => x.DateCreated);
        public override IEnumerable<ShoppingCartRecord> GetRange(int skip, int take) => GetRange(Table.OrderByDescending(x => x.DateCreated), skip, take);

        // The additional Find method uses the Id of the Customer and the Id of a Product to find a specific record.
        public ShoppingCartRecord? Find(int customerId, int productId)
        {
            return Table.FirstOrDefault(x => x.CustomerId == customerId && x.ProductId == productId);
        }

        public override int Update(ShoppingCartRecord entity, bool persist = true)
        {
            return Update(entity, _productRepo.Find(entity.ProductId)?.UnitsInStock, persist);
        }
        public int Update(ShoppingCartRecord entity, int? quantityInStock, bool persist = true)
        {
            if (entity.Quantity <= 0)
            {
                return Delete(entity, persist);
            }
            if (entity.Quantity > quantityInStock)
            {
                throw new InvalidQuantityException("Can't add more product than available in stock");
            }
            // lưu ý là gọi vào base
            return base.Update(entity, persist);
        }

        // The Add method checks to see if there are any of the same products already in the cart.
        public override int Add(ShoppingCartRecord entity, bool persist = true)
        {
            return Add(entity, _productRepo.Find(entity.ProductId)?.UnitsInStock, persist);
        }

        // The Add method checks to see if there are any of the same products already in the cart.
        // If there are, the quantity is updated(instead of adding a new record).
        // If there aren’t any, the product is added to the cart.
        // When the cart is updated, if the new quantity is less than or equal to zero, the item is deleted from the cart. Otherwise, the quantity is simply updated.
        // Both the Add and Update methods check the available inventory, and if the user is attempting to add more records into the cart than are available,
        // an InvalidQuantityException is thrown.
        public int Add(ShoppingCartRecord entity, int? quantityInStock, bool persist = true)
        {
            var item = Find(entity.CustomerId, entity.ProductId);
            if (item == null)
            {
                if (quantityInStock != null && entity.Quantity > quantityInStock.Value)
                {
                    throw new InvalidQuantityException("Can't add more product than available in stock");
                }
                return base.Add(entity, persist);
            }
            item.Quantity += entity.Quantity;
            return item.Quantity <= 0 ? Delete(item, persist) : Update(item, quantityInStock, persist);
        }

        // The GetRecord method simplifies the projections in the GetShoppingCartRecord and GetShoppingCartRecords methods.
        internal CartRecordWithProductInfo GetRecord(int customerId, ShoppingCartRecord scr, Product p, Category c)
            => new CartRecordWithProductInfo
            {
                Id = scr.Id,
                DateCreated = scr.DateCreated,
                CustomerId = customerId,
                Quantity = scr.Quantity,
                ProductId = scr.ProductId,
                Description = p.Description,
                ModelName = p.ModelName,
                ModelNumber = p.ModelNumber,
                ProductImage = p.ProductImage,
                ProductImageLarge = p.ProductImageLarge,
                ProductImageThumb = p.ProductImageThumb,
                CurrentPrice = p.CurrentPrice,
                UnitsInStock = p.UnitsInStock,
                CategoryName = c.CategoryName,
                LineItemTotal = scr.Quantity * p.CurrentPrice,
                TimeStamp = scr.TimeStamp
            };

        public CartRecordWithProductInfo GetShoppingCartRecord(int customerId, int productId)
            => Table.Where(x => x.CustomerId == customerId && x.ProductId == productId)
            .Include(x => x.Product!)
                .ThenInclude(p => p.Category!)
            .Select(x => GetRecord(customerId, x, x.Product!, x.Product!.Category!))
            .FirstOrDefault()!;

        public IEnumerable<CartRecordWithProductInfo> GetShoppingCartRecords(int customerId)
            => Table.Where(x => x.CustomerId == customerId)
            .Include(x => x.Product!)
                .ThenInclude(p => p.Category)
            .Select(x => GetRecord(customerId, x, x.Product!, x.Product!.Category!))
            .OrderBy(x => x.ModelName);

        // The Purchase() method executes the stored procedure built earlier in the chapter.
        public int Purchase(int customerId)
        {
            var customerIdParam = new SqlParameter("@customerId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = customerId
            };
            var orderIdParam = new SqlParameter("@orderId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            try
            {
                Context.Database.ExecuteSqlRaw("EXEC [Store].[PurchaseItemsInCart] @customerId, @orderid out",
                    customerIdParam, orderIdParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
            return (int)orderIdParam.Value;
        }
    }
}
