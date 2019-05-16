using Domain.ReturnModels.Chat.CommandResults;

namespace Domain.ReturnModels.Chat
{
    public class ReceiveMessageModel
    {
        public string Message { get; set; }
        public string CustomUsername { get; set; }
        public CommandResult CommandResult { get; set; }
    }
}
