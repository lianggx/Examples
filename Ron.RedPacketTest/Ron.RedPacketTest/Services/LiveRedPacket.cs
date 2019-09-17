using System;

namespace Ron.RedPacketTest.Service
{
    public class LiveRedPacket : RedPacket
    {
        public override string Name { get; } = "Live";
        public override string Put(int org_id, int money, int count, string reason)
        {
            Console.WriteLine("检查直播ID:{0}是否存在", org_id);
            return base.Create(reason, money, count);
        }

        public override string Get(int id)
        {
            Console.WriteLine("检查红包ID：{0} 是否当前主播红包", id);
            return base.Fighting();
        }
    }
}