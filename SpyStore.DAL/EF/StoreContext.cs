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

        #region DBSet
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCartRecord> ShoppingCartRecords { get; set; }

        #endregion DBSet

        public StoreContext()
        {
        }

        public StoreContext(DbContextOptions options) : base(options)
        {
            try
            {
                Database.Migrate();
            }
            catch (Exception)
            {
                //Should do something intelligent here
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
            //optionsBuilder.EnableSensitiveDataLogging();
        }

        // The Fluent API is another mechanism to shape your database. Fluent API code is placed in the override of the OnModelCreating() method in your DbContext class.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.EmailAddress).HasDatabaseName("IX_Customers").IsUnique();
            });


            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");
                entity.Property(e => e.ShipDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");
                entity.Property(e => e.OrderTotal)
                    .HasColumnType("money")
                    .HasComputedColumnSql("Store.GetOrderTotal([Id])")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.LineItemTotal)
                    .HasColumnType("money")
                    .HasComputedColumnSql("[Quantity]*[UnitCost]")
                    .ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.UnitCost).HasColumnType("money");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.UnitCost).HasColumnType("money");
                entity.Property(e => e.CurrentPrice).HasColumnType("money");
            });

            modelBuilder.Entity<ShoppingCartRecord>(entity =>
            {
                entity.HasIndex(e => new { ShoppingCartRecordId = e.Id, e.ProductId, e.CustomerId })
                    .HasDatabaseName("IX_ShoppingCart")
                    .IsUnique();
                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");
                entity.Property(e => e.Quantity).HasDefaultValue(1);
            });
        }
    }
}
