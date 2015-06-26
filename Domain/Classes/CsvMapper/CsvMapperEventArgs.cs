namespace Domain.Classes.CsvMapper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    public class CsvMapperEventArgs : EventArgs
    {
        private string[] columns { get; set; }

        private int rowCount { get; set; }

        public CsvMapperEventArgs(string[] columns, int rowCount)
        {
            this.Log = new List<LogEntry>();
            this.columns = columns;
            this.rowCount = rowCount;
        }

        public List<LogEntry> Log { get; private set; }

        public T MapColumn<T>(string columnNo)
        {
            int colNo;
            if (int.TryParse(columnNo, out colNo))
            {
                return this.MapColumn<T>(colNo);
            }

            return default(T);
        }

        public T MapColumn<T>(int columnNo)
        {
            if (columnNo < this.columns.Length)
            {
                try
                {
                    var columnValue = this.columns[columnNo];
                    if (!string.IsNullOrWhiteSpace(columnValue))
                    {
                        var converter = TypeDescriptor.GetConverter(typeof(T));
                        return (T)converter.ConvertFromString(columnValue);
                    }
                }
                catch (Exception ex)
                {
                    this.Log.Add(new LogEntry()
                    {
                        ErrorType = ErrorType.Warning,
                        Value = this.columns[columnNo],
                        RowCount = this.rowCount,
                        Message = ex.Message
                    });
                }
            }

            return default(T);
        }

        public T ParseColumn<T>(string columnNo) where T : struct
        {
            int colNo;
            if (int.TryParse(columnNo, out colNo))
            {
                return this.ParseColumn<T>(colNo);
            }

            return default(T);
        }

        public T ParseColumn<T>(int columnNo) where T : struct
        {
            if (columnNo < this.columns.Length)
            {
                var columnValue = this.columns[columnNo];
                if (!string.IsNullOrWhiteSpace(columnValue))
                {
                    var type = typeof(T);
                    var converter = TypeDescriptor.GetConverter(type);

					if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
                    {
                        columnValue = Regex.Match(columnValue.Trim(), @"\d+(\.\d+)?").Value;
                    }
                    else if (type == typeof(byte) || type == typeof(short) || type == typeof(int) || type == typeof(long))
                    {
                        columnValue = Regex.Match(columnValue.Trim(), @"\d+").Value;
                    }

                    return (T)converter.ConvertFromString(columnValue);
                }
            }

            return default(T);
        }

        public T Match<T>(string columnNo, string regEx)
        {
            int colNo;
            if (int.TryParse(columnNo, out colNo))
            {
                return this.Match<T>(colNo, regEx);
            }

            return default(T);
        }

        public T Match<T>(int columnNo, string regEx)
        {
            if (columnNo < this.columns.Length)
            {
                var columnValue = this.columns[columnNo];
                if (!string.IsNullOrWhiteSpace(columnValue))
                {
                    var type = typeof(T);
                    var converter = TypeDescriptor.GetConverter(type);
                    columnValue = Regex.Match(columnValue, regEx).Value.Trim();
                    return (T)converter.ConvertFromString(columnValue);
                }
            }

            return default(T);
        }
    }
}
