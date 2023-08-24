using Agarme_Server.Misc;
using Agarme_Server.Router;
using Agarme_Server.World;
using HPSocket.Tcp;
using System.Collections.Concurrent;

namespace Agarme_Server.NetWork
{
    public class GameServer : TcpPackServer, IDisposable
    {
        public bool closeServer = false;
        public MapSolver mapSolver;
        private ConcurrentDictionary<uint, PlayerRouter> routers;
        private Session session;

        public GameServer(MapSolver mapSolver) : base()
        {
            PackHeaderFlag = 0x169;
            MaxPackSize = 0x3FFFFF;
            this.mapSolver = mapSolver;
            session = new Session(this);
            routers = new ConcurrentDictionary<uint, PlayerRouter>();
        }

        public PlayerRouter this[uint connid]
        {
            get
            {
                routers.TryGetValue(connid, out PlayerRouter router);
                return router;
            }
        }

        public bool Launch(string ip, ushort port)
        {
            try
            {
                Address = ip.Trim();
                Port = port;
                bool result = Start();

                if (result)
                    Logger.Log($"服务端启动成功，[{Address}:{Port}]!", LogLevel.System);
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

        public bool Close()
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
