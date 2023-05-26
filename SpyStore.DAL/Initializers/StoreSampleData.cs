using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.DAL.Initializers
{
    public static class StoreSampleData
    {
        public static IEnumerable<Category> GetCategories() => new List<Category>
        {
            new Category { CategoryName = "Communications" },
            new Category { CategoryName = "Deception" },
            new Category { CategoryName = "Travel" },
        };

        public static IList<Product> GetProducts(IList<Category> categories)
        {
            var products = new List<Product>();
            foreach (var category in categories)
            {
                switch (category.CategoryName)
                {
                    case "Communications":
                        products.AddRange(new List<Product>
                        {
                            new Product
                            {
                                Category = category,
                                CategoryId = category.Id,
                                ProductImage = "product-image.png",
                                ProductImageLarge = "product-image-lg.png",
                                ProductImageThumb = "product-thumb.png",
                                ModelNumber = "RED1",
                                ModelName = "Communications Device",
                                UnitCost = 49.99M,
                                CurrentPrice = 49.99M,
                                Description = "Lorem ipsum",
                                UnitsInStock = 2,
                                IsFeatured = true
                            }
                        });
                        break;

                    default:
                        break;
                }
            }
            return products;
        }

        public static IEnumerable<Customer> GetAllCustomerRecords(StoreContext context) => new List<Customer>
        {
            new Customer() { EmailAddress = "spy@secrets.com",Password = "Foo", FullName = "Super Spy", }
        };

        public static List<Order> GetOrders(Customer customer, StoreContext context)
        {
            Order order = new Order()
            {
                Customer = customer,
                OrderDate = DateTime.Now.Subtract(new TimeSpan(20, 0, 0, 0)),
                ShipDate = DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0)),
            };

            GetOrderDetails(order, context);
            return new List<Order> { order };

            //return new List<Order>
            //{
            //    new Order()
            //    {
            //        Customer = customer,
            //        OrderDate = DateTime.Now.Subtract(new TimeSpan(20, 0, 0, 0)),
            //        ShipDate = DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0)),
            //        OrderDetails = GetOrderDetails(context)
            //    }
            //};
        }

        public static IList<OrderDetail> GetOrderDetails(Order order, StoreContext context)
        {
            var prod1 = context.Categories.Include(c => c.Products)
                                            .FirstOrDefault()?.Products.FirstOrDefault();

            var prod2 = context.Categories.Skip(1).Include(c => c.Products)
                                            .FirstOrDefault()?.Products.FirstOrDefault();

            var prod3 = context.Categories.Skip(2).Include(c => c.Products)
                                            .FirstOrDefault()?.Products.FirstOrDefault();

            return new List<OrderDetail>
            {
                new OrderDetail() {Order = order, Product = prod1, Quantity = 3,
                UnitCost = prod1!.CurrentPrice},
                new OrderDetail() {Order = order, Product = prod2, Quantity = 2,
                UnitCost = prod2!.CurrentPrice},
                new OrderDetail() {Order = order, Product = prod3, Quantity = 5,
                UnitCost = prod3!.CurrentPrice},
            };
        }

        public static IList<ShoppingCartRecord> GetCart(Customer customer, StoreContext context)
        {
            var prod1 = context.Categories.Skip(1)
            .Include(c => c.Products).FirstOrDefault()?
            .Products.FirstOrDefault();
            return new List<ShoppingCartRecord>
            {
                new ShoppingCartRecord {Customer = customer, DateCreated = DateTime.Now, Product = prod1, Quantity = 1}
            };
        }
    }
}
