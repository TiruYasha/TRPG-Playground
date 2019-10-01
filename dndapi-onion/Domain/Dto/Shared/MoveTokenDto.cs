using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dto.Shared
{
    public class MoveTokenDto
    {
        public Guid Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
