using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dto.RequestDto
{
    public class AddMapDto
    {
        public string Name { get; set; }
        public int WidthInPixels { get; set; }
        public int HeightInPixels { get; set; }
        public int GridSizeInPixels { get; set; }
    }
}
