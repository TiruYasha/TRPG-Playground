﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Commands
{
    public class DefaultCommand : Command
    {
        public DefaultCommand() : base(CommandType.Default)
        {
            
        }
    }
}