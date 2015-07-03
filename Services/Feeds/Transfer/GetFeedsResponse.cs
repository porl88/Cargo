namespace Services.Feeds.Transfer
{
    using System.Collections.Generic;

    public class GetFeedsResponse : BaseResponse
    {
        public List<FeedDto> Feeds { get; set; }
    }
}
