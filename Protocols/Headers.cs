namespace Agarme_Server.Protocols;

internal class ProtocolHeaders
{
    /* Receive Headers */
    public const byte ReceiveIP = 19; // 与客户端成功握手，客户端返回IP

    /* Send Headers */
    public const byte SendAfterJoin = 15; // 连接成功接收到客户端的返回数据后，发送地图数据和玩家所属
}
