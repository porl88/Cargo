namespace Domain.Data.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

	public interface IRepository<T>
	{
		T Get(int id);

        IQueryable<T> Get();
        
        Task<T> GetAsync(int id);

        Task<List<T>> GetAllAsync();

        Task<List<T>> FindAsync(Expression<Func<T, bool>> filter);

        //Task<List<T>> FindAsync(Expression<Func<T, bool>> filter, int skip, int take);
        
        T Insert(T entity);

		T Update(T entity);

		void Delete(int id);

		void Delete(T entity);
	}
}
