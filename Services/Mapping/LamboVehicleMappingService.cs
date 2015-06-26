namespace Services.Mapping
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using Domain.Entities;
    using Domain.Utilities;

    public class LamboVehicleMappingService : MappingService<LamboVehicle>
    {
        public override MappingResponse Map(List<string> filePaths, NameValueCollection mappings)
        {
            var response = base.Map(filePaths, mappings);
            response.SqlFilePath = this.CreateSql(this.Rows);
            return response;
        }

        private string CreateSql(List<LamboVehicle> translationLabels)
        {
            var sqlFilePath = Path.GetTempFileName();
            using (var writer = System.IO.File.CreateText(sqlFilePath))
            {
                writer.WriteLine("DECLARE @RESULTS TABLE ( VIN VarChar(20)); DECLARE @TABLE TABLE ( VIN VarChar(20)); DECLARE @VIN VarChar(20); DECLARE @ROWCOUNT INT;");
                foreach (var label in translationLabels)
                {
                    this.WriteSqlStatement(writer, label.Vin);
                }
                writer.WriteLine("SELECT * FROM @RESULTS;");
            }

            return sqlFilePath;
        }

        private void WriteSqlStatement(StreamWriter writer, string vin)
        {
            if (!string.IsNullOrWhiteSpace(vin))
            {
                writer.WriteLine(string.Format("DELETE FROM @TABLE; INSERT INTO @TABLE EXEC Lambo.VINLookup @LanguageID = 2, @Vin = '{0}'; SELECT @VIN = VIN FROM @TABLE; SET @ROWCOUNT = @@ROWCOUNT; IF @ROWCOUNT = 0 INSERT INTO @RESULTS (VIN) VALUES(@VIN);", vin));
                //writer.WriteLine(string.Format("EXEC Lambo.VINLookup @LanguageID = 2, @Vin = '{0}'; IF @@ROWCOUNT = 0 PRINT '{0}';", vin));
            }
        }
    }
}
