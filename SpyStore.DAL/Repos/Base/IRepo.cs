﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.DAL.Repos.Base
{
    // My version of the repository pattern starts with a core repository for the common methods that will be used
    // on all derived repositories.This core interface (called IRepo) will be supported by an abstract class that
    // encapsulates the DBSet<T> and DbContext methods, plus some additional convenience methods.
    public interface IRepo<T> where T : class
    {
        int Count { get; }
        bool HasChanges { get; }
        T? Find(int? id);
        T? GetFirst();
        IEnumerable<T> GetAll();
        IEnumerable<T> GetRange(int skip, int take);
        int Add(T entity, bool persist = true);
        int AddRange(IEnumerable<T> entities, bool persist = true);
        int Update(T entity, bool persist = true);
        int UpdateRange(IEnumerable<T> entities, bool persist = true);
        int Delete(T entity, bool persist = true);
        int DeleteRange(IEnumerable<T> entities, bool persist = true);
        int Delete(int id, byte[] timeStamp, bool persist = true);
        int SaveChanges();
    }
}
