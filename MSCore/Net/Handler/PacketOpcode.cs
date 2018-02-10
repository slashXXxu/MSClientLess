using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCore.Handler
{
    public static class PacketOpcode
    {
        public enum SendPacketOpcode : short
        {
            LOGIN_PASSWORD = 0x01,
            SERVERLIST_REQUEST = 0x03,

            CHAR_SELECT = 0x06,
            PLAYER_LOGGEDIN = 0x07,
            PONG = 0x0E,
        }

        public enum RecvPacketOpcode : short
        {
            LOGIN_STATUS = 0x00,

            SERVER_IP = 0x04,
            CHANGE_CHANNEL = 0x08,
            PING = 0x09,
            SERVERMESSAGE = 0x3D,
        }
    }
}
