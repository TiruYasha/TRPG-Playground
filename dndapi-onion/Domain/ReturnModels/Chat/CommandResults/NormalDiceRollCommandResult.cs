using Domain.Domain.Commands;

namespace Domain.ReturnModels.Chat.CommandResults
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
