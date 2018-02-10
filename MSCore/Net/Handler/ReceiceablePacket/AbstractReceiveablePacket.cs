using MSCore.Net.Handler.SendPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSCore.Client;
using MapleLib.PacketLib;
using MSCore.Handler;

namespace MSCore.Net.Handler.ReceiceablePacket
{
    public abstract class AbstractReceiveablePacket : AbstractPacket
    {
        PacketReader packet;

        public AbstractReceiveablePacket(PacketOpcode.RecvPacketOpcode Opcode)
        {
            this.Opcode = (int)Opcode;
        }

        public void SetPacket(PacketReader packet)
        {
            this.packet = packet;
        }

        public abstract void HandlePacket(MapleClient client, PacketReader packet);
       

        public override byte[] toArray()
        {
            throw new NotImplementedException();
        }
    }
}
