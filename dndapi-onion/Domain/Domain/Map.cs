using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Domain.Layers;
using Domain.Dto.Shared;

namespace Domain.Domain
{
    public class Map
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int WidthInPixels { get; private set; }
        public int HeightInPixels { get; private set; }
        public int GridSizeInPixels { get; private set; }
        public Guid GameId { get; private set; }
        public virtual Game Game { get; private set; }
        public virtual ICollection<Layer> Layers { get; private set; }

        private Map()
        {
            Layers = new List<Layer>();
        }

        public Map(MapDto dto) : this()
        {
            CheckArguments(dto);

            Id = Guid.NewGuid();
            Name = dto.Name;
            WidthInPixels = dto.WidthInPixels;
            HeightInPixels = dto.HeightInPixels;
            GridSizeInPixels = dto.GridSizeInPixels;
            var layerDto = new LayerDto
            {
                Name = "Layer1",
                Order = 0
            };
            Layers = new List<Layer> { new Layer(layerDto, Guid.NewGuid()) };
        }

        public Task Update(MapDto dto)
        {
            return Task.Run(() =>
            {
                CheckArguments(dto);

                Name = dto.Name;
                WidthInPixels = dto.WidthInPixels;
                HeightInPixels = dto.HeightInPixels;
                GridSizeInPixels = dto.GridSizeInPixels;
            });
        }

        public async Task<Layer> AddLayer(LayerDto dto)
        {
            var layer = await LayerFactory.Create(dto, Id);

            Layers.Add(layer);
            return layer;
        }

        private static void CheckArguments(MapDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Name may not be empty");
            }

            if (dto.WidthInPixels > 4000)
            {
                throw new ArgumentException("Width may not be larger than 4000 pixels");
            }

            if (dto.WidthInPixels < 0)
            {
                throw new ArgumentException("Width may not be smaller than 0 pixels");
            }

            if (dto.HeightInPixels > 4000)
            {
                throw new ArgumentException("Height may not be larger than 4000 pixels");
            }

            if (dto.HeightInPixels < 0)
            {
                throw new ArgumentException("Height may not be smaller than 0 pixels");
            }

            if (dto.GridSizeInPixels < 0)
            {
                throw new ArgumentException("Grid size may not be smaller than 0 pixels");
            }
        }
    }
}
