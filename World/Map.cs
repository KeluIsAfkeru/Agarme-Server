using Agarme_Server.Entity;
using Agarme_Server.Misc;
using Agarme_Server.Router;
using System.Diagnostics.Metrics;

namespace Agarme_Server.World
{
    public class Map
    {
        private static readonly IdAllocator Allocator = new IdAllocator();

        public uint Id;
        public MapManager mapManager;
        public ServerConfig config;
        public List<Cell> cells = new List<Cell>();
        public List<Food> foods = new List<Food>();
        public List<Virus> viruses = new List<Virus>();
        public List<Player> players = new List<Player>();
        public List<Minion> minions = new List<Minion>();
        public int routerCount { get => routers.Count; }
        private Dictionary<uint, PlayerRouter> routers = new Dictionary<uint, PlayerRouter>();
        private Queue<PlayerRouter> routerQueue = new Queue<PlayerRouter>();

        public Map(MapManager mapManager)
        {
            this.mapManager = mapManager;
            config = mapManager.mapSolver.config;
            Id = Allocator.Allocate();
        }

        public unsafe void Update()
        {
            HandleJoinRequests();

        }

        public void InitialEntity()
        {
            // 初始化食物
            int i = 0;
            while( i < config.FoodAmount )
            {
                Food food = new Food(this);
                i++;
            }
            i = 0;
            while (i < config.VirusAmount)
            {
                Virus virus = new Virus(this);
                i++;
            }
        }

        public void RemoveAll(ClientState stateToSet)
        {
            Allocator.Reset();
            for (int i = 0; i < routers.Count; ++i)
            {
                var router = routers.ElementAt(i).Value;
                router.State = stateToSet;
            }
            routers.Clear();
            cells.Clear();
            foods.Clear();
            viruses.Clear();
            players.Clear();
            minions.Clear();
        }

        public void RequestJoinMap(PlayerRouter router)
        {
            routerQueue.Enqueue(router);
        }

        private void HandleJoinRequests()
        {
            if(routerQueue.Count is not 0)
            {
                PlayerRouter router = routerQueue.Dequeue();
                routers.Add(router.Connid, router);
                mapManager.mapSolver.gameMode.OnPlayerSuccessfulJoin(router);
            }
        }

        public void RemoveRouter(uint connid)
        {
            if (routers.ContainsKey(connid))
            {
                mapManager.mapSolver.gameMode.OnPlayerRemove(routers[connid]);
                routers.Remove(connid);
            }
        }
    }
}
