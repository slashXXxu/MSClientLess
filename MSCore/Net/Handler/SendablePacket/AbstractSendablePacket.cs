using MapleLib.PacketLib;
using MSCore.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MSCore.Handler.PacketOpcode;
using MSCore.Client;

namespace MSCore.Net.Handler.SendablePacket
{
    public abstract class AbstractSendablePacket : AbstractPacket
    {
        protected PacketWriter packet;

        public AbstractSendablePacket(SendPacketOpcode opcode)
        {
            packet = new PacketWriter();
            packet.WriteShort((short)opcode);
        }
        public abstract void WriteBody(MapleClient client);

        public override byte[] toArray()
        {
            return packet.ToArray();
        }
    }
}
