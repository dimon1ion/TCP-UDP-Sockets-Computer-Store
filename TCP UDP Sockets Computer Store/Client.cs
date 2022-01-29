using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace TCP_UDP_Sockets_Computer_Store
{
    public class Client
    {
        public IPEndPoint EndPoint { get; set; }
        private Timer timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);

        private int count;

        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                if (count > 0)
                {
                    if (!timer.Enabled)
                    {
                        timer.Start();
                    }
                }
            }
        }


        public Client(IPEndPoint endPoint)
        {
            this.EndPoint = endPoint;
            timer.Elapsed += Tick;
            count = 0;
            System.Windows.Forms.MessageBox.Show(TimeSpan.FromMinutes(1).TotalMilliseconds.ToString());
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            count = 0;
        }
    }
}
