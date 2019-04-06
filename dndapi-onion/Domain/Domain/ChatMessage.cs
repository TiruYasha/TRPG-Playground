using Domain.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain
{
    public class ChatMessage
    {
        public Guid Id { get; private set; }
        public string Message { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public virtual Command Command { get; private set; }
        public virtual User User { get; private set; }
        public virtual Game Game { get; private set; }

        private ChatMessage()
        {
            // For EF
        }

        public ChatMessage(string message, User user, Game game)
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
            User = user;
            Game = game;

            Message = message;

            Command = CommandFactory.Create(message);

            Command.Execute();
        }
    }
}
