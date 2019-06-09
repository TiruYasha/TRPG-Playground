using System;

namespace Domain.Domain.JournalItems
{
    public class JournalItemPermission
    {
        public Guid JournalItemId { get; set; }
        public virtual JournalItem JournalItem { get; set; }

        public Guid UserId { get; set; }
        public Guid GameId { get; set; }
        public virtual GamePlayer GamePlayer { get; set; }

        public bool CanSee { get; set; }
        public bool CanEdit { get; set; }

        public JournalItemPermission() { }

        public JournalItemPermission(Guid journalItemId, Guid userId, Guid gameId, bool canSee, bool canEdit = false)
        {
            //TODO argument handling
            JournalItemId = journalItemId;
            UserId = userId;
            GameId = gameId;

            if (canEdit)
            {
                canSee = true;
            }

            CanSee = canSee;
            CanEdit = canEdit;
        }
    }
}
