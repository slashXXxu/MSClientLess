using MapleLib.PacketLib;
using MSCore.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MSCore.Handler.PacketOpcode;

namespace MSCore.Net.Handler.SendPacket
{
    abstract class AbstractSendablePacket : ISendablePacket
    {
        private PacketWriter packet;

        public AbstractSendablePacket(SendPacketOpcode opcode)
        {
            packet = new PacketWriter();
            packet.WriteShort((short)opcode);
        }

        public byte[] toArray()
        {
            return packet.ToArray();
        }
    }
}
