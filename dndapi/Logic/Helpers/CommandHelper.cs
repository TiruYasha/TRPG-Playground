using System;
using System.Collections.Generic;
using System.Text;
using Logic.Commands;
using Database;

namespace Logic.Helpers
{
    public class CommandHelper : ICommandHelper
    {
        public bool CheckIfMessageIsCommand(string message)
        {
            return message.StartsWith('/');
        }

        public CommandResult RunCommand(string message)
        {
            var command = CommandFactory.Create(message);

            CommandResult result = command.Run();

            return result;
        }
    }
}
