using Domain.Domain;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IGameRepository
    {
        Task CreateGameAsync(Game game);
    }
}
