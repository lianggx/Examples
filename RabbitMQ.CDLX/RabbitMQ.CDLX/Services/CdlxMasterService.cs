using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQHelper;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.CDLX.Services
{
    public class CdlxMasterService
    {
        private IConfiguration cfg = null;
        private ILogger logger = null;
        private string vhost = "test_mq";
        private string exchange = "cdlx-Exchange";
        private string routekey = "master";
        private static MQConnection connection = null;

        public CdlxMasterService(IConfiguration cfg)
        {
            this.cfg = cfg;
        }

        private MQConnection Connection
        {
            get
            {
                if (connection == null || !connection.Connection.IsOpen)
                {
                    connection = new MQConnection(
                        cfg["rabbitmq:username"],
                        cfg["rabbitmq:password"],
                        cfg["rabbitmq:host"],
                        Convert.ToInt32(cfg["rabbitmq:port"]),
                        vhost,
                        logger);
                }
                return connection;
            }
        }

        private static IModel channel = null;
        private IModel Channel
        {
            get
            {
                if (channel == null || channel.IsClosed)
                    channel = Connection.Connection.CreateModel();

                return channel;
            }
        }

        /// <summary>
        /// 发送队列消息
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage(object data, long expireMilliseconds)
        {
            string message = JsonConvert.SerializeObject(data);
            IBasicProperties prop = new BasicProperties();
            prop.Expiration = expireMilliseconds.ToString();
            this.Connection.Publish(this.Channel, exchange, routekey, message, prop);
        }
    }
}
