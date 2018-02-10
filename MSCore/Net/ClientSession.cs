using MapleLib.PacketLib;
using MSCore.Client;
using MSCore.Net.Handler;
using MSCore.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSCore.Net
{
    public class ClientSession
    {
        private const int MAXBUFFER = 16000;

        private Session session = null;

        public bool Connected { get { return session != null ? session.Connected : false; } }

        public bool isInit { get; private set; }

        private MapleClientHandler PacketHandler;

        private MapleClient Client;


        public ClientSession(MapleClient client)
        {
            this.Client = client;
            this.PacketHandler = new MapleClientHandler();
        }

        public void Connect(string ip, int port)
        {
            Logger.Debug("嘗試連接至伺服器 {0}:{1}", ip, port);
            try
            {
                Socket outSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                outSocket.BeginConnect(ip, port, new AsyncCallback(OnOutConnectCallback), outSocket);
            }
            catch { OnClientDisconnected(null); }
        }

        public void Disconnect()
        {
            Logger.Debug("客戶端主動關閉連線");
            if(Connected && session.Connected)
            {
                session.Socket.Disconnect(false);
            }
        }


        private void OnOutConnectCallback(IAsyncResult ar)
        {
            Socket sock = (Socket)ar.AsyncState;
            try
            {
                sock.EndConnect(ar);
            }
            catch
            {
                Logger.Error("客戶端無法連上");
                return;
            }
            Logger.Info("連接至伺服器 {0} 成功", sock.RemoteEndPoint.ToString());
            session = new Session(sock, SessionType.CLIENT_TO_SERVER);
            session.OnInitPacketReceived += new Session.InitPacketReceived(OnInitPacketReceived);
            session.OnPacketReceived += new Session.PacketReceivedHandler(OnPacketReceived);
            session.OnClientDisconnected += new Session.ClientDisconnectedHandler(OnClientDisconnected);
            session.WaitForDataNoEncryption();
        }

        private volatile Mutex mutex2 = new Mutex();

        void OnPacketReceived(byte[] packet)
        {
            if (!isInit || !Connected)
            {
                return;
            }
            mutex2.WaitOne();
            try
            {
                PacketHandler.HandlePacket(Client, new PacketReader(packet));
            }
            finally
            {
                mutex2.ReleaseMutex();
            }
        }

        void OnInitPacketReceived(short version, string patchVer, byte locale)
        {
            isInit = true;
            Logger.Info("伺服器端版本 {0}.{1} 地區 {2}", version, patchVer, locale);
        }

        void OnClientDisconnected(Session session)
        {
            Logger.Info("客戶端從 {0} 斷線", session.Socket.RemoteEndPoint.AddressFamily.ToString());
        }

        public void Send(byte[] data)
        {
            session.SendPacket(data);
        }

    }
}
