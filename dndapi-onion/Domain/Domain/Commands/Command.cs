using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Commands
{
    public abstract class Command
    {
        public Guid Id { get; set; }
        public CommandType Type { get; set; }

        public Command(CommandType type)
        {
            Id = Guid.NewGuid();
            Type = type;
        }

        public virtual void Execute(string message)
        {
            throw new MethodAccessException("This method should be implemented or not accessed");
        }
    }
}
