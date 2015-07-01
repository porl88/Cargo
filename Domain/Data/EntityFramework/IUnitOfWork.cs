namespace Domain.Data.EntityFramework
{
	using System.Threading.Tasks;

	public interface IUnitOfWork
	{
		void Commit();

		Task CommitAsync();
	}
}
