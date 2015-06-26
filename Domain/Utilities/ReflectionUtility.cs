namespace Domain.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;

    public static class ReflectionUtility
    {
        public static Dictionary<string, string> GetPropertiesByReflection(Type mappedToClassType)
        {
            var properties = new Dictionary<string, string>();

            foreach (var i in mappedToClassType.GetProperties())
            {
                var displayNameAttr = (DisplayNameAttribute)i.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault();
                properties.Add(i.Name, displayNameAttr == null ? i.Name : displayNameAttr.DisplayName);
            }

            return properties;
        }

        public static DataTable GetDataTableByReflection<T>(IEnumerable<T> data)
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
