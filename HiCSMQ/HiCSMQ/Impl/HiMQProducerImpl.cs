using System;
using System.Collections.Generic;

using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;

namespace HiCSMQ.Impl
{
    class HiMQProducerImpl: HiMQBase
    {
        public bool SendTopic(string topic, string msg)
        {
            IMessageProducer producer = GetTopic(topic);
            if (producer == null)
            {
                return false;
            }
            ITextMessage message = producer.CreateTextMessage();
            message.Text = msg;
            try
            {
                producer.Send(message);
            }
            catch(Exception ex)
            {
                ex.ToString();
                return false;
            }
            return true;
        }

        public int SendTopic(string topic, List<string> msgs)
        {
            IMessageProducer producer = GetTopic(topic);
            if (producer == null)
            {
                return -1;
            }

            int successCount = 0;
            foreach(string msg in msgs)
            {
                ITextMessage message = producer.CreateTextMessage();
                message.Text = msg;
                try
                {
                    producer.Send(message);
                    successCount++;
                }
                catch(Exception ex)
                {
                    ex.ToString();
                }
            }
            return successCount;
        }

        public override void Destory()
        {
            topics.Clear();
            topics = null;
            base.Destory();
        }

        private IMessageProducer GetTopic(string topic)
        {
            if (!Connecting())
            {
                return null;
            }

            if (mqSession == null)
            {
                return null;
            }

            if (topics.ContainsKey(topic))
            {
                return topics[topic];
            }

            ActiveMQTopic dest = new ActiveMQTopic(topic);
            IMessageProducer producer = mqSession.CreateProducer(dest);
            topics[topic] = producer;
            return producer;
        }


        Dictionary<string, IMessageProducer> topics = new Dictionary<string, IMessageProducer>();        
    }
}
