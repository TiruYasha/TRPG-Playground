using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RequestModels.Journal
{
    public class AddJournalFolderModel
    {
        public string Name { get; set; }
        public Guid ParentFolderId { get; set; }
        public Guid GameId { get; set; }
    }
}
