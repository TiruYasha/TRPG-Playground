using System;

namespace RestApi.Models.Chat.CommandResults
{
    public class CommandResult
    {
        public Guid Id { get; set; }
        public CommandType Type { get; set; }
    }
}
