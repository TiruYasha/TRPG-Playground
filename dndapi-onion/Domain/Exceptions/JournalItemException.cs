using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class JournalItemException : Exception
    {
        public JournalItemException(string message) : base(message)
        {
        }
    }
}
