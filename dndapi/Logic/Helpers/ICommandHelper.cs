using System;
using System.Collections.Generic;
using System.Text;
using Database;

namespace Logic.Helpers
{
    public interface ICommandHelper
    {
        bool CheckIfMessageIsCommand(string message);
        CommandResult RunCommand(string message);
    }
}
