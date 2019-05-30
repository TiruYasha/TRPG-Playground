namespace Domain.Dto.Shared
{
    public class JournalHandoutDto : JournalItemDto
    {
        public string Description { get; set; }
        public string OwnerNotes { get; set; }
        public JournalHandoutDto()
        {
            Type = Domain.JournalItems.JournalItemType.Handout;
        }
    }
}
