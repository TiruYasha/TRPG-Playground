using System;

namespace Domain.Domain.JournalItems
{
    public class JournalItemPemission
    {
        public Guid JournalItemId { get; set; }
        public virtual JournalItem JournalItem { get; set; }

        public Guid UserId { get; set; }
        public Guid GameId { get; set; }
        public virtual GamePlayer Player { get; set; }

        public bool CanSee { get; set; }
        public bool CanEdit { get; set; }
    }
}
