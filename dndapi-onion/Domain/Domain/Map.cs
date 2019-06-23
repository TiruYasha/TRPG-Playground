using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Dto.RequestDto;
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
        public virtual ICollection<Layer> Layers { get; private set; }

        private Map()
        {
            // for ef
        }

        public Map(AddMapDto dto) : this()
        {
            CheckArguments(dto);

            Id = Guid.NewGuid();
            Name = dto.Name;
            WidthInPixels = dto.WidthInPixels;
            HeightInPixels = dto.HeightInPixels;
            GridSizeInPixels = dto.GridSizeInPixels;
            Layers = new List<Layer> { new Layer("layer1") };
        }

        public Task Update(AddMapDto dto)
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

        private static void CheckArguments(AddMapDto dto)
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
