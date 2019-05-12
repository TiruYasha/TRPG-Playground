namespace Domain.ReturnModels.Chat.CommandResults
{
    public class UnrecognizedCommandResult: CommandResult
    {
        public UnrecognizedCommandResult()
        {
            Type = global::Domain.Domain.Commands.CommandType.UnrecognizedCommand;
        }
    }
}
