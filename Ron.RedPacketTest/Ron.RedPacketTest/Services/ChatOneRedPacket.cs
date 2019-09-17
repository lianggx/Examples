using System;

namespace Ron.RedPacketTest.Service
{
    public class ChatOneRedPacket : RedPacket
    {
        public override string Name { get; } = "ChatOne";
        public override string Put(int org_id, int money, int count, string reason)
        {
            Console.WriteLine("检查接收人ID:{0}是否存在", org_id);
            return base.Create(reason, money, count);
        }

        public override string Get(int id)
        {
            Console.WriteLine("检查红包ID:{0}，是否具有领取资格", id);
            return base.Fighting();
        }
    }
}