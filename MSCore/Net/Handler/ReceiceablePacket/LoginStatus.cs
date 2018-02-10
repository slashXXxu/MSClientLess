using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapleLib.PacketLib;
using MSCore.Client;
using MSCore.Tools;

namespace MSCore.Net.Handler.ReceiceablePacket
{
    class LoginStatus : AbstractReceiveablePacket
    {

        public LoginStatus() : base(MSCore.Handler.PacketOpcode.RecvPacketOpcode.LOGIN_STATUS)
        {

        }

        public override void HandlePacket(MapleClient client, PacketReader packet)
        {
            int result = packet.ReadByte();
            if(result == 0)
            {
                Logger.Info("帳號 {0} 登入成功", client.Account);
                client.AccountId = packet.ReadInt();
                client.Gender = packet.ReadByte();
            } else
            {
                Logger.Error("帳號 {0} 登入失敗 ， 錯誤代碼 {1}", client.AccountId, result);
            }
        }
    }
}
