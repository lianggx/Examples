using System;

namespace Ron.RedPacketTest.Service
{
    public abstract class RedPacket : IRedPacket
    {
        public abstract string Name { get; }

        public abstract string Put(int org_id, int money, int count, string reason);

        public abstract string Get(int id);

        protected string Create(string reason, int money, int count)
        {
            Console.WriteLine("红包类型：{0}，创建了红包:{1},金额：Money:{2},数量:{3}", this.Name, reason, money, count);
            return "成功";
        }

        protected string Fighting()
        {
            Console.WriteLine("红包类型：{0}，调用了抢红包方法:{0}", this.Name, nameof(Fighting));
            return "成功";
        }
    }
}