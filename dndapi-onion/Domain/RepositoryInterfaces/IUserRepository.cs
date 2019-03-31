using Domain.Domain;
using System;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid userId);
    }
}
