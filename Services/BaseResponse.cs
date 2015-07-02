namespace Services
{
	public enum ResponseStatus
	{
		OK,
		NotFound,
		BadRequest,
		SystemError
	}

	public abstract class BaseResponse
	{
		public ResponseStatus Status { get; set; }
	}
}
