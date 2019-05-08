using System;

namespace Domain.Exceptions
{
    public class JournalItemException : Exception
    {
        public JournalItemException(string message) : base(message)
        {
        }
    }
}
