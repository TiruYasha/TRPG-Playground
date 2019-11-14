using EventBusProvider.Models;
using System;
using System.Threading.Tasks;

namespace EventBusProvider
{
    public interface IBusProvider
    {
        string HostName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }

        Task OpenConnection();
        Task Publish();

        Task Consume();

        Task SendRpc(RpcMessage message, );
        Task ConsumeRpc();
    }
}
