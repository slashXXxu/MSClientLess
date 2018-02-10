using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSCore.Client;
using MSCore.Handler;
using MSCore.Tools;

namespace MSCore.Net.Handler.SendablePacket
{
    public class Pong : AbstractSendablePacket
    {
        public Pong() : base(PacketOpcode.SendPacketOpcode.PONG)
        {
        }

        public override void WriteBody(MapleClient client)
        {
            Logger.Info("發送 PONG 封包");
            return;
        }
    }
}
