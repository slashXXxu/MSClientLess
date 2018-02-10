using MSCore.Net.Handler.SendPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSCore.Handler;
using static MSCore.Handler.PacketOpcode;

namespace MSCore.Net.Handler.SendablePacket
{
    class LoginPassword : AbstractSendablePacket
    {
        public LoginPassword() : base(SendPacketOpcode.LOGIN_PASSWORD)
        {
        }
    }
}
