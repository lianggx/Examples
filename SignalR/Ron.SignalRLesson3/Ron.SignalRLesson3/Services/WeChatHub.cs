using Microsoft.AspNetCore.SignalR;
using Ron.SignalRLesson3.Models;
using System;
using System.Threading.Tasks;

namespace Ron.SignalRLesson3.Services
{
    public class WeChatHub : Hub<IHeartbeat>
    {
        public void Send(ChatMessage body)
        {
            Clients.All.RecvAsync(body);
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("游客[{0}]进入了聊天室", this.Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("游客[{0}]离开了聊天室", this.Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }

    public interface IHeartbeat
    {
        Task RecvAsync(object arg);

        Task HeartbeatAsync(int data);
    }
}
