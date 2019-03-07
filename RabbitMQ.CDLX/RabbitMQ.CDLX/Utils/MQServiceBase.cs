using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQHelper;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.CDLX.Utils
{
    public abstract class MQServiceBase : IService
    {
        internal bool started = false;
        internal IConfiguration cfg;
        internal ILogger logger;
        internal MQChannel Channel { get; set; }
        internal MQServiceBase(IConfiguration cfg, ILogger logger)
        {
            this.cfg = cfg;
            this.logger = logger;
            UserName = cfg["rabbitmq:username"];
            Password = cfg["rabbitmq:password"];
            Host = cfg["rabbitmq:host"];
            Port = Convert.ToInt32(cfg["rabbitmq:port"]);
        }

        public MQChannel CreateChannel(string queue, string routeKey, string exchangeType)
        {
            MQConnection conn = new MQConnection(this.UserName, this.Password, this.Host, this.Port, this.vHost, this.logger);
            MQChannel channel = conn.CreateReceiveChannel(exchangeType, this.Exchange, queue, routeKey);
            return channel;
        }

        public MQConnection CreateConnection()
        {
            MQConnection conn = new MQConnection(this.UserName, this.Password, this.Host, this.Port, this.vHost, this.logger);
            return conn;
        }

        public void SendWarning(string content)
        {
            Task.Run(() =>
            {
                try
                {
                    // To Do
                }
                catch (Exception ex) { logger.LogError($"{ex.Message},{ex.StackTrace}"); }
            });
        }


        /// <summary>
        ///  启动订阅
        /// </summary>
        public void Start()
        {
            if (started)
            {
                Console.WriteLine("服务已经启动！");
                return;
            }
            foreach (var item in this.Binds)
            {
                Channel = CreateChannel(item.Queue, item.RouterKey, item.ExchangeType);
                Channel.OnReceivedCallback = item.OnReceived;
                this.List.Add(Channel);
            }
            started = true;
        }

        /// <summary>
        ///  停止订阅
        /// </summary>
        public void Stop()
        {
            foreach (var c in this.List)
            {
                logger.LogDebug("正在关闭消息通道,{0},{1},{2}", c.ExchangeName, c.QueueName, c.RoutekeyName);
                c.Stop();
                logger.LogDebug("已关闭消息通道,{0},{1},{2}", c.ExchangeName, c.QueueName, c.RoutekeyName);
            }
            this.List.Clear();
            started = false;
        }

        public List<MQChannel> List { get; set; } = new List<MQChannel>();

        /// <summary>
        ///  访问消息队列的用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        ///  访问消息队列的密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        ///  消息队列的主机地址
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        ///  消息队列的主机开放的端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        ///  消息队列中定义的虚拟机
        /// </summary>
        public abstract string vHost { get; }
        /// <summary>
        ///  消息队列中定义的交换机
        /// </summary>
        public abstract string Exchange { get; }
        /// <summary>
        ///  定义的队列列表
        /// </summary>
        public abstract List<BindInfo> Binds { get; }
    }
}
