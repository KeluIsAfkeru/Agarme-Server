using Agarme_Server.Misc;
using Agarme_Server.NetWork;
using Agarme_Server.CostomType;

namespace Agarme_Server.Router
{
    public class PlayerRouter: Router
    {
        public HKVector Mouce { set; get; }
        public ClientState State { set; get; }
        public uint Connid { get; protected set; }
        public (string Ip , ushort Port) Address { private set; get; }
        public bool Eject { set; get; }
        public bool Split { set; get; }
        public bool MinionEject { set; get; }
        public bool MinionSplit { set; get; }

        public PlayerRouter(GameServer _server, IntPtr _connId, (string Ip, ushort Port) _address) : base()
        {
            server = _server;
            State = ClientState.NoWorld;
            Connid = (uint)_connId;

            double center_x = Config.MapWidth * 0.5;
            double center_y = Config.MapHeight * 0.5;

            Mouce = new HKVector(center_x, center_y);
            Address = _address;
        }

        public bool Send(List<byte> val) => server.Send((nint)Connid, val.ToArray(), val.Count);
        public bool Send(byte[] val) => server.Send((nint)Connid, val.ToArray(), val.Length);

    }
}
