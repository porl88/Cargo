namespace Services.Mapping
{
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Linq;
	using Domain.Classes.CsvMapper;
	using Domain.Utilities;

	public class MappingService<T> : IMappingService where T : class
	{
		protected List<T> Rows;

		public string[] GetHeaders(string filePath)
		{
			var csvProcessor = new CsvMapper<T>();
			return csvProcessor.GetHeaders(filePath);
		}

		public Dictionary<string, string> GetFieldLabels()
		{
			return ReflectionUtility.GetPropertiesByReflection(typeof(T));
		}

		public virtual MappingResponse Map(List<string> filePaths, NameValueCollection mappings)
		{
			var rows = new List<T>();
			var log = new List<LogEntry>();

			foreach (var filePath in filePaths)
			{
				var mapper = new CsvMapper<T>();
				rows.AddRange(mapper.Map(filePath, mappings).ToList());
				log.AddRange(mapper.Log);
			}

			this.Rows = rows;

			var response = new MappingResponse
			{
				ResultsAsTable = ReflectionUtility.GetDataTableByReflection<T>(rows),
				Log = log
			};

			return response;
		}
	}
}
