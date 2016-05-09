using System;
using System.Collections.Generic;

using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;

namespace HiCSMQ.Impl
{
    /// <summary>
    /// 接收到主题消息后的回调事件
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="msg"></param>
    public delegate void MQMsgCallback(string topic, string msg);

    class HiMQConsumerImpl : HiMQBase
    {
        MQMsgCallback callback = null;
        public bool Listen(string topic, MQMsgCallback evt)
        {
            callback = evt;
            if (!Connecting())
            {
                return false;
            }

            IMessageConsumer consumer = mqSession.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(topic));
            //注册监听事件
            consumer.Listener += OnTopic;
            return true;
        }

        public bool Listen(List<string> topics, MQMsgCallback evt)
        {
            callback = evt;
            if (!Connecting())
            {
                return false;
            }

            foreach (string it in topics)
            {
                IMessageConsumer consumer = mqSession.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(it));
                //注册监听事件
                consumer.Listener += OnTopic;
            }
            return true;
        }

        private void OnTopic(IMessage message)
        {
            if (message == null)
            {
                return;
            }

            ITextMessage txtmsg = message as ITextMessage;
            if (txtmsg == null)
            {
                return;
            }
            ActiveMQTopic dst = message.NMSDestination as ActiveMQTopic;
            if (dst == null)
            {
                return;
            }

            if (callback != null)
            {
                callback(dst.TopicName, txtmsg.Text);
            }
        }
    }
}
