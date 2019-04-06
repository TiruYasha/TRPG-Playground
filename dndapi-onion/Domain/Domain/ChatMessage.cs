﻿using Domain.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Command Command { get; set; }
        public virtual User User { get; set; }
        public virtual Game Game { get; set; }

        public ChatMessage()
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
