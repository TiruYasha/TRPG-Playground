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
        public int Order { get; set; }
        public LayerType Type { get; set; }
        public Guid MapId { get; set; }
        public Guid? LayerGroupId { get; set; }
        public ICollection<LayerDto> Layers { get; set; }

        public LayerDto()
        {
            Layers = new List<LayerDto>();
        }
    }
}
