using Domain.Domain.Commands;

namespace Domain.ReturnModels.Chat.CommandResults
{
    public class CommandDoesNotExist : CommandResult
    {
        public CommandDoesNotExist()
        {
            this.Type = CommandType.UnrecognizedCommand;
        }
    }
}
