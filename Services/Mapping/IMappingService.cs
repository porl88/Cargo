namespace Services.Mapping
{
    using System.Collections.Generic;
    using System.Collections.Specialized;

	public interface IMappingService
	{
		string[] GetHeaders(string filePath);

        Dictionary<string, string> GetFieldLabels();

		MappingResponse Map(List<string> filePaths, NameValueCollection mappings);
	}
}
