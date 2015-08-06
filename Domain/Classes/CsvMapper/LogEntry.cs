namespace Domain.Classes.CsvMapper
{
	public enum ErrorType
	{
		Fail,
		Warning
	}

	public class LogEntry
	{
		public int RowCount { get; set; }

		public ErrorType ErrorType { get; set; }

		public string Message { get; set; }

		public string ColumnName { get; set; }

		public string Value { get; set; }
	}
}
