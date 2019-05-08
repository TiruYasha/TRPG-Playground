using Domain.Domain.Commands;

namespace RestApi.Models.Chat.CommandResults
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
