using System;

namespace Logic.Exceptions
{
    public class GameNotFoundException : Exception
    {
        public GameNotFoundException(string message) : base(message)
        {
        }
    }
}