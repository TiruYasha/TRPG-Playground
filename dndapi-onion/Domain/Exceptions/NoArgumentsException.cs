using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class NoArgumentsException : Exception
    {
        public NoArgumentsException(string message) : base(message)
        {
        }
    }
}
