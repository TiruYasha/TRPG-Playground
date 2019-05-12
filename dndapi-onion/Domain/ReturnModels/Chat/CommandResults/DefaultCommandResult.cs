using Domain.Domain.Commands;

namespace Domain.ReturnModels.Chat.CommandResults
{
    public class DefaultCommandResult : CommandResult
    {
        public DefaultCommandResult()
        {
            Type = CommandType.Default;
        }
    }
}
