using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpyStore.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.DAL.Initializers
{
    public static class SampleData
    {
        // There are two static methods named InitializeData().
        // The first one takes an IServiceProvider, which is a component of the ASP.NET Core Dependency Injection framework, and is used in later chapters.
        // The second one takes an instance of a StoreContext and is used by the tests in the downloadable code.
        public static void InitializeData(IServiceProvider serviceProvider)
        {
            StoreContext? context = serviceProvider.GetService<StoreContext>();
            if (context is not null)
            {
                InitializeData(context);
            }
        }

        public static void InitializeData(StoreContext context)
        {
            context.Database.Migrate();
            //ClearData(context);
            SeedData(context);
        }

        // The ClearData method deletes all of the records in the database, then resets the identity for all of the primary keys.
        public static void ClearData(StoreContext context)
        {
            ExecuteDeleteSQL(context, "Categories");
            ExecuteDeleteSQL(context, "Customers");
            ResetIdentity(context);
        }

        public static void ExecuteDeleteSQL(StoreContext context, string tableName)
        {
            context.Database.ExecuteSqlRaw($"Delete from Store.{tableName}");
        }

        public static void ResetIdentity(StoreContext context)
        {
            var tables = new[] { "Categories", "Customers", "OrderDetails", "Orders", "Products", "ShoppingCartRecords" };
            foreach (var itm in tables)
            {
                context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"Store.{itm}\", RESEED, -1);");
            }
        }

        // The SeedData method calls the methods in the StoreSampleData class to populate the database.
        public static void SeedData(StoreContext context)
        {
            List<Models.Entities.Category> sampleCategories = StoreSampleData.GetCategories().ToList();
            IList<Models.Entities.Product> sampleProducts = StoreSampleData.GetProducts(sampleCategories);
            List<Models.Entities.Customer> sampleCustomers = StoreSampleData.GetAllCustomerRecords(context).ToList();

            try
            {
                // bổ sung AsNoTracking để có thể gọi lại ở phía dưới
                if (!context.Categories.Any())
                {
                    //context.Categories.AddRange(StoreSampleData.GetCategories());
                    context.Categories.AddRange(sampleCategories);
                    context.SaveChanges();
                }
                if (!context.Products.Any())
                {
                    //context.Products.AddRange(StoreSampleData.GetProducts(context.Categories.AsNoTracking().ToList()));
                    context.Products.AddRange(sampleProducts);
                    context.SaveChanges();
                }
                if (!context.Customers.Any())
                {
                    //context.Customers.AddRange(StoreSampleData.GetAllCustomerRecords(context));
                    context.Customers.AddRange(sampleCustomers);
                    context.SaveChanges();
                }

                var customer = context.Customers.AsNoTracking().FirstOrDefault();
                if (customer is not null)
                {
                    List<Models.Entities.Order> orders = StoreSampleData.GetOrders(customer, context);
                    IList<Models.Entities.ShoppingCartRecord> cartRecords = StoreSampleData.GetCart(customer, context);

                    if (!context.Orders.AsNoTracking().Any())
                    {
                        //context.Orders.AddRange(StoreSampleData.GetOrders(customer, context));
                        context.Orders.AddRange(orders);
                        //context.SaveChanges();
                    }

                    if (!context.ShoppingCartRecords.AsNoTracking().Any())
                    {
                        //context.ShoppingCartRecords.AddRange(StoreSampleData.GetCart(customer, context));
                        context.ShoppingCartRecords.AddRange(cartRecords);
                        //context.SaveChanges();
                    }

                    context.SaveChanges();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
