using System;
using System.Threading.Tasks;
using Domain.Dto.Shared;

namespace Domain.Domain.JournalItems
{
    public class JournalHandout : JournalItem
    {
        public string Description { get; private set; }
        public string OwnerNotes { get; private set; }

        private JournalHandout() { }

        public JournalHandout(JournalItemDto dto, Guid gameId) : base(dto, gameId)
        {
            var handoutModel = dto as JournalHandoutDto;
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
