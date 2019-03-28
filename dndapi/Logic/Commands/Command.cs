using System;
using System.Collections.Generic;
using System.Text;
using Database;

namespace Logic.Commands
{
    public abstract class Command
    {
        private readonly string _arguments;

        public Command(string arguments)
        {
            _arguments = arguments;
        }

        protected string GetArguments()
        {
            return _arguments;
        }

        public abstract CommandResult Run();
    }
}
