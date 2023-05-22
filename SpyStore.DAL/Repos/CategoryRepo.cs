using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.DAL.Repos.Base;
using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.DAL.Repos
{
    public class CategoryRepo : RepoBase<Category>
    {
        public CategoryRepo()
        {
        }

        public CategoryRepo(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public override IEnumerable<Category> GetAll() => Table.OrderBy(x => x.CategoryName);
        public override IEnumerable<Category> GetRange(int skip, int take) => GetRange(Table.OrderBy(x => x.CategoryName), skip, take);
    }
}
