using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Commands
{
    public static class CommandFactory
    {
        public static Command Create(string message)
        {
            var commands = message.Split(' ', 2);

            switch (commands[0])
            {
                case "/roll":
                    return new NormallRollCommand(commands[1]);
                default:
                    return null;
            }
        }
    }
}
