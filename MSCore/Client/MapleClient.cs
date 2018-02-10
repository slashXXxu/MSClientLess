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
    public class MapleClient
    {
        public String Account {  get; set; }

        public String Passowrd { get; set; }

        public Byte[] MacAddress;

        private ClientSession Session = null;

        public MapleClient()
        {
            Session = new ClientSession();
        }

        private void InitMacAddress()
        {
            MacAddress = new Byte[6];
            new Random((int)DateTime.Now.Ticks).NextBytes(MacAddress);
        }

        public void Connect(String ip, int port)
        {
            Session.Connect(ip, port);
        }

        public void Disconnect()
        {
            Session.Disconnect();
        }

        public void Send(IPacket packet)
        {
            Session.Send(packet.toArray());
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
