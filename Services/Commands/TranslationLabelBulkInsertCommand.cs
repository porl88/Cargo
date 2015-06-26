namespace Services.Commands
{
	using System.Collections.Generic;
	using System.IO;
	using Domain.Entities;
	using Domain.Utilities;

    public class TranslationLabelBulkInsertCommand : ISqlBulkInsertCommand
    {
		private readonly List<TranslationLabel> translationLabels;

		public string SqlFilePath { get; private set; }

		public TranslationLabelBulkInsertCommand(List<TranslationLabel> translationLabels)
		{
			this.translationLabels = translationLabels;
		}

        public void Execute()
        {
			this.SqlFilePath = Path.GetTempFileName();
			this.CreateSql(this.translationLabels, this.SqlFilePath);
        }

		private void CreateSql(List<TranslationLabel> translationLabels, string sqlFilePath)
		{
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
