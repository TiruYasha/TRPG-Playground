using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dto.Shared
{
    public class JournalItemPermissionDto
    {
        public Guid UserId { get; set; }
        public bool CanSee { get; set; }
        public bool CanEdit { get; set; }
    }
}
