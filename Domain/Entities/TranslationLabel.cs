namespace Domain.Entities
{
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	public class TranslationLabel
	{
		[Required]
		[DisplayName("Label Name")]
		public string LabelName { get; set; }

		public string German { get; set; }

		public string English { get; set; }

		public string French { get; set; }

		public string Italian { get; set; }

		public string Polish { get; set; }
	}
}
