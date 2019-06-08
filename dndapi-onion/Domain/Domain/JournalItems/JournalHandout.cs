using System;
using System.Threading.Tasks;
using Domain.Dto.RequestDto.Journal;
using Domain.Dto.Shared;

namespace Domain.Domain.JournalItems
{
    public class JournalHandout : JournalItem
    {
        public string Description { get; set; }
        public string OwnerNotes { get; set; }

        private JournalHandout() { }

        public JournalHandout(AddJournalItemDto dto, Guid gameId) : base(JournalItemType.Handout, dto.JournalItem.Name, gameId, dto.JournalItem.CanSee, dto.JournalItem.CanEdit)
        {
            var handoutModel = dto.JournalItem as JournalHandoutDto;
            Description = handoutModel.Description;
            OwnerNotes = handoutModel.OwnerNotes;
        }

        public override Task Update(JournalItemDto dto)
        {
            return Task.Run(async () =>
            {
                var handoutDto = dto as JournalHandoutDto;

                await base.Update(dto);

                Description = handoutDto.Description;
                OwnerNotes = handoutDto.OwnerNotes;
            });
        }
    }
}
