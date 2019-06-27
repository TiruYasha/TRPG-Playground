using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dto.Shared
{
    public class MapDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int GridSizeInPixels { get; set; }
        public int HeightInPixels { get; set; }
        public int WidthInPixels { get; set; }
    }
}
