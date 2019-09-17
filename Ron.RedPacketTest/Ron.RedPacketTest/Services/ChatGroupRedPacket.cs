using System;

namespace Ron.RedPacketTest.Service
{
    public class ChatGroupRedPacket : RedPacket
    {
        public override string Name { get; } = "ChatGroup";
        public override string Put(int org_id, int money, int count, string reason)
        {
            Console.WriteLine("检查群ID：{0},是否存在", org_id);
            return base.Create(reason, money, count);
        }

        public override string Get(int id)
        {
            Console.WriteLine("检否群ID：{0}，当前用户是否群成员", id);
            return base.Fighting();
        }
    }
}