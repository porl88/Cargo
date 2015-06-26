namespace Domain.Classes.CsvMapper
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;
	using Domain.Utilities;

	public class CsvMapper<T> where T : class
	{
		private char[] delimiters = { ',', '\t', ';', '|', '^' };

		private char delimiter;

		private int columns;

		private int rowIndex;

		public CsvMapper()
		{
			this.Log = new List<LogEntry>();
		}

		public delegate V CsvMapperHandler<V>(object sender, CsvMapperEventArgs e);

		public event CsvMapperHandler<T> OnItemDataBound;

		public List<LogEntry> Log { get; private set; }

		public string[] GetHeaders(string filePath)
		{
			var headerRow = File.ReadLines(filePath).FirstOrDefault();
			return this.GetHeaderFromCsvLine(headerRow);
		}

		public string[] GetHeadersLegacy(string filePath)
		{
			using (var reader = File.OpenText(filePath))
			{
				var headerRow = reader.ReadLine();
				return this.GetHeaderFromCsvLine(headerRow);
			}
		}

		public string[] GetHeaders(Stream fileStream)
		{
			using (var reader = new StreamReader(fileStream))
			{
				var headerRow = reader.ReadLine();
				return this.GetHeaderFromCsvLine(headerRow);
			}
		}

		public List<T> Map(string filePath, NameValueCollection mappings = null)
		{
			var rows = File.ReadLines(filePath);
			var headerRow = rows.FirstOrDefault();
			this.InitialiseMapping(headerRow);
			return rows.Skip(1).Select((x, index) => this.ProcessRow(x, index, mappings)).Where(x => x != null).ToList();
		}

		public List<T> MapLegacy(string filePath, NameValueCollection mappings = null)
		{
			int index = 1;
			var rows = new List<T>();

			using (var reader = File.OpenText(filePath))
			{
				var headerRow = reader.ReadLine();
				this.InitialiseMapping(headerRow);

				while (!reader.EndOfStream)
				{
					var row = reader.ReadLine();
					rows.Add(this.ProcessRow(row, index++, mappings));
				}
			}

			return rows;
		}

		public List<T> Map(Stream fileStream, NameValueCollection mappings = null)
		{
			int index = 1;
			var rows = new List<T>();

			using (var reader = new StreamReader(fileStream))
			{
				var headerRow = reader.ReadLine();
				this.InitialiseMapping(headerRow);

				while (!reader.EndOfStream)
				{
					var row = reader.ReadLine();
					rows.Add(this.ProcessRow(row, index++, mappings));
				}
			}

			return rows;
		}

		private char GetDelimiter(string row)
		{
			var delimiter = this.delimiters.OrderByDescending(x => row.Split(x).Count()).FirstOrDefault();
			if (delimiter != default(char))
			{
				return delimiter;
			}
			else
			{
				throw new IOException("A valid delimiter cannot be found.");
			}
		}

		private string[] GetHeaderFromCsvLine(string headerRow)
		{
			var delimiter = this.GetDelimiter(headerRow);
			return headerRow.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
		}

		private T ProcessRow(string row, int index, NameValueCollection mappings)
		{
			this.rowIndex = index;

			try
			{
				var fields = this.GetFieldsFromCsvLine(row);

				var item = this.MapToClassInstance(fields, mappings);

				if (this.ValidateClassInstance(item))
				{
					return item;
				}
			}
			catch (Exception ex)
			{
				this.Log.Add(new LogEntry
				{
					ErrorType = ErrorType.Error,
					Message = ex.Message,
					RowCount = this.rowIndex,
					Value = row
				});
			}

			return null;
		}

		private string[] GetFieldsFromCsvLine(string csvLine)
		{
			// convert excel to unicode CSV
			// https://www.ablebits.com/office-addins-blog/2014/04/24/convert-excel-csv/#export-csv-utf8
			var fields = csvLine.Split(this.delimiter).Select(x => x.Trim(' ', '"')).ToArray();

			if (fields.Length == this.columns || !csvLine.Contains('"'))
			{
				return fields;
			}
			else
			{
				// make sure any instances of the separator within double quotes are treated as literal text - requires the double quotes to be done properly
				var sanitisedRow = Regex.Replace(csvLine, "\"[^\"]{2,}\"", match => match.Value.Replace(this.delimiter, '¬'));
				return sanitisedRow.Split(this.delimiter).Select(x => x.Trim(' ', '"').Replace('¬', this.delimiter)).ToArray();
			}
		}

		private bool ValidateClassInstance(T item)
		{
			// http://odetocode.com/blogs/scott/archive/2011/06/29/manual-validation-with-data-annotations.aspx
			ICollection<ValidationResult> results;
			if (DataAnnotationsUtility.TryValidate(item, out results))
			{
				return true;
			}
			else
			{
				return this.LogValidationErrors(results);
			}
		}

		private bool LogValidationErrors(ICollection<ValidationResult> results)
		{
			var isValid = true;

			foreach (var error in results)
			{
				if (error.ErrorMessage.Contains("required"))
				{
					isValid = false;
				}

				this.Log.Add(new LogEntry
				{
					ErrorType = isValid ? ErrorType.Warning : ErrorType.Error,
					RowCount = this.rowIndex,
					ColumnName = string.Join(", ", error.MemberNames),
					Value = isValid ? string.Join(", ", error.GetType().CustomAttributes) : string.Empty,
					Message = error.ErrorMessage
				});
			}

			return isValid;
		}

		private void InitialiseMapping(string headerRow)
		{
			this.delimiter = this.GetDelimiter(headerRow);
			this.columns = headerRow.Length;
		}

		private T MapToClassInstance(string[] fields, NameValueCollection mappings)
		{
			T item;

			if (mappings == null)
			{
				var csvMapEvent = new CsvMapperEventArgs(fields, this.rowIndex);
				item = this.OnItemDataBound(this, csvMapEvent);
				this.Log.AddRange(csvMapEvent.Log);
			}
			else
			{
				item = this.BindMappingsByReflection(fields, mappings);
			}

			return item;
		}

		private T BindMappingsByReflection(string[] fields, NameValueCollection mappings)
		{
			var type = typeof(T);
			var instance = (T)Activator.CreateInstance(type);
			var properties = TypeDescriptor.GetProperties(type);

			foreach (PropertyDescriptor prop in properties)
			{
				try
				{
					int columnNo;
					if (int.TryParse(mappings[prop.Name], out columnNo))
					{
						var fieldValue = fields[columnNo];
						if (!string.IsNullOrWhiteSpace(fieldValue))
						{
							var propertyInfo = instance.GetType().GetProperty(prop.Name);
							var propertyType = propertyInfo.PropertyType;
							fieldValue = this.ParseValue(propertyInfo.PropertyType, fieldValue);
							propertyInfo.SetValue(instance, prop.Converter.ConvertFromString(fieldValue), null);
						}
					}
				}
				catch (Exception ex)
				{
					this.Log.Add(new LogEntry
					{
						ErrorType = ErrorType.Error,
						Message = ex.Message,
						RowCount = this.rowIndex,
						Value = mappings[prop.Name]
					});
				}
			}

			return instance;
		}

		private string ParseValue(Type propertyType, string fieldValue)
		{
			if (propertyType == typeof(float) || propertyType == typeof(double) || propertyType == typeof(decimal))
			{
				fieldValue = Regex.Match(fieldValue, @"\d+(\.\d+)?").Value;
			}
			else if (propertyType == typeof(byte) || propertyType == typeof(short) || propertyType == typeof(int) || propertyType == typeof(long))
			{
				fieldValue = Regex.Match(fieldValue, @"\d+").Value;
			}

			return fieldValue;
		}
	}
}
