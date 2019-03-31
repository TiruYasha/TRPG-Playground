using Domain.Domain;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly DndContext context;

        public UserRepository(DbContextOptions<DndContext> options)
        {
            this.context = new DndContext(options);
        }

        public Task<User> GetUserByIdAsync(Guid userId)
        {
            return context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
