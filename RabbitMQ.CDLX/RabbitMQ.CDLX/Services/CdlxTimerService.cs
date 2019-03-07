using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.CDLX.Models;
using RabbitMQ.CDLX.Utils;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.CDLX.Services
{
    public class CdlxTimerService : MQServiceBase
    {
        public override string vHost { get { return "test_mq"; } }
        public override string Exchange { get { return "cdlx-Exchange"; } }
        public override List<BindInfo> Binds => new List<BindInfo>();
        private string queue = "cdlx-Master";

        public CdlxTimerService(IConfiguration cfg, ILogger logger) : base(cfg, logger)
        {
        }

        /// <summary>
        ///  检查死信队列
        /// </summary>
        /// <returns></returns>
        public List<CdlxMessage> CheckMessage()
        {
            long total = 0;
            List<CdlxMessage> list = new List<CdlxMessage>();
            var connection = base.CreateConnection();
            using (IModel channel = connection.Connection.CreateModel())
            {
                bool latest = true;
                while (latest)
                {
                    BasicGetResult result = channel.BasicGet(this.queue, false);
                    total++;
                    latest = result != null;
                    if (latest)
                    {
                        var json = Encoding.UTF8.GetString(result.Body);
                        list.Add(JsonConvert.DeserializeObject<CdlxMessage>(json));
                    }
                }
                channel.Close();
                connection.Close();
            }
            return list;
        }
    }
}
