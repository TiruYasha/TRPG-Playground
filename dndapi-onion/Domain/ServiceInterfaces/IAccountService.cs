using Domain.Domain;
using System;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
    public interface IAccountService
    {
        Task RegisterAsync(string email, string password);
        Task<string> LoginAsync(string email, string password);
    }
}
