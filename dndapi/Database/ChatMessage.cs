using System;

namespace Database
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual CommandResult CommandResult { get; set; }
        public virtual DndUser User { get; set; }
        public virtual Game Game { get; set; }
    }
}
