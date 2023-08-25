using Agarme_Server.CostomType;
using Agarme_Server.Entity;
using Agarme_Server.Misc;
using Agarme_Server.NetWork;
using Agarme_Server.Router;
using Agarme_Server.World;
using MessagePack;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Agarme_Server.Protocols
{
    public class Protocol
    {
        private GameServer server;

        public Protocol(GameServer server)
        {
            this.server = server;
        }

        public void handleIpReceived(byte[] data, Action<string> process)
        {
            if (data.Length is 0)
                return;

            int offset = 0;

            if (data[offset++] != ProtocolHeaders.ReceiveIP)
                return;

            string address = BufferReader.ReadStr(data, data.Length - 1, ref offset);

            process(address);
        }

        public bool sendAfterJoin(PlayerRouter router)
        {
            bool result = false;

            if (router is null)
                return result;

            try
            {
                byte[] bytes = new byte[37];
                HkRect mapRect = router.map.Border;

                BufferWriter.WriteUnmanaged(ProtocolHeaders.SendAfterJoin, bytes, 0);
                BufferWriter.WriteUnmanaged(router.Id, bytes, 1);
                BufferWriter.WriteUnmanaged(mapRect.X, bytes, 5);
                BufferWriter.WriteUnmanaged(mapRect.Y, bytes, 13);
                BufferWriter.WriteUnmanaged(mapRect.Width, bytes, 21);
                BufferWriter.WriteUnmanaged(mapRect.Height, bytes, 29);

                result = router.Send(bytes);
            }
            catch (Exception ex)
            {
                Logger.Log($"在尝试序列化并发送初始静态数据时出现错误，错误原因：{ex.Message}", LogLevel.Error);
            }

            return result;
        }
    }

}
