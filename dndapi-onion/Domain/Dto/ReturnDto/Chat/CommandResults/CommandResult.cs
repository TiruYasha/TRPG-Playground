using System;
using Domain.Domain.Commands;

namespace Domain.Dto.ReturnDto.Chat.CommandResults
{
    public class CommandResult
    {
        public Guid Id { get; set; }
        public CommandType Type { get; set; }
    }
}
