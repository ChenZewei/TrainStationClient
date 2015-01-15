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

namespace client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Socket server, client;
        private IPEndPoint ipEnd;
        byte[] recv;
        int i;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            recv = new byte[200];
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
            //while(true)
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
            }
        }

        private void Send(object sender, RoutedEventArgs e)
        {
            server.Send(Encoding.ASCII.GetBytes(SendBox.Text));
        }
    }
}
