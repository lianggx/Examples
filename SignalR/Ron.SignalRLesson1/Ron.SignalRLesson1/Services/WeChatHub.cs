using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ron.SignalRLesson1.Services
{
    public class WeChatHub : Hub
    {
        public void Send(MessageBody body)
        {
            Clients.All.SendAsync("Recv", body);
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("哇，有人进来了：{0}", this.Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("靠，有人跑路了：{0}", this.Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }

    public class MessageBody
    {
        public int Type { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
    }
}
