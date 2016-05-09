using System;
using System.Collections.Generic;

namespace HiCSMQ
{
    /// <summary>
    /// MQ生产者,主要复杂向MQ发送主题消息
    /// MQ访问代理,主要负责接收QM发来的消息,并转到现成安全的队列.同时调用消息回调函数.
    /// 当前支持ActiveMQ1.6.0
    /// xuminRong 2016-05-07
    /// </summary>
    public sealed class HiMQProducer
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
        /// 发送主题消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
	    public bool SendTopic(string topic, string msg)
        {
            return impl.SendTopic(topic, msg);
        }
	
        /// <summary>
        /// 发送主题消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="msgs"></param>
        /// <returns></returns>
        public int SendTopic(string topic, List<string> msgs)
        {
            return impl.SendTopic(topic, msgs);
        }
	
	    /// <summary>
	    /// 关闭
	    /// </summary>
	    /// <returns></returns>
        public void Destory()
        {
            impl.Destory();
        }

        HiCSMQ.Impl.HiMQProducerImpl impl = new Impl.HiMQProducerImpl();
    }
}
