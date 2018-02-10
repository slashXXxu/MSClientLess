using MSCore.Net;
using MSCore.Net.Handler.SendPacket;
using MSCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCore.Client
{
    class MapleClient
    {
        public String Account {  get; set; }

        public String Passowrd { private get; set; }

        private ClientSession session = null;

        public MapleClient()
        {
            session = new ClientSession();
        }

        public void Connect(String ip, int port)
        {
            session.Connect(ip, port);
        }

        public void Disconnect()
        {
            session.Disconnect();
        }

        public void Send(ISendablePacket packet)
        {
            session.Send(packet.toArray());
        }

        public void Login()
        {
            if (Account == null || Passowrd == null)
            {
                Logger.Error("帳號或密碼不可為空");
                return;
            }
            
        }

    }
}
