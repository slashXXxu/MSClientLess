using MSCore;
using MSCore.Net.Handler;
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

            ms.ConnectToLoginServer("127.0.0.1", 8484);
            ms.Login("admin", "admin");
            Console.Read();
        }
    }
}
