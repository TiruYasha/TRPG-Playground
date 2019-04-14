using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Commands
{
    public static class CommandFactory
    {
        public static Command Create(string message)
        {
            if (!IsCommand(message))
            {
                return new DefaultCommand();
            }

            (string command, string arguments) = ParseCommandAndArguments(message);

            switch (command)
            {
                case "/r":
                    return new NormalDiceRollCommand(arguments);
                default:
                    throw new CommandDoesNotExistException($"Unrecognized command: {message}");
            }
        }

        private static bool IsCommand(string message)
        {
            return message.StartsWith('/');
        }

        private static (string command, string arguments) ParseCommandAndArguments(string message)
        {
            var splitText = message.Split(' ', 2);

            if(splitText.Length < 2)
            {
                throw new NoArgumentsException("Please provide arguments");
            }

            return (splitText[0], splitText[1]);
        }
    }
}
