using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace TCP_UDP_Sockets_Computer_Store
{
    public partial class ServerForm : Form
    {
        IPAddress address;
        EndPoint endPoint;
        Socket socketServer;
        List<CompComponent> compComponents;
        List<Client> points;

        public ServerForm()
        {
            InitializeComponent();

            compComponents = new List<CompComponent>()
            {
                new CompComponent("Monitor", 22.55),
                new CompComponent("Keyboard", 10.00),
                new CompComponent("System unit", 5.00),
                new CompComponent("Mouse", 23.55)
            };

            points = new List<Client>();

            address = IPAddress.Parse("127.0.0.1");
            endPoint = new IPEndPoint(address, 51234);

            socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socketServer.Bind(endPoint);

            Task.Run(() => Listen());
        }

        private async void Listen()
        {
            byte[] buffer;
            string message;
            while (socketServer != null)
            {
                buffer = new byte[1024];
                EndPoint end = new IPEndPoint(IPAddress.Any, 0);
                var res = await socketServer.ReceiveFromAsync(buffer, SocketFlags.None, end);

                end = (IPEndPoint)res.RemoteEndPoint;

                Client client;

                if ((client = points.Where(x => x.EndPoint.ToString() == ((IPEndPoint)end).ToString()).FirstOrDefault()) == null)
                {
                    client = new Client((IPEndPoint)end);
                    points.Add(client);
                }
                client.Count++;

                message = Encoding.UTF8.GetString(buffer).Trim('\0');

                textBox1.Text += $"{((IPEndPoint)endPoint).Address} ({DateTime.Now.TimeOfDay}){(client.Count > 10 ? "(Blocked)" : "")}: {message}";
                textBox1.Text += "\r\n";
                if (client.Count <= 10)
                {
                    Task.Run(() => Answer(message, end));
                }
            }
        }

        private async void Answer(string message, EndPoint end)
        {
            string send;
            switch (message)
            {
                case "monitor":
                    send = compComponents[0].ToString();
                    break;
                case "keyboard":
                    send = compComponents[1].ToString();
                    break;
                case "system unit":
                    send = compComponents[2].ToString();
                    break;
                case "mouse":
                    send = compComponents[3].ToString();
                    break;
                default:
                    send = "Wrong command!";
                    break;
            }
            socketServer.SendTo(Encoding.UTF8.GetBytes(send), end);
        }
    }
}
