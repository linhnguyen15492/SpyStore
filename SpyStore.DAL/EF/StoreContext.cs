using Microsoft.EntityFrameworkCore;
using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.DAL.EF
{
    public class StoreContext : DbContext
    {
        private const string connectionString = @"Server=localhost;
                                                    Database=SpyStore;
                                                    Trusted_Connection=True;
                                                    MultipleActiveResultSets=true;
                                                    Trust Server Certificate=true";

        public StoreContext()
        {
        }

        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
    }
}
