using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQHelper;
using Newtonsoft.Json;
using RabbitMQ.CDLX.Models;
using RabbitMQ.CDLX.Utils;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace RabbitMQ.CDLX.Services
{
    public class CdlxConsumerService : MQServiceBase
    {
        public override string vHost { get { return "test_mq"; } }
        public override string Exchange { get { return "cdlx-Exchange"; } }
        private string queue = "cdlx-Consumer";
        private string routeKey = "all";
        private List<BindInfo> bs = new List<BindInfo>();
        public override List<BindInfo> Binds { get { return bs; } }

        public CdlxConsumerService(IConfiguration cfg, ILogger logger) : base(cfg, logger)
        {
            this.bs.Add(new BindInfo
            {
                ExchangeType = ExchangeType.Direct,
                Queue = this.queue,
                RouterKey = this.routeKey,
                OnReceived = this.OnReceived
            });
        }
        bool first = true;
        private void OnReceived(MessageBody body)
        {
            if (first)
            {
                Console.WriteLine("消费时间\t类型\t进入时间\t过期时间\t内容");
                first = false;
            }
            var message = JsonConvert.DeserializeObject<CdlxMessage>(body.Content);
            Console.WriteLine("{0}\t{1}\t{5}{2}\t{3}\t{4}",
                                DateTime.Now.ToString("HH:mm:ss"),
                                message.Type,
                                message.CreateTime.ToString("HH:mm:ss"),
                                message.CreateTime.AddSeconds(message.Expire).ToString("HH:mm:ss"),
                                message.Data,
                                message.Type == MessageType.RedPackage ? "" : "\t");

            body.Consumer.Model.BasicAck(body.BasicDeliver.DeliveryTag, true);
        }
    }
}
