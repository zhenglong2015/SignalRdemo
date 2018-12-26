using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// 自定义一个Hub类
    /// 用于Singlar的连接
    /// </summary>
    public class MyHub : Hub
    {
        /// <summary>
        /// 存放连接客户端的标识
        /// 通过维护一个标识ID可以实现聊天室
        /// </summary>
        private List<string> m_ConnectionId = new List<string>();

        /// <summary>
        /// Called when the connection connects to this hub instance.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task</returns>
        public override Task OnConnected()
        {
            string connectionId = this.Context.ConnectionId;

            Console.WriteLine("{0}上线", connectionId);

            if (m_ConnectionId.Any(i => i.Equals(connectionId)))
            {
                m_ConnectionId.Add(connectionId);
            }

            return base.OnConnected();
        }

        //
        // Summary:
        //     Called when a connection disconnects from this hub gracefully or due to a timeout.
        //
        // Parameters:
        //   stopCalled:
        //     true, if stop was called on the client closing the connection gracefully; false,
        //     if the connection has been lost for longer than the Microsoft.AspNet.SignalR.Configuration.IConfigurationManager.DisconnectTimeout.
        //     Timeouts can be caused by clients reconnecting to another SignalR server in scaleout.
        //
        // Returns:
        //     A System.Threading.Tasks.Task
        public override Task OnDisconnected(bool stopCalled)
        {
            string connectionId = this.Context.ConnectionId;

            Console.WriteLine("{0}下线", connectionId);

            if (m_ConnectionId.Any(i => i.Equals(connectionId)))
            {
                m_ConnectionId.Remove(connectionId);
            }

            return base.OnDisconnected(stopCalled);
        }
        /// <summary>
        /// 自定义发送方法
        /// 供应客户端使用
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public void Send(string name, string message)
        {
            Clients.AllExcept(this.Context.ConnectionId).AddMessage(name, message);
        }
    }
}
