using System;

namespace Domain.Domain.Commands
{
    public abstract class Command
    {
        public Guid Id { get; private set; }
        public CommandType Type { get; private set; }
        public string CommandText { get; protected set; }

        protected Command()
        {

        }

        protected Command(CommandType type, string commandText)
        {
            Id = Guid.NewGuid();
            Type = type;
            CommandText = commandText;
        }

        public abstract void Execute();
    }
}
