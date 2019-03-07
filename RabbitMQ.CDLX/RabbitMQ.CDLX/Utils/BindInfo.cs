using MQHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.CDLX.Utils
{
    public partial class BindInfo
    {
        /// <summary>
        ///  队列名称
        /// </summary>
        public string Queue { get; set; }
        /// <summary>
        ///  路由名称
        /// </summary>
        public string RouterKey { get; set; }
        /// <summary>
        ///  交换机类型
        /// </summary>
        public string ExchangeType { get; set; }
        /// <summary>
        ///  订阅回调函数
        /// </summary>
        public Action<MessageBody> OnReceived { get; set; }
    }
}
