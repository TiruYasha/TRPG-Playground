using Domain.Dto.ReturnDto.Chat.CommandResults;

namespace Domain.Dto.ReturnDto.Chat
{
    public class ReceiveMessageModel
    {
        public string Message { get; set; }
        public string CustomUsername { get; set; }
        public CommandResult CommandResult { get; set; }
    }
}
