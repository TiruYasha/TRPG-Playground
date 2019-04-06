using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class CommandDoesNotExistException : Exception
    {
        public CommandDoesNotExistException(string message) : base(message)
        {
        }
    }
}
