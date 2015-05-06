using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Socket server, client;
        private IPEndPoint ipEnd;
        Thread recvThread;
        int i;

        public MainWindow()
        {
            InitializeComponent();
        }

        ~MainWindow()
        {
            if (recvThread != null)
            {
                recvThread.Abort();
                recvThread.Join();
            }
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            ipEnd = new IPEndPoint(IPAddress.Parse(IP.Text), 15000);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipEnd);
            }
            catch(SocketException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Send.IsEnabled = true;
            recvThread = new Thread(RecvThread);
            recvThread.IsBackground = true;
            recvThread.Start();
            /*while(true)
            {
                try
                {
                    i = server.Receive(recv);
                }
                catch (SocketException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                ReceiveBox.AppendText( Encoding.UTF8.GetString(recv,0,i)+"\n");
            }*/
        }

        private void RecvThread()
        {
            byte[] recv;
            recv = new byte[2048];
            while(true)
            {
                try
                {
                    i = server.Receive(recv);
                }
                catch (SocketException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                this.Dispatcher.BeginInvoke(new Action(() => ReceiveBox.AppendText(Encoding.GetEncoding("GB2312").GetString(recv, 0, i))));
            }
            
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < 100; i++ )
                    server.Send(Encoding.GetEncoding("GB2312").GetBytes(SendBox.Text));
            }
            catch(SocketException ex)
            {
                ReceiveBox.AppendText("Please connect server!\r\n");
                Console.Write(ex.Message);
            }
        }
    }
}
