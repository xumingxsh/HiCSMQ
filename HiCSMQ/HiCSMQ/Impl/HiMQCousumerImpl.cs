using System;
using System.Collections.Generic;

using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;

namespace HiCSMQ.Impl
{
    public delegate void MQMsgCallback(int flag, string topic, string msg);
    class HiMQCousumerImpl : HiMQBase
    {
        MQMsgCallback callback = null;
        public bool Listen(string topic, int flag, MQMsgCallback evt)
        {
            callback = evt;
            if (!Connecting())
            {
                return false;
            }

            IMessageConsumer consumer = mqSession.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("firstQueue"));
            //注册监听事件
            consumer.Listener += (IMessage message)=>
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
                evt(flag, topic, txtmsg.Text);
            };
            return true;
        }
    }
}
