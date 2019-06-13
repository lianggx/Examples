using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ron.Consul
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder().AddCommandLine(args).Build();
            var url = $"{config["schema"]}://{config["ip"]}:{config["port"]}";
            return WebHost.CreateDefaultBuilder(args)
                  .UseStartup<Startup>()
                  .UseConfiguration(config)
                  .UseUrls(url)
                  .Build();
        }
    }
}
