namespace RestApi.Models.Chat.CommandResults
{
    public class UnrecognizedCommandResult: CommandResult
    {
        public UnrecognizedCommandResult()
        {
            Type = Domain.Domain.Commands.CommandType.UnrecognizedCommand;
        }
    }
}
