using Domain.Domain.Commands;
using System;

namespace Domain.Domain
{
    public class ChatMessage
    {
        public Guid Id { get; private set; }
        public string Message { get; private set; }
        public string CustomUsername { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public virtual Command Command { get; private set; }
        public virtual User User { get; private set; }
        public virtual Game Game { get; private set; }

        public ChatMessage()
        {
        }

        public ChatMessage(string message, string customUsername, User user, Game game)
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
            User = user;
            Game = game;

            Message = message;
            CustomUsername = ParseCustomUsername(customUsername, user);
            Command = CommandFactory.Create(message);

            Command.Execute();
        }

        private static string ParseCustomUsername(string customUsername, User user)
        {
            return string.IsNullOrEmpty(customUsername) ? user.UserName : customUsername;
        }
    }
}
