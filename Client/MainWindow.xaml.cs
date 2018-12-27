using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private IHubProxy m_HubProxy { get; set; }

        const string SWRVWEURL = "http://localhost:8888/signalr";
        private HubConnection m_Connection { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                ConnectAsync();
            };

            this.Unloaded += (s, e) =>
            {
                if (m_Connection != null)
                {
                    m_Connection.Dispose();
                }
            };
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            // 通过代理来调用服务端的Send方法
            // 服务端Send方法再调用客户端的AddMessage方法将消息输出到消息框中
            m_HubProxy.Invoke("Send", "ztb", txtSend.Text.Trim());

            txtSend.Text = String.Empty;
            txtSend.Focus();
        }


        /// <summary>
        /// 连接服务
        /// </summary>
        private async void ConnectAsync()
        {
            m_Connection = new HubConnection(SWRVWEURL);

            // 创建一个集线器代理对象
            m_HubProxy = m_Connection.CreateHubProxy("MyHub");

            // 供服务端调用，将消息输出到消息列表框中
            m_HubProxy.On<string, string>("AddMessage", (name, message) =>
                 this.Dispatcher.Invoke(() =>
                    txtMsg.AppendText(String.Format("{0}: {1}\r", name, message))
                ));

            try
            {
                await m_Connection.Start();
            }
            catch (HttpRequestException e)
            {
                // 连接失败
                return;
            }

            txtMsg.AppendText("连上服务：" + SWRVWEURL + "\r");
        }
    }
}
