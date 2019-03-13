using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ron.SignalRServer.BLL
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Message msg)
        {
            await Clients.All.SendAsync("ReceiveMessage", msg.UserName, msg.Content);
            
        }
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("UserId：{0} Connected", this.Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("UserId：{0} Disconnected", this.Context.ConnectionId);
            if (exception != null)
            {
                Console.WriteLine("{0} | {1}", exception.Message, exception.StackTrace);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }

    public class Message
    {
        public string UserName { get; set; }
        public string Content { get; set; }
    }
}
