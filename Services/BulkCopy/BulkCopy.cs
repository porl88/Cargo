namespace Services.BulkCopy
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Data.SqlClient;
	using Domain.Utilities;

	public class BulkCopy
	{
		private SqlConnection connection;

		private SqlTransaction transaction;

		private string destinationTable;

		public void Process()
		{
			var data = new List<string>();
			data = this.EnrichData<string>(data).ToList();
			this.BulkInsert(data, "ConnectionString", "TableName");
		}

		public void BulkInsert<T>(IEnumerable<T> data, string connectionString, string tableName)
		{
			this.destinationTable = tableName;

			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();

				using (var transaction = connection.BeginTransaction())
				{
					var sourceTableName = tableName + "_xxx_test";

					try
					{
						this.CreateTemporaryTable(sourceTableName, connection, transaction);
						this.BulkInsertDataIntoTemporaryTable(connection, transaction, sourceTableName, data);
						this.MergeTemporaryDataIntoTable("StoredProcedure", connection, transaction);
						this.DeleteTemporaryTable(sourceTableName, connection, transaction);
						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
						connection.Close();
						throw;
					}
				}
			}
		}

		private IEnumerable<T> EnrichData<T>( IEnumerable<T> data)
		{
			return data;
		}

		private void MergeTemporaryDataIntoTable(string commandText, SqlConnection connection, SqlTransaction transaction)
		{
			this.ExecCommand(commandText, connection, transaction);
		}

		private void CreateTemporaryTable(string sourceTableName, SqlConnection connection, SqlTransaction transaction)
		{
			var commandText = string.Format("DROP {0}; SELECT * FROM {1} WHERE 1 = 2;", sourceTableName, this.destinationTable);
			this.ExecCommand(commandText, connection, transaction);
		}

		private void DeleteTemporaryTable(string sourceTableName, SqlConnection connection, SqlTransaction transaction)
		{
			var commandText = string.Format("DROP {0}", sourceTableName);
			this.ExecCommand(commandText, connection, transaction);
		}

		private void ExecCommand(string commandText, SqlConnection connection, SqlTransaction transaction)
		{
			using (var command = new SqlCommand(commandText, connection, transaction))
			{
				command.ExecuteNonQuery();
			}
		}

		private void BulkInsertDataIntoTemporaryTable<T>(SqlConnection connection, SqlTransaction transaction, string sourceTableName, IEnumerable<T> data)
		{
			using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
			{
				bulkCopy.BatchSize = 100;
				bulkCopy.DestinationTableName = sourceTableName;
				bulkCopy.WriteToServer(this.GetDataTableByReflection<T>(data));
			}
		}


		//public void InsertAll<T>(IEnumerable<T> data, string connectionString, string tableName)
		//{
		//	using (var connection = new SqlConnection(connectionString))
		//	{
		//		connection.Open();

		//		using (var transaction = connection.BeginTransaction())
		//		{
		//			using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
		//			{
		//				bulkCopy.BatchSize = 100;
		//				bulkCopy.DestinationTableName = tableName;

		//				try
		//				{
		//					bulkCopy.WriteToServer(ReflectionUtility.GetDataTableByReflection<T>(data));
		//					transaction.Commit();
		//				}
		//				catch (Exception)
		//				{
		//					transaction.Rollback();
		//					connection.Close();
		//					throw;
		//				}
		//			}
		//		}
		//	}
		//}

		private DataTable GetDataTableByReflection<T>(IEnumerable<T> data)
		{
			var properties = TypeDescriptor.GetProperties(typeof(T));
			var table = new DataTable();

			foreach (PropertyDescriptor prop in properties)
			{
				var column = new DataColumn(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

				if (!string.IsNullOrWhiteSpace(prop.DisplayName))
				{
					column.Caption = prop.DisplayName;
				}

				table.Columns.Add(column);
			}

			foreach (T item in data)
			{
				var row = table.NewRow();

				foreach (PropertyDescriptor prop in properties)
				{
					row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
				}

				table.Rows.Add(row);
			}

			return table;
		}
	}
}


