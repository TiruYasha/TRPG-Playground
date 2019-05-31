using Domain.Domain.Commands;

namespace Domain.Dto.ReturnDto.Chat.CommandResults
{
    public class CommandDoesNotExist : CommandResult
    {
        public CommandDoesNotExist()
        {
            this.Type = CommandType.UnrecognizedCommand;
        }
    }
}
