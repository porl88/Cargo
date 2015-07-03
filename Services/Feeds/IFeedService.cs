namespace Services.Feeds
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Services.Feeds.Transfer;

	public interface IFeedService
	{
        Task<GetFeedsResponse> GetAllFeedsAsync();

		Task<EditFeedResponse> AddFeedAsync(FeedDto feed);

		Task DeleteFeedAsync(int id);
	}
}
