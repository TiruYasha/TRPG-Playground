using System;
using System.Collections.Generic;
using System.Text;

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
