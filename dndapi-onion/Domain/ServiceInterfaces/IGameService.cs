using Domain.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
    public interface IGameService
    {
        Task CreateGameAsync(string gameName, Guid ownerId);
    }
}
