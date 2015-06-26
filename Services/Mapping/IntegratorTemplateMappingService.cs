namespace Services.Mapping
{
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.IO;
	using Domain.Entities;
	using Domain.Utilities;

	public class IntegratorTemplateMappingService : MappingService<IntegratorTemplate>
	{
		public override MappingResponse Map(List<string> filePaths, NameValueCollection mappings)
		{
			var response = base.Map(filePaths, mappings);
			response.SqlFilePath = this.CreateSql(this.Rows);
			return response;
		}

        private string CreateSql(List<IntegratorTemplate> templates)
		{
			var sqlFilePath = Path.GetTempFileName();
			using (var writer = System.IO.File.CreateText(sqlFilePath))
			{
                foreach (var template in templates)
				{
                    //this.WriteSqlStatement(writer, 2, label.LabelName, label.English);
                    //this.WriteSqlStatement(writer, 3, label.LabelName, label.German);
                    //this.WriteSqlStatement(writer, 4, label.LabelName, label.French);
                    //this.WriteSqlStatement(writer, 6, label.LabelName, label.Italian);
                    //this.WriteSqlStatement(writer, 38, label.LabelName, label.Polish);
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

        private class Language
        {
            public int LanguageId { get; set; }

            public string Language { get; set; }

            public string LanguageCode { get; set; }
        }

        private List<Language> GetLanguages()
        {
            var languages = new List<Language>();
            languages.Add(new Language { LanguageId = 38, Language = "Azerbaijani", LanguageCode = "az" });
            return languages;
        }
	}
}
