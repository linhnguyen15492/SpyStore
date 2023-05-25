using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.DAL.Repos.Base;
using SpyStore.DAL.Repos.Interfaces;
using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.DAL.Repos
{
    // The CustomerRepo provides the core features of the BaseRepo class and implements the ICustomerRepo interface.
    public class CustomerRepo : RepoBase<Customer>, ICustomerRepo
    {
        public CustomerRepo(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public CustomerRepo() : base()
        {
        }

        public override IEnumerable<Customer> GetAll() => Table.OrderBy(x => x.FullName);
        public override IEnumerable<Customer> GetRange(int skip, int take)
        => GetRange(Table.OrderBy(x => x.FullName), skip, take);
    }
}
