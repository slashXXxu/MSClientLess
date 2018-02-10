using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapleLib.PacketLib;
using MSCore.Client;
using MSCore.Handler;
using MSCore.Net.Handler.SendablePacket;
using MSCore.Tools;

namespace MSCore.Net.Handler.ReceiceablePacket
{
    public class Ping : AbstractReceiveablePacket
    {
        public Ping() : base(PacketOpcode.RecvPacketOpcode.PING)
        {
        }

        public override void HandlePacket(MapleClient client, PacketReader packet)
        {
            Logger.Info("收到 PING 封包");
            client.Send(new Pong());
        }
    }
}
