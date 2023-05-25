// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.Models.Entities;

Console.WriteLine("Hello world!");

/*
1. Create the context class.
2. Create a base entity class.
3. Create and configure model class(es).
4. Add the model to the context as a DbSet<T>.
5. Create a migration and run it to update the database.
6. Add a strongly typed repository for the model.


 */

StoreContext context = new StoreContext();
var c = new Category() { CategoryName = "test", Products = new List<Product>() };
context.Categories.Add(c);
context.SaveChanges();
Console.WriteLine("thêm ngành hàng thành công");

var a = context.Categories.AsNoTracking().ToList();
Console.WriteLine(a);