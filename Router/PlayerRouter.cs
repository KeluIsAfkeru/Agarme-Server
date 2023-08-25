using Agarme_Server.Misc;
using Agarme_Server.NetWork;
using Agarme_Server.CostomType;
using HPSocket.WebSocket;

namespace Agarme_Server.Router
{
    public class PlayerRouter: Router
    {
        public HKVector Mouce { set; get; }
        public ClientState State { set; get; }
        public uint Connid { get; private set; }
        public uint Id { get; private set; }
        public (string Ip , ushort Port) Address { private set; get; }
        public bool Eject { set; get; }
        public bool Split { set; get; }
        public bool MinionEject { set; get; }
        public bool MinionSplit { set; get; }

        private static readonly IdAllocator Allocator = new IdAllocator();

        public PlayerRouter(GameServer _server, IntPtr _connId) : base()
        {
            server = _server;
            State = ClientState.NoWorld;
            Connid = (uint)_connId;

            AllocateId();

            double center_x = Config.MapWidth * 0.5;
            double center_y = Config.MapHeight * 0.5;

            Mouce = new HKVector(center_x, center_y);
        }

        public void AllocateId()
        {
            Id = Allocator.Allocate();
        }


        public void RecycleId(uint identity)
        {
            Allocator.Recycle(identity);
        }


        public void ResetIds()
        {
            Allocator.Reset();
        }

        public void SetAddress(string ip, ushort port)
        {
            Address = new (ip, port);
        }


        public bool Send(List<byte> val) => server.Send((nint)Connid, OpCode.Binary, val.ToArray(), val.Count);
        public bool Send(byte[] val) => server.Send((nint)Connid, OpCode.Binary, val, val.Length);

    }
}
