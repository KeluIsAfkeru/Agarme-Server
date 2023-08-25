using Agarme_Server.Misc;
using Agarme_Server.Protocols;
using Agarme_Server.Router;
using HPSocket;
using HPSocket.WebSocket;
using System.Collections.Concurrent;

namespace Agarme_Server.NetWork
{
    public class Session: IHub
    {
        private ConcurrentDictionary<uint,PlayerRouter> playersToJoin;
        private GameServer server;
        private Protocol protocol;

        public Session()
        {
            playersToJoin = new ConcurrentDictionary<uint, PlayerRouter>(Environment.ProcessorCount, 20);
        }

        public void IniServer(GameServer server)
        {
            this.server = server;
            protocol = server.protocol;
        }

        public HandleResult OnOpen(IWebSocketServer sender, IntPtr connId)
        {
            PlayerRouter router = new PlayerRouter(server, connId);

            // 将新加入的用户放入待加入队列中
            playersToJoin.TryAdd((uint)connId, router);

            return HandleResult.Ok;
        }


        public HandleResult OnClose(IWebSocketServer sender, IntPtr connId, SocketOperation socketOperation, int errorCode)
        {
            Logger.Log($"ID为：{connId}的玩家退出了游戏", LogLevel.Info);

            server.RemoveRouterById((uint)connId);

            return HandleResult.Ok;
        }


        public HandleResult OnMessage(IWebSocketServer sender, IntPtr connId, bool final, OpCode opCode, byte[] mask, byte[] data)
        {
            PlayerRouter router = server.PlayerRouterById((uint)connId);

            protocol.handleIpReceived(data, IpReceivedCallback(connId,router));


            return HandleResult.Ok;
        }

        public void OnPing(IWebSocketServer sender, IntPtr connId, byte[] data)
        {
        }

        public void OnPong(IWebSocketServer sender, IntPtr connId, byte[] data)
        {

        }

        private Action<string> IpReceivedCallback(IntPtr connId,PlayerRouter router)
        {
            return (address) =>
            {
                if (router is null && !playersToJoin.TryGetValue((uint)connId, out router))
                {
                    LogAndCloseConnection("接收到消息的玩家不在待加入队列中", LogLevel.Error, connId);
                    return;
                }

                try
                {
                    router.SetAddress(address, server.mapSolver.config.ServerPort);

                    if (!server.JoinRouter(router))
                    {
                        LogAndCloseConnection($"服务器加入路由失败：{router.Id}", LogLevel.Error, connId);
                        return;
                    }

                    if (!playersToJoin.TryRemove(router.Id, out _))
                        Logger.Log($"移除玩家失败：{router.Id}", LogLevel.Error);

                    protocol.sendAfterJoin(router);

                    Logger.Log($"ID为：{connId}的玩家加入了服务器", LogLevel.Info);
                }
                catch (Exception ex)
                {
                    LogAndCloseConnection($"处理IP接收协议时出错：{ex}", LogLevel.Error, connId);
                    return;
                }
            };
        }

        private void LogAndCloseConnection(string message, LogLevel level, IntPtr connId)
        {
            Logger.Log(message, level);
            server.Close(connId);
        }

    }

}