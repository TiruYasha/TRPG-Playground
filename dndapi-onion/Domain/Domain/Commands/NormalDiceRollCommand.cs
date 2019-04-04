using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Commands
{
    public class NormalDiceRollCommand : Command
    {
        public NormalDiceRollCommand() : base(CommandType.NormallDiceRoll)
        {

        }
    }
}
