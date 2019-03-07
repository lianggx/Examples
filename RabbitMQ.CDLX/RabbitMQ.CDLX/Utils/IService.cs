using MQHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.CDLX.Utils
{
    public interface IService
    {
        /// <summary>
        ///  创建通道
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="routeKey">路由名称</param>
        /// <param name="exchangeType">交换机类型</param>
        /// <returns></returns>
        MQChannel CreateChannel(string queue, string routeKey, string exchangeType);
        /// <summary>
        ///  开启订阅
        /// </summary>
        void Start();
        /// <summary>
        ///  停止订阅
        /// </summary>
        void Stop();
        /// <summary>
        ///  发送错误消息
        /// </summary>
        /// <param name="content"></param>
        void SendWarning(string content);
        /// <summary>
        ///  通道列表
        /// </summary>
        List<MQChannel> List { get; set; }
        /// <summary>
        ///  访问消息队列的用户名
        /// </summary>
        string UserName { get; }
        /// <summary>
        ///  访问消息队列的密码
        /// </summary>
        string Password { get; }
        /// <summary>
        ///  消息队列的主机地址
        /// </summary>
        string Host { get; }
        /// <summary>
        ///  消息队列的主机开放的端口
        /// </summary>
        int Port { get; }
        /// <summary>
        ///  消息队列中定义的虚拟机
        /// </summary>
        string vHost { get; }
        /// <summary>
        ///  消息队列中定义的交换机
        /// </summary>
        string Exchange { get; }
        /// <summary>
        ///  定义的队列列表
        /// </summary>
        List<BindInfo> Binds { get; }
    }
}
