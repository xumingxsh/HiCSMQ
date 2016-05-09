using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiCSMQ
{
    /// <summary>负责接收QM发来的消息,并转到现成安全的队列.同时调用消息回调函数.
    /// 当前支持ActiveMQ1.6.0
    /// xuminRong 2016-05-08
    /// </summary>
    public sealed class HiMQConsumer
    {
        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public void Init(string ip, ushort port, string user, string pwd)
        {
            impl.Init(ip, port, user, pwd);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <returns></returns>
        public void Destory()
        {
            impl.Destory();
        }

        /// <summary>
        /// 监听主题
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="evt"></param>
        /// <returns></returns>
        public bool Listen(string topic, HiCSMQ.Impl.MQMsgCallback evt)
        {
            return impl.Listen(topic, evt);
        }

        /// <summary>
        /// 监听主题
        /// </summary>
        /// <param name="topics"></param>
        /// <param name="evt"></param>
        /// <returns></returns>
        public bool Listen(List<string> topics, HiCSMQ.Impl.MQMsgCallback evt)
        {
            return impl.Listen(topics, evt);
        }

        HiCSMQ.Impl.HiMQConsumerImpl impl = new Impl.HiMQConsumerImpl();
    }
}
