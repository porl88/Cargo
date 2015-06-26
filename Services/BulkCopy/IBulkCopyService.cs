namespace Services.BulkCopy
{
    using System.Collections.Generic;

    public interface IBulkCopyService
    {
        void InsertAll<T>(IEnumerable<T> data, string connectionString, string tableName);
    }
}
