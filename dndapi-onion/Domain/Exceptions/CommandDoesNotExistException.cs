using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class UnrecognizedCommandException : Exception
    {
        public UnrecognizedCommandException(string message) : base(message)
        {
        }
    }
}
