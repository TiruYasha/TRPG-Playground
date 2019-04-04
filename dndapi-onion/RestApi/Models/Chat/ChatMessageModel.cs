using System;
using System.Collections.Generic;
using System.Text;

namespace RestApi.Models.Chat
{
    public class ChatMessageModel
    {
        public string User { get; set; }
        public string Message { get; set; }
        //public CommandResultModel CommandResult { get; set; }
    }
}
