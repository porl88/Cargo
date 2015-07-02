namespace Domain.Testing
{
	using System.Threading.Tasks;
	using Domain.Data.EntityFramework;
	using Domain.Entities;

	public class MockUnitOfWork : IUnitOfWork
	{
		private readonly IRepository<Feed> feedRepository;

		public MockUnitOfWork()
		{
			this.feedRepository = new MockRepository<Feed>();
		}

		public IRepository<Feed> FeedRepository
		{
			get { return this.feedRepository; }
		}

		public void Commit()
		{
		}

		public Task CommitAsync()
		{
			throw new System.NotImplementedException();
		}
	}
}
