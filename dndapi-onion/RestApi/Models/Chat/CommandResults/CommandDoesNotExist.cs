using Domain.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

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
