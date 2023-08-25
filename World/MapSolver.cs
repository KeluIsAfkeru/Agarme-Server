using Agarme_Server.NetWork;
using Agarme_Server.Gamemode;
using Agarme_Server.Misc;

namespace Agarme_Server.World
{
    public class MapSolver
    {
        public GameServer server { get; private set; }
        public GameMode gameMode { get; private set; }
        public MapManager mapManager { get; private set; }
        public GameModeManager gameModeManager { get; private set; }
        public List<ServerConfig> configs { get; private set; }
        public GameClock gameClock { get; private set; } 

        public ServerConfig config { get; private set; }

        public MapSolver(List<ServerConfig> configs)
        {
            this.configs = configs;
            config = configs.FirstOrDefault();
            gameModeManager = new GameModeManager();
            gameMode = gameModeManager.GetGameModeById(config.MapGameMode);

            string url = $"{(config.IsWss ? "wss" : "ws")}://{config.ServerIP.Trim()}:{config.ServerPort}";
            server = new GameServer(this, url);

            mapManager = new MapManager(this);

            gameClock = new GameClock(16);
            gameClock.AddCallback(Tick);
        }

        public void Start()
        {
            server.Launch();
            // 启动服务器先初始化一个Map
            var map = mapManager.CreatNewMap();
            if (map is not null)
                Logger.Log($"成功创建新世界[{map.Id}],并加载[{map.cells.Count}]个细胞实体!", LogLevel.System);
            else
                Logger.Log($"初始化世界失败，请稍后重试!", LogLevel.Error);

            gameClock.Start();
        }

        public void StopTick()
        {
            gameClock.Stop();
        }

        public void UpdateGameMode(int gameModeId)
        {
            gameMode = gameModeManager.GetGameModeById(config.MapGameMode);
        }

        public void ReloadConfig()
        {
            gameMode = gameModeManager.GetGameModeById(config.MapGameMode);
        }

        public void ReSetConfig(int gameModeId)
        {
            gameMode = gameModeManager.GetGameModeById(gameModeId);
        }

        public void Tick()
        {
            foreach(Map map in mapManager.maps.Values)
            {
                map.Update();
            }
        }

    }
}
