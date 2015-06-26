namespace Domain.Entities
{
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	public class SalesVersion
	{
		[Required]
		[DisplayName("Sales Version Code")]
		public string SalesVersionCode { get; set; }

		[Required]
		[DisplayName("Model Year")]
		public int? ModelYear { get; set; }

		[Required]
		public int? Series { get; set; }

		[Required]
		[DisplayName("Sales Version Description")]
		public string SalesVersionDescription { get; set; }
	}
}
