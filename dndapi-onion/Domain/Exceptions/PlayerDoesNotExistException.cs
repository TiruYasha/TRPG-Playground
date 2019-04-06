using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class PlayerDoesNotExistException : Exception
    {
        public PlayerDoesNotExistException(string message) : base(message)
        {
        }
    }
}
