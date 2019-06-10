using System;
using System.Threading.Tasks;
using Domain.Dto.Shared;

namespace Domain.Domain.JournalItems
{
    public class JournalCharacterSheet : JournalItem
    {
        public string Description { get; set; }
        public string OwnerNotes { get; set; }
        public virtual Image Token { get; set; }

        private JournalCharacterSheet() { }

        public JournalCharacterSheet(JournalItemDto dto, Guid gameId) : base(dto, gameId)
        {
            var journalCharacterSheetDto = dto as JournalCharacterSheetDto;
            Description = journalCharacterSheetDto.Description;
            OwnerNotes = journalCharacterSheetDto.OwnerNotes;
        }

        public override Task Update(JournalItemDto dto)
        {
            return Task.Run(async () =>
            {
                var journalCharacterSheetDto = dto as JournalCharacterSheetDto;

                await base.Update(dto);

                Description = journalCharacterSheetDto.Description;
                OwnerNotes = journalCharacterSheetDto.OwnerNotes;
            });
        }

        public Task<Image> SetToken(string extension, string originalName)
        {
            return Task.Run(() =>
            {
                var image = new Image(extension, originalName);

                Token = image;

                return image;
            });
        }
    }
}
