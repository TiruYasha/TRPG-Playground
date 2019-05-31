namespace Domain.Dto.ReturnDto.Chat.CommandResults
{
    public class UnrecognizedCommandResult: CommandResult
    {
        public UnrecognizedCommandResult()
        {
            Type = global::Domain.Domain.Commands.CommandType.UnrecognizedCommand;
        }
    }
}
