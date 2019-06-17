using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ron.Consul.Models;
using System;

namespace Ron.Consul.Extensions
{
    public static class StartupExtension
    {
        /// <summary>
        ///  定义服务健康检查的url地址
        /// </summary>
        public const string HEALTH_CHECK_URI = "/consul/health/check";

        /// <summary>
        ///  读取 Consul 配置，注入服务
        /// </summary>
        /// <param name="service"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsulConfig(this IServiceCollection service,
                                                        IConfiguration configuration)
        {
            var clientConfig = configuration.GetSection("Consul").Get<ConsulConfig>();
            service.Configure<ConsulConfig>(configuration.GetSection("Consul"));

            return service;
        }


        /// <summary>
        ///  将 ConsulClient 注入管道
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <param name="lifetime"></param>
        /// <param name="cc"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app,
                                                    IConfiguration configuration,
                                                    IApplicationLifetime lifetime,
                                                    IOptions<ConsulConfig> cc)
        {
            var clientConfig = cc.Value;
            //获取服务运行侦听的地址和端口作为健康检查的地址
            var clientIP = new Uri($"{configuration["scheme"]}://{configuration["ip"]}:{configuration["port"]}");
            var serviceId = $"{clientConfig.ClientName}-{clientIP.Host}-{clientIP.Port}";
            var ipv4 = clientIP.Host;

            var consulClient = new ConsulClient(config =>
            {
                config.Address = new Uri(clientConfig.Server);
                config.Datacenter = clientConfig.DataCenter;
            });

            var healthCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(7), // 服务启动 7 秒后注册服务
                Interval = TimeSpan.FromSeconds(9), // 健康检查的间隔时间为：9秒
                HTTP = $"{clientIP.Scheme}://{ipv4}:{clientIP.Port}{HEALTH_CHECK_URI}"
            };

            var regInfo = new AgentServiceRegistration()
            {
                Checks = new[] { healthCheck },
                Address = ipv4,
                ID = serviceId,
                Name = clientConfig.ClientName,
                Port = clientIP.Port
            };
            consulClient.Agent.ServiceRegister(regInfo).GetAwaiter().GetResult();

            lifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceRegister(regInfo);
            });
            return app;
        }

        /// <summary>
        ///  实现健康检查输出，无需另行定义 Controller
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder MapHealthCheck(this IApplicationBuilder app)
        {
            app.Map(HEALTH_CHECK_URI, s =>
            {
                s.Run(async context =>
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Health check {0}", DateTime.Now);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    await context.Response.WriteAsync("ok");
                });
            });
            return app;
        }
    }
}
