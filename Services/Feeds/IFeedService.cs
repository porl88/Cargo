namespace Services.Feeds
{
	using System.Threading.Tasks;
	using Services.Feeds.Transfer;

	public interface IFeedService
	{
		Task GetFeedAsync();

		Task<EditFeedResponse> AddFeedAsync(FeedDto feed);

		Task DeleteFeedAsync(int id);
	}
}
