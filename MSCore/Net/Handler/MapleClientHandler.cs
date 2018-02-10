using MSCore.Handler;
using MSCore.Net.Handler.ReceiceablePacket;
using MSCore.Net.Handler.SendPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCore.Net.Handler
{
    public class MapleClientHandler
    {

        static MapleClientHandler _instance;

        Dictionary<int, AbstractPacket> recvPackets;

        public MapleClientHandler()
        {
            recvPackets = new Dictionary<int, AbstractPacket>();
            Init();
        }

        public static MapleClientHandler Instance { get {
                if (_instance == null)
                    _instance = new MapleClientHandler();
                return _instance;
            } }

        public void Init()
        {
            var type = typeof(AbstractReceiveablePacket);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => !type.Equals(p))
                .Where(p => type.IsAssignableFrom(p));

            foreach (var _type in types)
            {
                AbstractReceiveablePacket packet = (AbstractReceiveablePacket)Activator.CreateInstance(_type);
                if (!recvPackets.ContainsKey(packet.Opcode))
                {
                    recvPackets.Add(packet.Opcode, packet);
                }
            }
        }
    }
}
