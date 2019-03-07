using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.CDLX.Utils
{
    public class MQServcieManager
    {
        private int timer_tick = 60 * 1000;
        private Timer timer = null;
        private ILogger logger = null;
        private IConfiguration cfg;

        public MQServcieManager(IConfiguration cfg, ILogger logger)
        {
            this.cfg = cfg;
            this.logger = logger;

            timer = new Timer(OnInterval, "", timer_tick, timer_tick);
        }

        /// <summary>
        ///  自检
        /// </summary>
        /// <param name="sender"></param>
        private void OnInterval(object sender)
        {
            int count = 0;
            logger.LogInformation("检查连接状态...");
            for (int i = 0; i < this.ChannelList.Count; i++)
            {
                var item = this.ChannelList[i];
                foreach (var c in item.List)
                {
                    if (c.Connection == null || c.Connection == null || !c.Connection.IsOpen)
                    {
                        count++;
                        logger.LogInformation("重新创建消息订阅,{0},{1}", c.QueueName, c.RoutekeyName);
                        try
                        {
                            c.Stop();
                            item.List.Remove(c);
                            var channel = item.CreateChannel(c.QueueName, c.RoutekeyName, c.ExchangeTypeName);
                            item.List.Add(channel);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError($",{ex.Message},{ex.StackTrace}");

                            item.SendWarning($"重新创建消息订阅,{ c.QueueName},{c.RoutekeyName}");
                            return;
                        }
                        logger.LogInformation("创建完成,{0},{1}", c.QueueName, c.RoutekeyName);
                    }
                }
            }
            logger.LogInformation($"检查完成.错误数：{count}");
        }

        public void Start()
        {
            ChannelList = new List<IService>
            {
                new CDLX.Services.CdlxConsumerService(this.cfg, this.logger)
            };

            foreach (var item in this.ChannelList)
            {
                item.Start();
            }
        }

        public void Stop()
        {
            foreach (var item in this.ChannelList)
            {
                item.Stop();
            }
            ChannelList.Clear();
            timer.Dispose();
        }

        public List<IService> ChannelList { get; set; }
    }
}
