using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapleLib.PacketLib;
using MSCore.Client;
using MSCore.Handler;
using MSCore.Tools;

namespace MSCore.Net.Handler.ReceiceablePacket
{
    class ServerMessage : AbstractReceiveablePacket
    {
        public ServerMessage() : base(PacketOpcode.RecvPacketOpcode.SERVERMESSAGE)
        {

        }

        public override void HandlePacket(MapleClient client, PacketReader packet)
        {
            int channel = -1;
            String message = "";
            int type = packet.ReadByte();
            if (type == 4)
            {
                packet.ReadBool();
                return;
            }
            message = packet.ReadMapleString();
            switch(type)
            {
                case 3:
                case 11:
                case 12:
                    channel = packet.ReadByte() + 1;
                    packet.ReadByte();
                    break;
                case 8:
                    channel = packet.ReadByte() + 1;
                    packet.ReadByte();
                    // TODO: 道具廣
                    break;
                case 9:
                    channel = packet.ReadByte() + 1;
                    break;
                case 10:
                    int lines = packet.ReadByte();
                    if (lines > 1)
                        message += "\r\n" + packet.ReadMapleString();
                    if (lines > 2)
                        message += "\r\n" + packet.ReadMapleString();
                    break;
                
            }
            Logger.Info("[伺服器訊息] {0}", message);
        }
    }
}
