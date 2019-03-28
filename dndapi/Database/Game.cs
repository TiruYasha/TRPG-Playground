using System;
using System.Collections.Generic;

namespace Database
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual DndUser Owner { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual List<Participant> Participants { get; set; }
        public virtual List<ChatMessage> Messages { get; set; }
        
        public Game()
        {
            Participants = new List<Participant>();
            Messages = new List<ChatMessage>();
        }
    }
}
