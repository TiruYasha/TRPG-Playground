using System;

namespace Domain.Dto.ReturnDto.Journal
{
    public class UploadedImageDto
    {
        public Guid JournalItemId { get; set; }
        public Guid ImageId { get; set; }
    }
}
