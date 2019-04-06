using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Commands
{
    public abstract class Command
    {
        public Guid Id { get; private set; }
        public CommandType Type { get; private set; }
        public string CommandText { get; protected set; }

        public Command()
        {

        }

        public Command(CommandType type, string commandText)
        {
            Id = Guid.NewGuid();
            Type = type;
            CommandText = commandText;
        }

        public abstract void Execute();
    }
}
