using System.Linq.Expressions;

namespace BookNest.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
		T GetFirstOrDefault(Expression<Func<T, bool>> filter,string? includeProperties = null);
		IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Save();
	}
}
