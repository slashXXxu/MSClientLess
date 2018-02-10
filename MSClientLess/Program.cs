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

            ms.ConnectToLoginServer("172.96.161.162", 8484);

            MapleClientHandler.Instance.ToString();

            Console.Read();
        }
    }
}
