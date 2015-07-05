namespace Bookie.Data.Repositories
{
    using Bookie.Common;
    using Bookie.Common.Model;
    using Bookie.Data.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlServerCe;
    using System.Linq;
    using System.Linq.Expressions;
    using EntityState = System.Data.Entity.EntityState;

    public class GenericDataRepository<T> : IGenericDataRepository<T> where T : class, IEntity
    {
        public virtual IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            try
            {
                using (var context = new Context())
                {
                    IQueryable<T> dbQuery = context.Set<T>();

                    //Apply eager loading
                    foreach (var navigationProperty in navigationProperties) dbQuery = dbQuery.Include(navigationProperty);

                    list = dbQuery.AsNoTracking().ToList();
                }
            }
            catch (SqlCeException ex)
            {
                throw new BookieException(String.Format("{0} - {1}", typeof(T), ex.Message), ex);
            }
            return list;
        }

        public virtual IList<T> GetList(Func<T, bool> where,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            try
            {
                using (var context = new Context())
                {
                    IQueryable<T> dbQuery = context.Set<T>();

                    //Apply eager loading
                    foreach (var navigationProperty in navigationProperties) dbQuery = dbQuery.Include(navigationProperty);

                    list = dbQuery.AsNoTracking().Where(where).ToList();
                }
            }
            catch (SqlCeException ex)
            {
                throw new BookieException(String.Format("{0} - {1}", typeof(T), ex.Message), ex);
            }

            return list;
        }

        public virtual T GetSingle(Func<T, bool> where,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            T item;
            try
            {
                using (var context = new Context())
                {
                    IQueryable<T> dbQuery = context.Set<T>();

                    //Apply eager loading
                    foreach (var navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include(navigationProperty);

                    item = dbQuery
                        .AsNoTracking() //Don't track any changes for the selected item
                        .FirstOrDefault(where); //Apply where clause
                }
            }
            catch (SqlCeException ex)
            {
                throw new BookieException(String.Format("{0} - {1}", typeof(T), ex.Message), ex);
            }

            return item;
        }

        public virtual void Add(params T[] items)
        {
            Update(items);
        }

        public virtual void Update(params T[] items)
        {
            try
            {
                using (var context = new Context())
                {
                    var dbSet = context.Set<T>();
                    foreach (var item in items)
                    {
                        dbSet.Add(item);
                        foreach (var entry in context.ChangeTracker.Entries<IEntity>())
                        {
                            var entity = entry.Entity;
                            entry.State = GetEntityState(entity.EntityState);
                        }
                    }
                    context.SaveChanges();
                }
            }
            catch (SqlCeException ex)
            {
                throw new BookieException(String.Format("{0} - {1}", typeof(T), ex.Message), ex);
            }
        }

        public void Remove(params T[] items)
        {
            Update(items);
        }

        public void Attach(T entity)
        {
            throw new NotImplementedException();
        }

        protected static EntityState GetEntityState(Common.Model.EntityState entityState)
        {
            switch (entityState)
            {
                case Common.Model.EntityState.Unchanged:
                    return EntityState.Unchanged;

                case Common.Model.EntityState.Added:
                    return EntityState.Added;

                case Common.Model.EntityState.Modified:
                    return EntityState.Modified;

                case Common.Model.EntityState.Deleted:
                    return EntityState.Deleted;

                default:
                    return EntityState.Detached;
            }
        }
    }
}