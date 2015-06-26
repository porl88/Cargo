namespace Domain.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.SqlClient;
    using System.Web;

    public static class SqlUtility
    {
        public static T GetValue<T>(object columnValue)
        {
            var type = typeof(T);
            var converter = TypeDescriptor.GetConverter(type);
            return (T)converter.ConvertFromString(columnValue.ToString());
        }

        public static string SqlEncode(string field)
        {
            field = HttpUtility.HtmlDecode(field.Trim());
            return string.IsNullOrWhiteSpace(field) ? null : field.Replace("'", "''");
        }

        public static void BulkInsert<T>(IEnumerable<T> data, string connectionString, string tableName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.BatchSize = 100;
                        bulkCopy.DestinationTableName = tableName;

                        try
                        {
                            bulkCopy.WriteToServer(ReflectionUtility.GetDataTableByReflection<T>(data));
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            connection.Close();
                            throw;
                        }
                    }
                }
            }
        }
    }
}
