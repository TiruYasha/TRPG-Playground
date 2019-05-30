using Domain.Domain.Commands;

namespace Domain.Dto.ReturnDto.Chat.CommandResults
{
    public class DefaultCommandResult : CommandResult
    {
        public DefaultCommandResult()
        {
            Type = CommandType.Default;
        }
    }
}
