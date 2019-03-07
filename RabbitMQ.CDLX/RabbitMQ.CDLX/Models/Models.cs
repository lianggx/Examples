using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.CDLX.Models
{
    public enum MessageType
    {
        RedPackage = 1,
        Order,
        Vote
    }

    public class CdlxMessage
    {
        public MessageType Type { get; set; }
        public DateTime CreateTime { get; set; }
        public long Expire { get; set; }
        public string Data { get; set; }
    }
}
