using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ReturnModels.Journal
{
    public class UploadedImageDto
    {
        public Guid JournalItemId { get; set; }
        public Guid ImageId { get; set; }
    }
}
