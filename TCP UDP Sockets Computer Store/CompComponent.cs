using System;
using System.Collections.Generic;
using System.Text;

namespace TCP_UDP_Sockets_Computer_Store
{
    public class CompComponent
    {
        public CompComponent(string name, double cost)
        {
            Name = name;
            Cost = cost;
        }
        public string Name { get; set; }
        public double Cost { get; set; }

        public override string ToString()
        {
            return $"Name:{Name} Cost:{(int)Cost},{(Cost * 100) % 100}";
        }
    }
}
