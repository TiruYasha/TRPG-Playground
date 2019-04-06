using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RequestModels.Chat
{
    public class SendMessageModel
    {
        public Guid GameId { get; set; }
        public string CustomUsername { get; set; }
        public string Message { get; set; }
    }
}
