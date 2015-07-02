namespace Domain.Data.EntityFramework
{
	using System.Threading.Tasks;
	using Domain.Entities;

	public interface IUnitOfWork
	{
		IRepository<Feed> FeedRepository { get; }

		void Commit();

		Task CommitAsync();
	}
}
