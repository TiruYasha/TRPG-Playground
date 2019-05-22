using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class FileException : Exception
    {
        public FileException(string message) : base(message)
        {
        }
    }
}
