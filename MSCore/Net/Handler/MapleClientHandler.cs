using MapleLib.PacketLib;
using MSCore.Client;
using MSCore.Handler;
using MSCore.Net.Handler.ReceiceablePacket;
using MSCore.Net.Handler.SendPacket;
using MSCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCore.Net.Handler
{
    public class MapleClientHandler
    {

        Dictionary<int, AbstractReceiveablePacket> recvPackets;

        public MapleClientHandler()
        {
            recvPackets = new Dictionary<int, AbstractReceiveablePacket>();
            Init();
        }

        private void Init()
        {
            var type = typeof(AbstractReceiveablePacket);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => !type.Equals(p))
                .Where(p => type.IsAssignableFrom(p));

            foreach (var _type in types)
            {
                AbstractReceiveablePacket packet = (AbstractReceiveablePacket)Activator.CreateInstance(_type, new object[] { });
                if (!recvPackets.ContainsKey(packet.Opcode))
                {
                    recvPackets.Add(packet.Opcode, packet);
                }
            }
        }

        public void HandlePacket(MapleClient client, PacketReader packet)
        {
            int opcode = packet.ReadShort();
            if(recvPackets.ContainsKey(opcode))
            {
                Logger.Debug("[接收封包] OPCODE: {0}(0x{1:X4}) ALL: {2}",
               Enum.GetName(typeof(PacketOpcode.SendPacketOpcode), opcode),
               opcode,
               HexUtil.ByteArrayToString(packet.ToArray()));
               recvPackets[opcode].HandlePacket(client, packet);
            }
            else
            {
                Logger.Warning("封包 OPCODE:{0} 未處理", String.Format("0x{0:X4}", opcode));
            }
        }
    }
}
