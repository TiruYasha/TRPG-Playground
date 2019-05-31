using System;

namespace Domain.Dto.RequestDto.Chat
{
    public class SendMessageModel
    {
        public Guid GameId { get; set; }
        public string CustomUsername { get; set; }
        public string Message { get; set; }
    }
}
