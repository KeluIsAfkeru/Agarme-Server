using Agarme_Server.Misc;
using Agarme_Server.World;
using System.Reflection;

namespace Agarme_Server.Gamemode
{
    /// <summary>
    /// 游戏模式管理器
    /// </summary>
    public class GameModeManager
    {
        private Dictionary<int, GameMode> gameModes;

        public GameModeManager()
        {
            gameModes = new Dictionary<int, GameMode>();
            UpdateGameModes();
        }

        private void UpdateGameModes()
        {
            gameModes.Clear();

            var gameModeType = typeof(GameMode);
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(p => gameModeType.IsAssignableFrom(p) && !p.IsAbstract);

            foreach (var type in types)
            {
                var gameMode = Activator.CreateInstance(type) as GameMode;
                gameModes.Add(gameMode.identity, gameMode);
            }
        }

        public T GetGameMode<T>() where T : GameMode
        {
            foreach (var gameMode in gameModes.Values)
            {
                if (gameMode is T)
                    return gameMode as T;
            }
            return null;
        }

        public GameMode GetGameModeById(int id)
        {
            GameMode mode;
            gameModes.TryGetValue(id, out mode);
            return mode;
        }

        public void ListGameModes()
        {
            Logger.Log("当前支持的游戏模式如下：", LogLevel.System);
            foreach (var gameMode in gameModes.Values)
            {
                Logger.Log($"ID: {gameMode.identity}, Name: {gameMode.GetType().Name}", LogLevel.System);
            }
        }

        public void RefreshGameModes()
        {
            UpdateGameModes();
        }
    }
}
