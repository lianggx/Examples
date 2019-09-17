using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ron.RedPacketTest.ViewModel
{
    public class RedPacketViewModel
    {
        // 红包类型
        public string Type { get; set; }
        // 业务编号
        public int Org_Id { get; set; }
        // 金额
        public int Money { get; set; }
        // 数量
        public int Count { get; set; }
        // 说明
        public string Reason { get; set; }
    }
}
