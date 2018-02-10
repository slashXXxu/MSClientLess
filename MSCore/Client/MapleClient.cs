using MSCore.Handler;
using MSCore.Net;
using MSCore.Net.Handler;
using MSCore.Net.Handler.ReceiceablePacket;
using MSCore.Net.Handler.SendablePacket;
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
        public int AccountId;

        public String Account {  get; set; }

        public String Passowrd { get; set; }

        public int Gender;

        public int GmLevel;

        public Byte[] MacAddress;

        private ClientSession Session = null;

        public MapleClient()
        {
            Session = new ClientSession(this);
            InitMacAddress();
        }

        public bool isConnected()
        {
            return Session != null ? Session.Connected : false;
        }

        public bool isInit()
        {
            return Session != null ? Session.isInit : false;
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

        public void Send(AbstractSendablePacket packet)
        {
            packet.WriteBody(this);
            Logger.Debug("[發送封包] OPCODE: {0}(0x{1:X4}) ALL: {2}",
                Enum.GetName(typeof(PacketOpcode.SendPacketOpcode), packet.Opcode),
                packet.Opcode,
                HexUtil.ByteArrayToString(packet.toArray()));
            Session.Send(packet.toArray());
        }

        public void Login()
        {
            if (Account == null || Passowrd == null)
            {
                Logger.Error("帳號或密碼不可為空");
                return;
            }
            Send(new LoginPassword());
        }

    }
}
