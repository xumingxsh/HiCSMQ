using System;

using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;

namespace HiCSMQ.Impl
{
    class HiMQBase
    {
        public void Init(string ip, ushort port, string user, string pwd)
        {
            this.ip = ip;
            this.port = port;
            this.user = user;
            this.pwd = pwd;
        }
        public virtual void Destory()
        {
            if (mqSession != null)
            {
                mqSession.Dispose();
            }
            if (mqConn != null)
            {
                mqConn.Dispose();
            }
        }

        protected bool Connecting()
        {
            if (mqConn != null)
            {
                return true;
            }

            // 尝试连接是否成功(不断开自动重连)
            if (!TryConnect())
            {
                return false;
            }

            // 实际连接,断开会自动重连
            string address = GetAddress();
            try
            {
                IConnectionFactory factory = new ConnectionFactory(address);
                mqConn = factory.CreateConnection();
                mqConn.Start();
                mqSession = mqConn.CreateSession(AcknowledgementMode.AutoAcknowledge);
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        private bool TryConnect()
        {
            string address = GetTryWddress();
            IConnection conn = null;
            try
            {
                IConnectionFactory factory = new ConnectionFactory(address);
                conn = factory.CreateConnection();
                conn.Start();
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private string GetAddress()
        {
            return string.Format("failover:(tcp://{0}:{1})", ip, port);
        }

        private string GetTryWddress()
        {
            return string.Format("tcp://{0}:{1}", ip, port);
        }
        string ip;
        ushort port;
        string user;
        string pwd;
        protected IConnection mqConn;
        protected ISession mqSession;
    }
}
