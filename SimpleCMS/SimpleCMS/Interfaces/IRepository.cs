using SimpleCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleCMS.DAL
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll(bool includeDeleted = false);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        T Get(int i);
        bool Save(out Exception exception);
    }

    public class Repository<T> : IRepository<T> 
        where T : class, IHasId
    {
        BootstrapContext context = new BootstrapContext();

        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.Entry(entity).State = System.Data.Entity.EntityState.Added;
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
        }

        public void Edit(T entity)
        {
            context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public IQueryable<T> GetAll(bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return context.Set<T>().AsQueryable();
            }
            return context.Set<T>().FilterDeleted().AsQueryable();
        }

        public T Get(int i)
        {
            var all = context.Set<T>().FilterDeleted();
            return all.Where(x => x.Id == i).FirstOrDefault();
        }

        public bool Save(out Exception exception)
        {
            exception = null;
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            return false;
        }
    }
}