namespace Services.Mapping
{
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.IO;
	using Domain.Entities;
	using Domain.Utilities;

	public class TranslationLabelMappingService : MappingService<TranslationLabel>
	{
		public override MappingResponse Map(List<string> filePaths, NameValueCollection mappings)
		{
			var response = base.Map(filePaths, mappings);
			response.SqlFilePath = this.CreateSql(this.Rows);
			return response;
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
				writer.WriteLine(string.Format("EXEC spRoll_InsertUpdateVariableAndVariableTranslation '{0}', {1}, N'{2}', {3}, {4}", SqlUtility.SqlEncode(labelName), languageId, SqlUtility.SqlEncode(labelValue), 88, 384));
			}
		}
	}
}
