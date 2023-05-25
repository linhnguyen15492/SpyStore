using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using SpyStore.DAL.EF;
using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpyStore.DAL.Repos.Interfaces;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace SpyStore.DAL.Repos.Base
{
    public abstract class RepoBase<T> : IDisposable, IRepo<T> where T : EntityBase, new()
    {
        // Create a protected variable for the StoreContext, instantiate it in the constructors, and dispose of it in the Dispose method.
        protected readonly StoreContext Db;
        protected DbSet<T> Table;
        public StoreContext Context => Db;

        protected RepoBase()
        {
            Db = new StoreContext();
            Table = Db.Set<T>();
        }

        // The second constructor takes a DbContextOptions instance to support dependency injection.
        protected RepoBase(DbContextOptions<StoreContext> options)
        {
            Db = new StoreContext(options);
            Table = Db.Set<T>();
        }

        bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                // Free any other managed objects here.
                //
            }
            Db.Dispose();
            _disposed = true;
        }

        // A significant advantage to encapsulating the DbSet<T> and DbContext operations in a repository class is wrapping the call to SaveChanges.
        // This enables centralized error handling of calls to make changes to the database.
        public int SaveChanges()
        {
            try
            {
                return Db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //A concurrency error occurred
                //Should handle intelligently
                Console.WriteLine(ex);
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                //DbResiliency retry limit exceeded
                //Should handle intelligently
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                //Should handle intelligently
                Console.WriteLine(ex);
                throw;
            }
        }


        public virtual int Add(T entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }
        public virtual int AddRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.AddRange(entities);
            return persist ? SaveChanges() : 0;
        }
        public virtual int Update(T entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }
        public virtual int UpdateRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }
        public virtual int Delete(T entity, bool persist = true)
        {
            Table.Remove(entity);
            return persist ? SaveChanges() : 0;
        }
        public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.RemoveRange(entities);
            return persist ? SaveChanges() : 0;
        }


        // The Delete method first uses that method to check if the entity is being tracked. If it is, and the TimeStamps match, the entity is removed.
        // If the entity is being tracked and the TimeStamps don’t match, an exception is thrown.
        // If the entity isn’t being tracked, a new entity is created and tracked with the EntityState of Deleted.
        public int Delete(int id, byte[] timeStamp, bool persist = true)
        {
            var entry = GetEntryFromChangeTracker(id);
            if (entry != null)
            {
                if (entry.TimeStamp == timeStamp)
                {
                    return Delete(entry, persist);
                }
                throw new Exception("Unable to delete due to concurrency violation.");
            }
            Db.Entry(new T { Id = id, TimeStamp = timeStamp }).State = EntityState.Deleted;
            return persist ? SaveChanges() : 0;
        }

        internal T? GetEntryFromChangeTracker(int? id)
        {
            return Db.ChangeTracker.Entries<T>().Select((EntityEntry e) => (T)e.Entity)
            .FirstOrDefault(x => x.Id == id);
        }

        public T? Find(int? id) => Table.Find(id);

        public T? GetFirst() => Table.FirstOrDefault();
        public virtual IEnumerable<T> GetAll() => Table;

        // The GetRange method is used for chunking. The base public implementation uses the default sort order.
        // The internal method takes an IQueryable<T> to allow downstream implementations to change the sort order or filters prior to the chunking.
        internal IEnumerable<T> GetRange(IQueryable<T> query, int skip, int take) => query.Skip(skip).Take(take);
        public virtual IEnumerable<T> GetRange(int skip, int take) => GetRange(Table, skip, take);

        public bool HasChanges => Db.ChangeTracker.HasChanges();
        public int Count => Table.Count();
    }
}
