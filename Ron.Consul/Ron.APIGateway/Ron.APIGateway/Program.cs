using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ron.APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder().AddCommandLine(args).Build();
            var url = $"{config["scheme"]}://{config["ip"]}:{config["port"]}";
            return WebHost.CreateDefaultBuilder(args)
                          .UseConfiguration(config)
                          .UseUrls(url)
                          .UseStartup<Startup>();
        }
    }
}
