using Agarme_Server.Entity;
using Agarme_Server.Misc;
using Agarme_Server.NetWork;
using Agarme_Server.CostomType;
using Agarme_Server.World;

namespace Agarme_Server.Router
{
    public abstract class Router
    {
        private HKVector _deathLocation;
        private HKVector _viewSum;

        protected GameServer server;

        public Map map { get;private set; }
        public string Name { get; set; }
        public uint Color { get; set; }
        public HKVector DeathLocation
        {
            get => _deathLocation;
            set => _deathLocation = value;
        }
        public HKVector ViewSum
        {
            get => _viewSum;
            set => _viewSum = value;
        }
        public double Mass { get; set; }
        public int SpaceCount { get; set; }
        public int LastSplitCount { get; set; }
        public int SplitCount => OwnCells.Count;
        public bool Deleted { get; set; }
        public HkRect View { get; set; }
        public Dictionary<uint, Cell> OwnCells { get; } = new Dictionary<uint, Cell>();
        public ServerConfig Config { get; set; } = new ServerConfig();

        public Router()
        {
            View = new HkRect(HKRand.Double(0, Config.MapWidth), HKRand.Double(0, Config.MapHeight), Config.InitialViewWidth, Config.InitialViewHeight);
            ViewSum = new HKVector();
            DeathLocation = new HKVector();
        }

        public void SetMap(Map map)
        {
            this.map = map;
        }

        public virtual void GenViewWH()
        {
            double s = Math.Pow(Mass * Config.PlayerViewZoom, Config.PlayerMassZoom) + Config.PlayerViewZoom * OwnCells.Count;
            View.Width = Config.InitialViewWidth * s;
            View.Height = Config.InitialViewHeight * s;
        }

        public virtual void UpdateView()
        {
            if (OwnCells.Count == 0) return;

            CalculateViewSumAndMass();

            View.X = ViewSum.X / SplitCount;
            View.Y = ViewSum.Y / SplitCount;
        }

        private void CalculateViewSumAndMass()
        {
            ViewSum = new HKVector();
            Mass = 0;

            foreach (var cell in OwnCells.Values)
            {
                ViewSum.X += cell.X;
                ViewSum.Y += cell.Y;
                Mass += cell.Mass;
            }
        }
    }

    public enum ClientState
    {
        NoWorld,
        Spectating,
        Disconnected,
        Playing
    }
}
