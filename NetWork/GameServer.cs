using Agarme_Server.Misc;
using Agarme_Server.Protocols;
using Agarme_Server.Router;
using Agarme_Server.World;
using HPSocket.Ssl;
using HPSocket.WebSocket;
using System.Collections.Concurrent;

namespace Agarme_Server.NetWork
{
    public class GameServer : WebSocketServer, IDisposable
    {
        public bool closeServer = false;
        public MapSolver mapSolver;
        public Protocol protocol;
        public string Url => mapSolver.config is null ? "" : $"{(mapSolver.config.IsWss ? "wss" : "ws")}://{mapSolver.config.ServerIP.Trim()}:{mapSolver.config.ServerPort}";

        private ConcurrentDictionary<uint, PlayerRouter> routers;
        private Session session;

        public GameServer(MapSolver mapSolver,string url) : base(url)
        {
            protocol = new Protocol(this);
            session = new Session();
            session.IniServer(this);
            routers = new ConcurrentDictionary<uint, PlayerRouter>();
            this.mapSolver = mapSolver;
        }

        public PlayerRouter this[uint connid]
        {
            get
            {
                routers.TryGetValue(connid, out PlayerRouter router);
                return router;
            }
        }

        public bool Launch()
        {
            try
            {
                IgnoreCompressionExtensions = false;
                PingInterval = 10000;
                // 最大封包大小
                MaxPacketSize = 0x7FFFF;

                // 子协议, 微信接口等会发送自定义的子协议,询问服务器是不是支持, 如果需要配置请再此配置
                SubProtocols = null;

                if (mapSolver.config.IsWss)
                {
                    // wss请开启此设置, 设置ssl配置, 会自动初始化ssl环境
                    SslConfiguration = new SslConfiguration
                    {
                        // 不从内存加载证书
                        FromMemory = false,

                        // ssl证书配置, 支持单向验证
                        VerifyMode = SslVerifyMode.Peer,
                        CaPemCertFileOrPath = "ssl-cert\\ca.crt",
                        PemCertFile = "ssl-cert\\server.cer",
                        PemKeyFile = "ssl-cert\\server.key",
                        KeyPassword = "123456",
                    };
                }

                // 注册ws服务器, 未对path注册服务则无法访问
                AddHub<Session>("/",session);

                // 注册回显服务, 客户端通过ws[s]://127.0.0.1:端口/Game连接
                AddHub<Session>("/Game", session);

                // 启动服务
                bool result = Start();

                if (result)
                    Logger.Log($"服务端启动成功，[{Url}]!", LogLevel.System);
                else
                    Logger.Log("服务端启动失败，请检查端口占用再重试！", LogLevel.Error);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Log($"服务端捕捉到一个连接错误，错误原因为：{ex.Message}", LogLevel.Error);
                return false;
            }
        }

        public bool Restart()
        {
            try
            {
                Logger.Log("尝试重启服务器中...", LogLevel.System);

                mapSolver.StopTick();
                Stop();
                mapSolver.mapManager.ToRestart();
                mapSolver.Start();
                routers.Clear();

                if (Start())
                {
                    var map = mapSolver.mapManager.CreatNewMap();
                    if (map is not null)
                    {
                        Logger.Log($"重启服务器成功！", LogLevel.System);
                        Logger.Log($"成功创建新世界[{map.Id}],并加载[{map.cells.Count}]个细胞实体!", LogLevel.System);
                    }
                    else
                        Logger.Log($"初始化世界失败，请稍后重试!", LogLevel.Error);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Logger.Log($"服务端捕捉到一个连接错误，错误原因为：{ex.Message}", LogLevel.Error);
                return false;
            }
        }

        public bool CloseServer()
        {
            try
            {
                Logger.Log("尝试关闭服务器中...", LogLevel.System);

                mapSolver.StopTick();
                Stop();
                mapSolver.mapManager.ToStop();
                routers.Clear();
                closeServer = true;

                Logger.Log("服务端成功关闭!", LogLevel.System);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"尝试关闭服务端时捕捉到一个错误，错误原因为：{ex.Message}", LogLevel.Error);
                return false;
            }
        }

        public PlayerRouter RemoveRouterById(uint connid)
        {
            PlayerRouter router;
            router = this[connid];
            if(router is not null)
            {
                router.Deleted = true;
                router.State = ClientState.Disconnected;
                mapSolver.mapManager.RemoveRouterAddress(router);
                routers.TryRemove(connid, out router);
            }
            
            return router;
        }

        public PlayerRouter RemoveRouter(PlayerRouter router)
        {
            if (router is not null)
            {
                router.Deleted = true;
                router.State = ClientState.Disconnected;
                mapSolver.mapManager.RemoveRouterAddress(router);
                routers.TryRemove(router.Connid, out router);
            }

            return router;
        }

        public bool JoinRouter(PlayerRouter router) => mapSolver.mapManager.MapJoinPlayer(router) == JoinStatus.Successful && routers.TryAdd(router.Connid, router);

        public PlayerRouter PlayerRouterById(uint connid)
        {
            routers.TryGetValue(connid, out PlayerRouter router);
            return router;
        }

        public void OperateRoutersParallel(Action<PlayerRouter> operation)
        {
            Parallel.ForEach(routers.Values, operation);
        }
    }
}
