using MSCore.Net.Handler.SendPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSCore.Handler;
using static MSCore.Handler.PacketOpcode;
using MSCore.Client;

namespace MSCore.Net.Handler.SendablePacket
{
    class LoginPassword : AbstractSendablePacket
    {
        public LoginPassword() : base(SendPacketOpcode.LOGIN_PASSWORD)
        {
        }

        public override void WriteBody(MapleClient client)
        {
            packet.WriteMapleString(client.Account);
            packet.WriteMapleString(client.Passowrd);

            packet.WriteBytes(client.MacAddress);

        }
    }
}
