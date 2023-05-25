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
    public class ProductRepo : RepoBase<Product>, IProductRepo
    {
        public ProductRepo(DbContextOptions<StoreContext> options) : base(options)
        {
            Table = Context.Products;
        }
        public ProductRepo() : base()
        {
            Table = Context.Products;
        }

        // The GetAll() method overrides the base GetAll() method to return the Product records in order of the ModelName property.
        public override IEnumerable<Product> GetAll() => Table.OrderBy(x => x.ModelName);

        // The internal GetRecord method is used to project the LINQ results in the following methods
        // (GetAllWithCategoryName(), GetFeaturedWithCategoryName(), GetOneWithCategoryName(), and Search()) to create the ProductAndCategoryBase view model.
        internal ProductAndCategoryBase GetRecord(Product p, Category c) => new ProductAndCategoryBase()
        {
            CategoryName = c.CategoryName,
            CategoryId = p.CategoryId,
            CurrentPrice = p.CurrentPrice,
            Description = p.Description,
            IsFeatured = p.IsFeatured,
            Id = p.Id,
            ModelName = p.ModelName,
            ModelNumber = p.ModelNumber,
            ProductImage = p.ProductImage,
            ProductImageLarge = p.ProductImageLarge,
            ProductImageThumb = p.ProductImageThumb,
            TimeStamp = p.TimeStamp,
            UnitCost = p.UnitCost,
            UnitsInStock = p.UnitsInStock
        };

        // The GetProductsForCategory() method returns all of the Product records for a specific Category Id, ordered by ModelName.
        public IEnumerable<ProductAndCategoryBase> GetProductsForCategory(int id) => Table
                .Where(p => p.CategoryId == id)
                .Include(p => p.Category)
                .Select(item => GetRecord(item, item.Category!))
                .OrderBy(x => x.ModelName);

        public IEnumerable<ProductAndCategoryBase> GetAllWithCategoryName() => Table
                .Include(p => p.Category)
                .Select(item => GetRecord(item, item.Category!))
                .OrderBy(x => x.ModelName);

        public IEnumerable<ProductAndCategoryBase> GetFeaturedWithCategoryName() => Table
                .Where(p => p.IsFeatured)
                .Include(p => p.Category)
                .Select(item => GetRecord(item, item.Category!))
                .OrderBy(x => x.ModelName);

        public ProductAndCategoryBase? GetOneWithCategoryName(int id) => Table
                .Where(p => p.Id == id)
                .Include(p => p.Category)
                .Select(item => GetRecord(item, item.Category!))
                .SingleOrDefault();

        public IEnumerable<ProductAndCategoryBase> Search(string searchString) => Table
                .Where(p => p.Description.ToLower().Contains(searchString.ToLower()) || p.ModelName.ToLower().Contains(searchString.ToLower()))
                .Include(p => p.Category)
                .Select(item => GetRecord(item, item.Category!))
                .OrderBy(x => x.ModelName);
    }
}
