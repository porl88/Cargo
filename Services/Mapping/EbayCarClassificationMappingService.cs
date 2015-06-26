namespace Services.Mapping
{
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.IO;
	using System.Linq;
	using Domain.Classes.CsvMapper;
	using Domain.Entities;
	using Domain.Utilities;

	public class EbayCarClassificationMappingService : MappingService<EbayCarClassification>
	{
		public override MappingResponse Map(List<string> filePaths, NameValueCollection mappings)
		{
			var response = base.Map(filePaths, mappings);
			response.SqlFilePath = this.CreateSql(this.Rows);
			return response;
		}

		private string CreateSql(List<EbayCarClassification> classifications)
		{
			var sqlFilePath = Path.GetTempFileName();
			using (var writer = System.IO.File.CreateText(sqlFilePath))
			{
				writer.WriteLine("DECLARE @VehicleModelId INT");

				foreach (var classification in classifications)
				{
					this.WriteSqlStatement(writer, classification);
				}
			}

			return sqlFilePath;
		}

		private void WriteSqlStatement(StreamWriter writer, EbayCarClassification classification)
		{
			writer.WriteLine(string.Format("SET @VehicleModelId = (SELECT TOP 1 VehicleModelId FROM VehicleModels m INNER JOIN OEMs o ON o.OEMID = m.OEMID WHERE o.Name = '{0}' AND m.Name = '{1}')", SqlUtility.SqlEncode(classification.MakeName), SqlUtility.SqlEncode(classification.ModelName)));
			writer.WriteLine(string.Format("IF NOT EXISTS (SELECT 'X' FROM dbo.EbayModelMappings WHERE ModelId = '{2}') INSERT INTO dbo.EbayModelMappings (VehicleModelId, MakeId, MakeName, ModelId, ModelName, MarketId) VALUES (@VehicleModelId, {0}, N'{1}', '{2}', N'{3}', '{4}')", classification.MakeId, SqlUtility.SqlEncode(classification.MakeName), classification.ModelId, SqlUtility.SqlEncode(classification.ModelName), classification.MarketId));
		}
	}
}
