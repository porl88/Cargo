namespace Services.Tests.Feeds
{
	using System;
    using System.Linq;
	using System.Threading.Tasks;
	using Domain.Data.EntityFramework;
	using Domain.Testing;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Services.Feeds;
	using Services.Feeds.Transfer;

	[TestClass]
	public class FeedServiceTests
	{
        public async Task GetAllFeedsAsyncTest()
        {
            // arrange
            var unitOfWork = new MockUnitOfWork();
            var feedService = new FeedService(unitOfWork);

            var mappings = new int[] { 5, 2, 3, 1, 4 };

            var feed = new FeedDto
            {
                Class = "TestClass",
                Mappings = string.Join(",", mappings)
            };

            // act
            var result = await feedService.GetAllFeedsAsync();

            // assert
            Assert.AreEqual(ResponseStatus.OK, result.Status);
            var repositoryItems = unitOfWork.FeedRepository.Get().ToList();
            Assert.AreEqual(1, repositoryItems.Count());
        }

		[TestMethod]
		public async Task AddFeedAsyncTest()
		{
			// arrange
            var unitOfWork = new MockUnitOfWork();
            var feedService = new FeedService(unitOfWork);

			var mappings = new int[] { 5, 2, 3, 1, 4 };

			var feed = new FeedDto
			{
				Class = "TestClass",
				Mappings = string.Join(",", mappings)
			};

			// act
			var result = await feedService.AddFeedAsync(feed);

			// assert
			Assert.AreEqual(ResponseStatus.OK, result.Status);
            var repositoryItems = unitOfWork.FeedRepository.Get().ToList();
            Assert.AreEqual(1, repositoryItems.Count());
		}
	}
}
