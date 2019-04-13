using System;
using System.Collections.Generic;
using System.Text;

namespace RestApi.Models.Journal
{
    public class AddedJournalFolderModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }
    }
}
