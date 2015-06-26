namespace Services.Mapping
{
	using System.Collections.Generic;
	using System.Data;
	using Domain.Classes.CsvMapper;

	public class MappingResponse
	{
		public List<LogEntry> Log { get; set; }

		public DataTable ResultsAsTable { get; set; }

		public string SqlFilePath { get; set; }
	}
}
