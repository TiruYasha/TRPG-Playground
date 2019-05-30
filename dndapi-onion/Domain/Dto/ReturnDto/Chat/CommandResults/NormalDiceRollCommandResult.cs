using Domain.Domain.Commands;

namespace Domain.Dto.ReturnDto.Chat.CommandResults
{
    public class NormalDiceRollCommandResult : CommandResult
    {
        public NormalDiceRollCommandResult()
        {
            Type = CommandType.NormallDiceRoll;
        }

        public int RollResult { get; set; }
    }
}
