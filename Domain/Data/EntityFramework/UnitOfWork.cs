namespace Domain.Data.EntityFramework
{
	using System.Threading.Tasks;
	using Domain.Entities;

	public class UnitOfWork : IUnitOfWork
	{
		private readonly CargoDbContext context = new CargoDbContext();
		
		private readonly IRepository<Feed> feedRepository;

		public UnitOfWork()
		{
			this.feedRepository = new Repository<Feed>(this.context);
		}

		public IRepository<Feed> FeedRepository
		{
			get { return this.FeedRepository; }
		}

		public void Commit()
		{
			this.context.SaveChanges();
		}

		public async Task CommitAsync()
		{
			await this.context.SaveChangesAsync();
		}
	}
}
