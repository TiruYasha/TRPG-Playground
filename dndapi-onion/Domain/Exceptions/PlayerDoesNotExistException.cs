using System;

namespace Domain.Exceptions
{
    public class PlayerDoesNotExistException : Exception
    {
        public PlayerDoesNotExistException(string message) : base(message)
        {
        }
    }
}
