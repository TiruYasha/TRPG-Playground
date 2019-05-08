using System;

namespace Domain.Exceptions
{
    public class NoArgumentsException : Exception
    {
        public NoArgumentsException(string message) : base(message)
        {
        }
    }
}
