namespace Domain.Data.EntityFramework
{
	using System.Linq;
	using System.Threading.Tasks;

	public interface IRepository<T>
	{
		IQueryable<T> Get();

		T Get(int id);

		Task<T> GetAsync(int id);
		
		T Insert(T entity);

		T Update(T entity);

		void Delete(int id);

		void Delete(T entity);
	}
}
