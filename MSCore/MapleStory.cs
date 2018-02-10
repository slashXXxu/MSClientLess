using MapleLib.PacketLib;
using MSCore.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCore
{
    public class MapleStory
    {
        private ClientSession client;

        public MapleStory()
        {
            client = new ClientSession();
        }

        public void ConnectToLoginServer(string ip, int port)
        {
            client.Connect(ip, port);
        }
    }
}
