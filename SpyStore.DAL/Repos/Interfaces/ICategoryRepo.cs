using SpyStore.DAL.Repos.Base;
using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.DAL.Repos.Interfaces
{
    public interface ICategoryRepo : IRepo<Category>
    {
        IEnumerable<Category> GetAllWithProducts();
        Category GetOneWithProducts(int? id);
    }
}
