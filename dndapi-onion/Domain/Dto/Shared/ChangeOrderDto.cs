using System;

namespace Domain.Dto.Shared
{
    public class ChangeOrderDto
    {
        public int PreviousPosition { get; set; }
        public int NewPosition { get; set; }
    }
}
