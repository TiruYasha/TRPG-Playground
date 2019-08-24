using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Domain.PlayArea;
using Domain.Dto.Shared;

namespace Domain.Domain.Layers
{
    public class Layer
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public LayerType Type { get; private set; }
        public int Order { get; set; }
        public bool IsVisible { get; set; }
        public bool IsVisibleToPlayers { get; set; }
        public Guid MapId { get; private set; }
        public virtual Map Map { get; private set; }
        public virtual ICollection<Token> Tokens { get; private set; }

        protected Layer()
        {
            Tokens = new List<Token>();
        }

        public Layer(LayerDto dto, Guid mapId, LayerType type = LayerType.Default) : this()
        {
            CheckArguments(dto.Name);

            Id = Guid.NewGuid();
            Name = dto.Name;
            Order = dto.Order;
            Type = type;
            IsVisible = false;
            IsVisibleToPlayers = false;
            MapId = mapId;
        }

        public Task Update(string name)
        {
            return Task.Run(() =>
            {
                CheckArguments(name);
                Name = name;
            });
        }

        public Task<Layer> UpdateOrder(int order)
        {
            return Task.Run(() =>
            {
                Order = order;
                return this;
            });
        }

        public Task<Token> AddToken(TokenDto tokenDto)
        {

            return Task.Run(() =>
            {
                var token = TokenFactory.Create(tokenDto);
                Tokens.Add(token);
                return token;
            });
        }

        private static void CheckArguments(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name may not be empty");
            }
        }

        public Task ToggleVisibleToPlayers()
        {
            return Task.Run(() =>
            {
                this.IsVisibleToPlayers = !IsVisibleToPlayers;
            });
        }

        public Task ToggleVisible()
        {
            return Task.Run(() =>
            {
                this.IsVisible = !IsVisible;
            });
        }
    }
}
