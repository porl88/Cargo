namespace Services.Tests.Feeds
{
	using System;
	using System.Threading.Tasks;
	using Domain.Data.EntityFramework;
	using Domain.Testing;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Services.Feeds;
	using Services.Feeds.Transfer;

	[TestClass]
	public class FeedServiceTests
	{
		private readonly FeedService feedService;

		private IUnitOfWork unitOfWork;

		public FeedServiceTests()
		{
			this.unitOfWork = new MockUnitOfWork();
			this.feedService = new FeedService(this.unitOfWork);
		}

		[TestMethod]
		public async Task AddFeedAsyncTest()
		{
			// arrange
			var mappings = new int[] { 5, 2, 3, 1, 4 };

			var feed = new FeedDto
			{
				Class = "TestClass",
				Mappings = string.Join(",", mappings)
			};

			// act
			var result = await this.feedService.AddFeedAsync(feed);

			// assert
			Assert.AreEqual(ResponseStatus.OK, result.Status);
		}
	}
}
