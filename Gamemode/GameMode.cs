using Agarme_Server.Entity;
using Agarme_Server.Misc;
using Agarme_Server.Router;
using Agarme_Server.World;

namespace Agarme_Server.Gamemode
{
    /// <summary>
    /// 提供GameMode的规范，你可以自己设计模式
    /// </summary>
    public abstract class GameMode
    {
        public MapSolver mapSolver;
        public int identity;

        private static readonly IdAllocator Allocator = new IdAllocator();

        public GameMode() 
        {
            //不要在这个构造函数里加参数，否则会报错
            identity = (int)Allocator.Allocate();
        }

        public void SetMapSolver(MapSolver mapSolver)
        {
            this.mapSolver = mapSolver;
        }

        public virtual void OnPlayerRequestJoin(PlayerRouter router) { }
        public virtual void OnPlayerSuccessfulJoin(PlayerRouter router) { }
        public virtual void OnPlayerRemove(PlayerRouter router) { }
        public virtual void OnPlayerSpawn(PlayerRouter router, Player player) { }
        public virtual void OnCellSpawn(Cell cell) { }
        public virtual void OnMapCreate(Map map) { }
        public virtual void OnMapDestory(Map map) { }
    }
}
