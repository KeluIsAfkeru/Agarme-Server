using Agarme_Server.Misc;
using Agarme_Server.Router;
using HPSocket;


namespace Agarme_Server.NetWork
{
    public class Session
    {
        private GameServer server;

        public Session(GameServer server)
        {
            this.server = server;
            server.OnPrepareListen += new ServerPrepareListenEventHandler(OnPrepareListen);
            server.OnAccept += new ServerAcceptEventHandler(OnAccept);
            server.OnReceive += new ServerReceiveEventHandler(OnReceive);
            server.OnClose += new ServerCloseEventHandler(OnClose);
            server.OnShutdown += new ServerShutdownEventHandler(OnShutDown);
        }

        private HandleResult OnPrepareListen(IServer sender, IntPtr listen) => HandleResult.Ok;


        private HandleResult OnShutDown(IServer sender) => HandleResult.Ok;


        private HandleResult OnAccept(IServer sender, IntPtr connId, IntPtr client)
        {
            Logger.Log($"ID为：{connId}的玩家加入了服务器", LogLevel.Info);

            string clientIp;
            ushort clientPort;
            server.GetRemoteAddress(connId, out clientIp, out clientPort);

            PlayerRouter router = new PlayerRouter(server, connId, (clientIp, clientPort));

            if (!server.JoinRouter(router))
            {
                server.Disconnect(connId, true);
                return HandleResult.Error;
            }

            //....处理成功连接后的逻辑

            return HandleResult.Ok;
        }


        private HandleResult OnClose(IServer sender, IntPtr connId, SocketOperation socketOperation, int errorCode)
        {
            Logger.Log($"ID为：{connId}的玩家退出了游戏", LogLevel.Info);

            server.RemoveRouterById((uint)connId);

            return HandleResult.Ok;
        }


        unsafe private HandleResult OnReceive(IServer sender, IntPtr connId, byte[] data)
        {
            //.......

            return HandleResult.Ok;
        }


    }
}
