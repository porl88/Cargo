namespace Services.Feeds
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Data.EntityFramework;
    using Domain.Entities;
    using Services.Feeds.Transfer;

    public class FeedService : IFeedService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IRepository<Feed> feedRepository;

        //private readonly ILogService logService;

        public FeedService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.feedRepository = unitOfWork.FeedRepository;
        }

        public async Task<GetFeedsResponse> GetAllFeedsAsync()
        {
            var response = new GetFeedsResponse();

            try
            {
                var feeds = await this.feedRepository.GetAllAsync();
                response.Feeds = feeds.Select(x => new FeedDto
                {
                    Id = x.Id,
                    Class = x.Class,
                    Mappings = x.Mappings
                }).ToList();
                response.Status = ResponseStatus.OK;
            }
            catch (Exception ex)
            {
                //this.logService.LogError(ex, new HttpContextWrapper(HttpContext.Current));
                response.Status = ResponseStatus.SystemError;
            }

            return response;
        }

        public async Task<EditFeedResponse> AddFeedAsync(FeedDto feed)
        {
            var response = new EditFeedResponse();

            try
            {
                var now = DateTime.Now;

                var newFeed = new Feed
                {
                    Class = feed.Class,
                    Mappings = feed.Mappings,
                    Created = now,
                    Updated = now
                };

                var addedArticle = this.feedRepository.Insert(newFeed);
                await this.unitOfWork.CommitAsync();
                response.Status = ResponseStatus.OK;
            }
            catch (Exception ex)
            {
                //this.logService.LogError(ex, new HttpContextWrapper(HttpContext.Current));
                response.Status = ResponseStatus.SystemError;
            }

            return response;
        }

        public Task DeleteFeedAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
