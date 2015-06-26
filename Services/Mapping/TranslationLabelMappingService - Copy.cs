namespace Services.Mapping
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
	using System.IO;
    using System.Linq;
	using Domain.Classes.CsvMapper;
    using Domain.Entities;
    using Domain.Utilities;

	public class TranslationLabelMappingService : IMappingService
	{
        private NameValueCollection mappings;

		public string[] GetHeaders(string filePath, char delimiter)
		{
			using (var csvProcessor = new CsvMapper<TranslationLabel>(filePath, delimiter))
			{
				return csvProcessor.GetHeaders();
			}
		}

		public Dictionary<string, string> GetFieldLabels()
		{
            return ReflectionUtility.GetPropertiesByReflection(typeof(TranslationLabel));
		}

		public MappingResponse Map(string filePath, char delimiter, NameValueCollection mappings)
		{
			using (var mapper = new CsvMapper<TranslationLabel>(filePath, delimiter))
			{
				this.mappings = mappings;
				mapper.OnItemDataBound += CsvProcessor_OnItemDataBound;
				var translationLabels = mapper.MapToClass().Skip(1).ToList();

				var response = new MappingResponse
				{
					Results = ReflectionUtility.GetDataTableByReflection<TranslationLabel>(translationLabels),
					Log = mapper.Log,
					SqlFilePath = this.CreateSql(translationLabels)
				};

				return response;
			}
		}

		private TranslationLabel CsvProcessor_OnItemDataBound(object sender, CsvMapperEventArgs e)
		{
			return new TranslationLabel
			{
                LabelName = e.MapColumn<string>(this.mappings["LabelName"]),
                English = e.MapColumn<string>(this.mappings["English"]),
                German = e.MapColumn<string>(this.mappings["German"]),
                French = e.MapColumn<string>(this.mappings["French"]),
                Italian = e.MapColumn<string>(this.mappings["Italian"]),
                Polish = e.MapColumn<string>(this.mappings["Polish"])
			};
		}

		private string CreateSql(List<TranslationLabel> translationLabels)
		{
			var sqlFilePath = Path.GetTempFileName();
			using (var writer = System.IO.File.CreateText(sqlFilePath))
			{
				foreach (var label in translationLabels)
				{
					this.WriteSqlStatement(writer, 2, label.LabelName, label.English);
					this.WriteSqlStatement(writer, 3, label.LabelName, label.German);
					this.WriteSqlStatement(writer, 4, label.LabelName, label.French);
					this.WriteSqlStatement(writer, 6, label.LabelName, label.Italian);
					this.WriteSqlStatement(writer, 38, label.LabelName, label.Polish);
				}
			}

			return sqlFilePath;
		}

		private void WriteSqlStatement(StreamWriter writer, int languageId, string labelName, string labelValue)
		{
			if (!string.IsNullOrWhiteSpace(labelName) && !string.IsNullOrWhiteSpace(labelValue))
			{
				writer.WriteLine(string.Format("EXEC spRoll_InsertUpdateVariableAndVariableTranslation '{0}', {1}, N'{2}', {3}, {4}", labelName, languageId, SqlUtility.SqlEncode(labelValue), 88, 384));
			}
		}
	}
}
