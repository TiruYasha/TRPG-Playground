using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Commands
{
    public static class CommandFactory
    {
        public static Command Create(string message)
        {
            var commandText = GetStringBeforeFirstSpace(message);

            switch (commandText)
            {
                case "/r":
                    return new NormalDiceRollCommand(commandText);
                default:
                    return new DefaultCommand();
            }
        }

        private static string GetStringBeforeFirstSpace(string message)
        {
            var splitText = message.Split(' ');
            return splitText[0];
        }
    }
}
