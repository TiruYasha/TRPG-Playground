using RestApi.Models.Chat.CommandResults;

namespace RestApi.Models.Chat
{
    public class ReceiveMessageModel
    {
        public string Message { get; set; }
        public string CustomUsername { get; set; }
        public CommandResult CommandResult { get; set; }
    }
}
