using MapleLib.PacketLib;
using MSCore.Client;
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
        private MapleClient client;

        public MapleStory()
        {
            client = new MapleClient();
        }

        public void ConnectToLoginServer(string ip, int port)
        {
            client.Connect(ip, port);
        }

        public void Login(String account, String password)
        {
            client.Account = account;
            client.Passowrd = password;
            while (!client.isInit()) ;
            client.Login();
        }
    }
}
