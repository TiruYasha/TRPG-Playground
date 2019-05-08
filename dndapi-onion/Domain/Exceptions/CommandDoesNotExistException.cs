using System;

namespace Domain.Exceptions
{
    public class UnrecognizedCommandException : Exception
    {
        public UnrecognizedCommandException(string message) : base(message)
        {
        }
    }
}
