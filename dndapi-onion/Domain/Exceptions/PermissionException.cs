using System;

namespace Domain.Exceptions
{
    public class PermissionException : Exception
    {
        public PermissionException(string message) : base(message)
        {
        }
    }
}
