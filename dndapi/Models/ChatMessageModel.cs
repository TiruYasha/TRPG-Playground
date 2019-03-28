using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ChatMessageModel
    {
        public string User { get; set; }
        public string Message { get; set; }
        public CommandResultModel CommandResult { get; set; } 
    }
}
