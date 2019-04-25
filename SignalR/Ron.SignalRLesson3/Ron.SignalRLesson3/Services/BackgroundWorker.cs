using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ron.SignalRLesson3.Services
{
    public class WeChatHubWorker : BackgroundService
    {
        private readonly IHubContext<WeChatHub, IHeartbeat> heartbeat;
        public WeChatHubWorker(IHubContext<WeChatHub, IHeartbeat> heartbeat)
        {
            this.heartbeat = heartbeat;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await this.heartbeat.Clients.All.HeartbeatAsync(0);
                await Task.Delay(3000);
                Console.WriteLine("heartbeat");
            }
        }
    }
}
