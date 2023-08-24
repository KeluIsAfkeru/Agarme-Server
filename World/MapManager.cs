using Agarme_Server.Misc;
using Agarme_Server.Router;
using System.Collections.Concurrent;

namespace Agarme_Server.World
{
    public enum JoinStatus
    {
        Successful,
        Banned,
        ReachIpLimit,
        Unknow
    }
    /// <summary>
    /// 管理和分配世界
    /// </summary>
    public class MapManager
    {
        public ConcurrentDictionary<uint, Map> maps = new ConcurrentDictionary<uint, Map>();
        public MapSolver mapSolver;
        private ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
        private Dictionary<string, List<PlayerRouter>> ipList = new Dictionary<string, List<PlayerRouter>>();
        private List<string> banList = new List<string>();
        private uint presentMaxId;

        public MapManager(MapSolver mapSolver)
        {
            this.mapSolver = mapSolver;
        }

        /// <summary>
        /// 为玩家分配世界
        /// </summary>
        /// <param name="router"></param>
        /// <returns></returns>
        public JoinStatus MapJoinPlayer(PlayerRouter router)
        {
            rwLock.EnterReadLock();
            try
            {
                // Check if the player IP is in the blacklist
                if (banList.Contains(router.Address.Ip))
                {
                    Logger.Log($"客户[{router.Connid}] IP被拉黑，加入服务器失败！IP:[{router.Address.Ip}]", LogLevel.Error);
                    return JoinStatus.Banned;
                }
            }
            finally
            {
                rwLock.ExitReadLock();
            }

            List<PlayerRouter> ipRouters;
            rwLock.EnterWriteLock();
            try
            {
                // Check if the player IP has exceeded the maximum number of connections per IP
                if (!ipList.TryGetValue(router.Address.Ip, out ipRouters))
                {
                    ipRouters = new List<PlayerRouter>(mapSolver.config.MapIpLimit);
                    ipList.Add(router.Address.Ip, ipRouters);
                }

                if (ipRouters.Count >= mapSolver.config.MapIpLimit)
                {
                    // Exceeded the maximum number of connections of the same IP, prohibiting joining
                    Logger.Log($"客户[{router.Connid}] 超出了同IP最大连接数，加入服务器失败！当前同IP连接数:[{ipRouters.Count}]", LogLevel.Error);
                    return JoinStatus.ReachIpLimit;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }

            Map map = AssignMapToPlayer(router);
            ipRouters.Add(router);
            router.State = ClientState.Spectating;
            map.RequestJoinMap(router);
            mapSolver.gameMode.OnPlayerRequestJoin(router);

            return JoinStatus.Successful;
        }

        public Map CreatNewMap()
        {
            Map map = new Map(this);
            presentMaxId = map.Id;
            maps.TryAdd(map.Id, map);
            map.InitialEntity();
            mapSolver.gameMode.OnMapCreate(map);
            return map;
        }

        /// <summary>
        /// 通过房间的ID来销毁房间
        /// </summary>
        /// <param name="id"></param>
        public void DestroyMapById(uint id)
        {
            Map mapToDestroy;
            if (maps.TryGetValue(id, out mapToDestroy))
            {
                mapSolver.gameMode.OnMapDestory(mapToDestroy);
                mapToDestroy.RemoveAll(ClientState.NoWorld);
                maps.TryRemove(id, out mapToDestroy);
            }
        }

        public void ListAllMaps()
        {
            Logger.Log("当前所有的世界如下：", LogLevel.System);
            foreach (KeyValuePair<uint, Map> map in maps)
            {
                Logger.Log($"ID:[{map.Key}]", LogLevel.System);
            }
        }

        public void RemoveAllMap(ClientState stateToSet)
        {
            foreach(var map in maps.Values)
                map.RemoveAll(stateToSet);
            maps.Clear();
            presentMaxId = 1;
        }

        public void ToRestart()
        {
            ipList.Clear();
            RemoveAllMap(ClientState.NoWorld);
        }

        public void ToStop()
        {
            ipList.Clear();
            RemoveAllMap(ClientState.Disconnected);
        }

        public Map GetMapById(uint id)
        {
            Map map;
            maps.TryGetValue(id, out map);
            return map;
        }

        public void RemoveRouterAddress(PlayerRouter router)
        {
            if (router is null) return;

            string ip = router.Address.Ip;
            rwLock.EnterWriteLock();
            try
            {
                if (ipList.ContainsKey(ip))
                {
                    List<PlayerRouter> sameIpRouters = ipList[ip];
                    if (sameIpRouters.Count is not 0)
                        sameIpRouters.RemoveAt(0);
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        private Map AssignMapToPlayer(PlayerRouter router)
        {
            Map map;
            // 如果存在房间，先看最后一个房间是否满了
            // 如果最新的一个房间满了或者还没有房间，就创建个新房间并分配给用户
            if (maps.Count > 0)
            {
                map = GetMapById(presentMaxId);
                if (map != null && map.routerCount < mapSolver.config.MapPlayerLimit)
                {
                    router.SetMap(map);
                }
                else
                {
                    map = CreatNewMap();
                    router.SetMap(map);
                }
            }
            else
            {
                map = CreatNewMap();
                router.SetMap(map);
            }

            return map;
        }
    }
}
