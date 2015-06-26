namespace Services.Mapping
{
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.IO;
	using Domain.Entities;
	using Domain.Utilities;

	public class SalesVersionMappingService : MappingService<SalesVersion>
	{
		public override MappingResponse Map(List<string> filePaths, NameValueCollection mappings)
		{
			var response = base.Map(filePaths, mappings);
			response.SqlFilePath = this.CreateSql(this.Rows);
			return response;
		}

		private string CreateSql(List<SalesVersion> salesVersions)
		{
			var sqlFilePath = Path.GetTempFileName();
			using (var writer = System.IO.File.CreateText(sqlFilePath))
			{
				foreach (var version in salesVersions)
				{
					this.WriteSqlStatement(writer, version.SalesVersionCode, version.ModelYear, version.SalesVersionDescription, version.Series);
				}
			}

			return sqlFilePath;
		}

		private void WriteSqlStatement(StreamWriter writer, string salesVersionCode, int? modelYear, string salesVersionDescription, int? series)
		{
			if (!string.IsNullOrWhiteSpace(salesVersionCode) && !string.IsNullOrWhiteSpace(salesVersionDescription))
			{
				writer.WriteLine(string.Format("UPDATE cis.Vehicles SET CleanSalesVersionDescription = N'{2}' WHERE CountryCode = 'ES' AND SUBSTRING(pno12, 1, 1) = {3} AND ModelYear = {1} AND SalesVersionCode = '{0}'", salesVersionCode, modelYear, SqlUtility.SqlEncode(salesVersionDescription), series));
			}
		}
	}
}
