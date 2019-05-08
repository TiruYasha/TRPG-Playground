using Domain.Domain.Commands;

namespace RestApi.Models.Chat.CommandResults
{
    public class CommandDoesNotExist : CommandResult
    {
        public CommandDoesNotExist()
        {
            this.Type = CommandType.UnrecognizedCommand;
        }
    }
}
