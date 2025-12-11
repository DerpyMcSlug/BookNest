using BookNest.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BookNest.DataAccess.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }

    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        // Persist changes to the underlying store
        void Save();
    }
}
