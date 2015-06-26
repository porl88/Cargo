namespace Services.Commands
{
	public interface ISqlBulkInsertCommand : ICommand
	{
		string SqlFilePath { get; }
	}
}
