namespace Ron.RedPacketTest.Service
{
    public interface IRedPacket
    {
        string Name { get; }
        string Put(int org_id, int money, int count, string reason);
        string Get(int id);
    }
}