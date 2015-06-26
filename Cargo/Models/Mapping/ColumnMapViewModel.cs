namespace Cargo.Models.Mapping
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	public class ColumnMapViewModel
	{
		public string[] Columns { get; set; }

		public Dictionary<string, string> FieldLabels { get; set; }
	}
}