namespace Domain.Data.EntityFramework
{
	using System.Threading.Tasks;

	public class UnitOfWork : IUnitOfWork
	{
		private readonly CargoDbContext context = new CargoDbContext();
		//private readonly IRepository<Article> articleRepository;

		public UnitOfWork()
		{
			//this.articleRepository = new Repository<Article>(this.context);
		}

		//public IRepository<Article> ArticleRepository
		//{
		//	get { return this.articleRepository; }
		//}

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
