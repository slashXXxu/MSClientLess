using MSCore.Net.Handler.SendPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSCore.Handler;
using static MSCore.Handler.PacketOpcode;
using MSCore.Client;
using MSCore.Tools;

namespace MSCore.Net.Handler.SendablePacket
{
    class LoginPassword : AbstractSendablePacket
    {
        public LoginPassword() : base(SendPacketOpcode.LOGIN_PASSWORD)
        {
        }

        public override void WriteBody(MapleClient client)
        {
            Logger.Info("登入帳號: {0} 登入密碼: {1} Mac: {2}", 
                client.Account, 
                client.Passowrd,
                HexUtil.ByteArrayToString(client.MacAddress));
            packet.WriteMapleString(client.Account);
            packet.WriteMapleString(client.Passowrd);
            packet.WriteBytes(client.MacAddress);
        }
    }
}
