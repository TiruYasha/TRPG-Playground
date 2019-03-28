using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public abstract class CommandResult
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
    }
}
