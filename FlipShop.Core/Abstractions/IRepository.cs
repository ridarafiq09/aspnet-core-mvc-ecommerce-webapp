using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FlipShop.Core.Abstractions
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(int id);
        IQueryable<TEntity> GetAll(int pageNo);
        IQueryable<TEntity> Find(Expression<Func<TEntity,bool>> Predicate);
        IQueryable<TEntity> FindByCategory(Expression<Func<TEntity, bool>> Predicate, string NavigationalProperty);
        void Update(TEntity entity);
        void Add(TEntity entity);   
        void Remove(TEntity entity);    

    }
}
