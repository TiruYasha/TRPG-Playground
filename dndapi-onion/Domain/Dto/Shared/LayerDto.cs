using System;
using System.Collections.Generic;
using System.Text;
using Domain.Domain;
using Domain.Domain.Layers;

namespace Domain.Dto.Shared
{
    public class LayerDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public LayerType Type { get; set; }
        public Guid MapId { get; set; }
    }
}
