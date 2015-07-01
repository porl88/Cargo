namespace Domain.Data.EntityFramework
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	public interface IRepository<T>
	{
		T Get(int id);

		Task<T> GetAsync(int id);

		IQueryable<T> Get();

		IQueryable<T> Find(Expression<Func<T, bool>> filter);

		T Insert(T entity);

		T Update(T entity);

		void Delete(int id);

		void Delete(T entity);
	}
}
