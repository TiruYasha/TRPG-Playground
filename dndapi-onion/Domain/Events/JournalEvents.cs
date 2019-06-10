using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Events
{
    public static class JournalEvents
    {
        public static string JournalItemAdded = "JournalItemAdded";
        public static string JournalItemImageUploaded = "JournalItemImageUploaded";
        public static string JournalItemUpdated = "JournalItemUpdated";
        public static string JournalItemDeleted = "JournalItemDeleted";
        public static string JournalCharacterSheetTokenUploaded = "JournalCharacterSheetTokenUploaded";
    }
}
