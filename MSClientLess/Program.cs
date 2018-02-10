using MSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClientLess
{
    class Program
    {
        static void Main(string[] args)
        {
            MapleStory ms = new MapleStory();

            ms.ConnectToLoginServer("192.168.156.1", 8484);

            Console.Read();
        }
    }
}
