using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HiCSMQ.Impl;

namespace HiCSMQ.Test
{
    [TestClass]
    public class UnitTest_Comsumer
    {
        [TestMethod]
        public void TestMethod_Comsumer_Listen()
        {
            bool isReceive = false;
            string content = "this is a test";
            string title = "TEST";
            HiMQCousumerImpl consumer = new HiMQCousumerImpl();
            consumer.Init("127.0.0.1", 61616, "admin", "admin");
            consumer.Listen(title, (string topic, string text) =>
                {
                    isReceive = true;
                    System.Diagnostics.Debug.WriteLine(string.Format("{0}:{1}", topic, text));
                    Assert.IsTrue(title.Equals(topic));
                    Assert.IsTrue(text.Equals(content));
                });

            HiMQProducerImpl producer = new HiMQProducerImpl();
            producer.Init("127.0.0.1", 61616, "admin", "admin");
            bool ret = producer.SendTopic(title, content);
            Assert.IsTrue(ret);
            while (!isReceive)
            {
                System.Threading.Thread.Sleep(100);
            }
            producer.Destory();
            consumer.Destory();
            System.Diagnostics.Debug.WriteLine("finish");
        }

        [TestMethod]
        public void TestMethod_Comsumer_Listen_List()
        {
            int index = 0;
            string content = "this is a test";
            List<string> titles = new List<string>();
            titles.Add("TEST1");
            titles.Add("TEST2");
            titles.Add("TEST3");
            titles.Add("TEST4");
            HiMQCousumerImpl consumer = new HiMQCousumerImpl();
            consumer.Init("127.0.0.1", 61616, "admin", "admin");
            consumer.Listen(titles, (string topic, string text) =>
            {
                index++;
                System.Diagnostics.Debug.WriteLine(string.Format("{0}:{1}", topic, text));
                Assert.IsTrue(topic.StartsWith("TEST"));
                Assert.IsTrue(text.Equals(content));
            });

            HiMQProducerImpl producer = new HiMQProducerImpl();
            producer.Init("127.0.0.1", 61616, "admin", "admin");
            foreach(string it in titles)
            {
                bool ret = producer.SendTopic(it, content);
                Assert.IsTrue(ret);
            }
            while (index < 3)
            {
                System.Threading.Thread.Sleep(100);
            }
            producer.Destory();
            consumer.Destory();
            System.Diagnostics.Debug.WriteLine("finish");
        }

        [TestMethod]
        public void TestMethod_Producer_Listen()
        {
            bool isReceive = false;
            List<string> contents = new List<string>();
            contents.Add("this is a test1");
            contents.Add("this is a test2");
            contents.Add("this is a test3");
            contents.Add("this is a test4");
            string title = "TEST";
            int index = 0;
            HiMQCousumerImpl consumer = new HiMQCousumerImpl();
            consumer.Init("127.0.0.1", 61616, "admin", "admin");
            consumer.Listen(title, (string topic, string text) =>
            {
                index++;
                    System.Diagnostics.Debug.WriteLine(string.Format("{0}:{1}", topic, text));
                    Assert.IsTrue(title.Equals(topic));
                    Assert.IsTrue(text.StartsWith("this is a test"));
                });

            HiMQProducerImpl producer = new HiMQProducerImpl();
            producer.Init("127.0.0.1", 61616, "admin", "admin");
            int ret = producer.SendTopic(title, contents);
            Assert.IsTrue(ret == 4);
            while (index < 3)
            {
                System.Threading.Thread.Sleep(100);
            }
            producer.Destory();
            consumer.Destory();
            System.Diagnostics.Debug.WriteLine("finish");
        }
    }
   }
