using RabbitMQ.CDLX.Models;
using RabbitMQ.CDLX.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.CDLX.BLL
{
    public class Contract
    {
        private CdlxMasterService masterService;
        public Contract(CdlxMasterService masterService)
        {
            this.masterService = masterService;
        }

        /// <summary>
        ///  推送消息到死信队列
        /// </summary>
        /// <param name="type">业务类型</param>
        /// <param name="data">业务数据</param>
        /// <param name="expire">在队列中的过期时间</param>
        public void Publish(MessageType type, string data, long expire)
        {
            var cm = new CdlxMessage { Type = type, Data = data, CreateTime = DateTime.Now, Expire = expire };
            masterService.SendMessage(cm, expire * 1000);
        }
    }
}
