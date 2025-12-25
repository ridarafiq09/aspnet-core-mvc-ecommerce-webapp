using FlipShop.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;

namespace FlipShop.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private FlipShopContext _context;

        public Repository(FlipShopContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            
            _context.Set<TEntity>().Add(entity);
        }

        public IQueryable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> Predicate)
        {
            return _context.Set<TEntity>().Where(Predicate);
        }

        public IQueryable<TEntity> GetAll(int pageNo)
        {
            int pageSize = 10;
            return _context.Set<TEntity>().Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        public TEntity? GetById(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public IQueryable<TEntity> FindByCategory(System.Linq.Expressions.Expression<Func<TEntity, bool>> Predicate, string NavigationalProperty)
        {
            return _context.Set<TEntity>().Where(Predicate).Include(NavigationalProperty);
        }

        public void Remove(TEntity entity)
        {
            _context.Entry<TEntity>(entity).State = EntityState.Deleted;
        }

        //see it
        public void Update(TEntity entity)
        {
            _context.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        private bool isDisposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _context = null;
            }
            this.isDisposed = true;
        }
    }
}
