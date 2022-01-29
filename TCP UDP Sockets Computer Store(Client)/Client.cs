using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCP_UDP_Sockets_Computer_Store_Client_
{
    public partial class ClientForm : Form
    {
        IPAddress address;
        EndPoint toEndPoint;
        EndPoint endPoint;
        Socket client;
        public ClientForm()
        {
            InitializeComponent();
            address = IPAddress.Parse("127.0.0.1");
            toEndPoint = new IPEndPoint(address, 51234);
            endPoint = new IPEndPoint(address, 0);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            client.Bind(endPoint);
            Task.Run(() => Listen());
        }

        private async void Listen()
        {
            byte[] buffer;
            while (client != null)
            {
                buffer = new byte[1024];
                await client.ReceiveFromAsync(buffer, SocketFlags.None, new IPEndPoint(IPAddress.Any, 0));
                textBox4.Text += Encoding.UTF8.GetString(buffer);
                textBox4.Text += "\r\n";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.SendTo(Encoding.UTF8.GetBytes(textBox1.Text), toEndPoint);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            address = IPAddress.Parse(textBox2.Text);
            toEndPoint = new IPEndPoint(address, Int32.Parse(textBox3.Text));
        }
    }
}
