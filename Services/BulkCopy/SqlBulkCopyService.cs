namespace Services.BulkCopy
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Domain.Utilities;

    public class SqlBulkCopyService // : IBulkCopyService
    {
        // http://blog.developers.ba/bulk-insert-generic-list-sql-server-minimum-lines-code/
        // http://www.codinghelmet.com/?path=howto/bulk-insert

        // drop test table
        // recreate test table from original - Select * Into lambo.@TableName From lambo.Vehicles Where 1 = 2
        // bulk insert into test table
        // merge from test table to live table - insert only new, update existing
        // drop test table


        //public void InsertAll<T>(IEnumerable<T> data, string connectionString, string tableName)
        //{
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        using (var transaction = connection.BeginTransaction())
        //        {
        //            using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
        //            {
        //                bulkCopy.BatchSize = 100;
        //                bulkCopy.DestinationTableName = tableName;

        //                try
        //                {
        //                    bulkCopy.WriteToServer(ReflectionUtility.GetDataTableByReflection<T>(data));
        //                    transaction.Commit();
        //                }
        //                catch (Exception)
        //                {
        //                    transaction.Rollback();
        //                    connection.Close();
        //                    throw;
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
