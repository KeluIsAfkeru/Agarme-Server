using System.Reflection;
using System.Text;

namespace Agarme_Server
{
    /// <summary>
    /// 世界的配置类
    /// </summary>
    public class ServerConfig
    {
        // Server
        public string ServerIP { get; private set; } = "127.0.0.1"; // 服务端IP
        public ushort ServerPort { get; private set; } = 8080; // 端口
        public bool IsWss { get; private set; } = false; // 是否使用WSS协议
        public string ServerVersion { get; private set; } = "V1.0.0"; // 服务端版本号
        public ushort ServerVersionCode { get; private set; } = 100; // 服务端版本代码
        public string WelcomeMessage { get; set; } = "欢迎加入Afkeru的服务器，祝您玩得愉快！"; // 欢迎语句
        public int MaxPlayerConnections { get; set; } = 3; // 单个玩家的最大连接数
        public bool ClearEjectedMass { get; set; } = false; // 是否清除吐球
        public bool ClearDisconnectedPlayers { get; set; } = true; // 是否清除掉线玩家
        public bool EnableSoloTrick { get; set; } = false; // 是否启用solotrick
        public double EjectedMassClearTime { get; set; } = 560; // 吐球清除时间
        public double DisconnectedPlayerClearTime { get; set; } = 360; // 掉线玩家清除时间
        public int ServerTickInterval { get; set; } = 35; // 服务端处理周期

        // Map
        public int MapLeft { get; set; } = 100; // 地图左上角横坐标
        public int MapTop { get; set; } = 100; // 地图左上角纵坐标
        public int MapWidth { get; set; } = 1500; // 地图宽度
        public int MapHeight { get; set; } = 1500; // 地图高度
        public int MapPlayerLimit { get; set; } = 50; // 一个地图最大承载玩家数
        public int MapIpLimit { get; set; } = 3; // 同Ip最大连接数
        public int MapGameMode { get; set; } = 1; // 游戏模式
        public double MapBorderCoverage { get; set; } = 0.6; // 边界与细胞之间的碰撞覆盖系数
        public bool EnableMapShrinking { get; set; } = false; // 地图是否缩小
        public bool EnableOutOfBorderMassAttenuation { get; set; } = true; // 地图外是否掉体积
        public bool EnableBorderCollision { get; set; } = true; // 是否启用边界碰撞
        public bool EnableBorderCollisionBounce { get; set; } = true; // 是否启用边界碰撞反弹

        // Food
        public int FoodAmount { get; set; } = 9000; // 食物数量
        public int MaxFoodSize { get; set; } = 5; // 食物最大大小
        public int MinFoodSize { get; set; } = 1; // 食物最小大小
        public int FoodSpawnAmount { get; set; } = 50; // 食物刷新数量
        public float FoodSpawnInterval { get; set; } = 1; // 食物刷新时间间隔

        // Player
        public int InitialViewWidth { get; set; } = 500; // 初始视野宽度
        public int InitialViewHeight { get; set; } = 500; // 初始视野高度
        public double InitialPlayerMass { get; set; } = 20000; // 玩家初始质量
        public double CoverageCoefficient { get; set; } = 0.3; // 覆盖系数
        public double DevourSizeCoefficient { get; set; } = 1.3; // 吞噬系数
        public double RebirthWidth { get; set; } = 250; // 重生范围宽度
        public double RebirthHeight { get; set; } = 250; // 重生范围高度
        public double CollisionDeviationConstant { get; set; } = 0.71; // 碰撞偏移常数
        public double PlayerMassDecayRate { get; set; } = 1; // 玩家质量衰减速率

        public double PlayerMoveSpeed { get; set; } = 1.26; // 玩家移动速度
        public double PlayerMoveSpeedDecayRate { get; set; } = 0.247; // 玩家移动速度衰减率

        public bool EnableAutoSplit { get; set; } = false; // 细胞是否自动分裂
        public double AutoSplitMassThreshold { get; set; } = 22500; // 细胞自动分裂的质量阈值
        public double PlayerLimitMass { get; set; } = 6000000; // 细胞质量的极限值

        public double PlayerFusionTime { get; set; } = 12; // 玩家融合时间
        public double PlayerFusionStartMass { get; set; } = 0.1; // 融合初始质量
        public double PlayerFusionCoefficient { get; set; } = 0.12; // 玩家融合系数
        public double PlayerFusionDelay { get; set; } = 10; // 玩家融合延迟

        public int MaxPlayerSplits { get; set; } = 512; // 玩家最大分裂数量
        public int MaxPlayerSplitRequests { get; set; } = 255; // 玩家一次性分裂请求的最大次数
        public double PlayerSplitSpeed { get; set; } = 1.27; // 玩家分裂速度
        public double PlayerSplitSpeedDecayRate { get; set; } = 0.945; // 玩家分裂速度衰减率
        public double MinPlayerSplitMass { get; set; } = 64; // 玩家最小分裂质量
        public double PlayerCollisionTime { get; set; } = 21.9; // 碰撞持续时间
        public double PlayerCollisionCoefficient { get; set; } = 0.38; // 碰撞计算系数
        public int PlayerCollisionAlgorithm { get; set; } = 2; // 碰撞算法

        public int PlayerEjectInterval { get; set; } = 0; // 吐球间隔
        public double MinEjectSize { get; set; } = 42; // 最小吐球质量
        public double EjectSize { get; set; } = 19; // 吐球大小
        public double EjectedMassLoss { get; set; } = 19; // 吐球损失质量
        public double EjectSpeed { get; set; } = 2.7; // 吐球速度
        public double EjectSpeedDecayRate { get; set; } = 0.910; // 吐球速度衰减率
        public bool EnableEjectCollision { get; set; } = false; // 是否启用吐球碰撞
        public double PlayerViewZoom { get; set; } = 0.21; // 玩家视野缩放
        public double PlayerMassZoom { get; set; } = 0.21; // 玩家质量缩放
        public double SpectateViewWidth { get; set; } = 100; // 观战视野宽度
        public double SpectateViewHeight { get; set; } = 100; // 观战视野高度

        // Virus
        public int VirusAmount { get; set; } = 30; // 病毒数量
        public int MaxVirusAmount { get; set; } = 40; // 最大病毒数量
        public double VirusSize { get; set; } = 200; // 病毒大小
        public double VirusSplitSize { get; set; } = 400; // 病毒分裂质量
        public double VirusSplitSpeed { get; set; } = 1.7; // 病毒分裂速度
        public double VirusExplodeSpeed { get; set; } = 1.7; // 病毒炸裂速度
        public double VirusSplitSpeedDecayRate { get; set; } = 0.95; // 病毒分裂速度衰减率
        public bool EnableVirusMovement { get; set; } = false; // 是否启用病毒移动
        public bool EnablePopSplit { get; set; } = true; // 是否启用病毒爆炸分裂

        // PlayerBot/Bot
        public double BotMass { get; set; } = 100; // Bot质量
        public int BotAmount { get; set; } = 1; // Bot数量
        public double MinionMass { get; set; } = 100; // Minion质量
        public int MinionAmount { get; set; } = 10; // Minion数量

        public ServerConfig()
        {

        }

        public ServerConfig(string serverIP, ushort serverPort, string serverVersion, ushort serverVersionCode)
        {
            ServerIP = serverIP;
            ServerPort = serverPort;
            ServerVersion = serverVersion;
            ServerVersionCode = serverVersionCode;
        }

        public override string ToString()
        {
            //使用反射将配置格式化输出
            StringBuilder sb = new StringBuilder();

            Type type = typeof(ServerConfig);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(this);

                sb.AppendLine($"{propertyName}: {propertyValue}");
            }

            return sb.ToString();
        }

        public void ChangeServerIP(string newIP)
        {
            ServerIP = newIP;
        }

        public void ChangeServerPort(ushort newPort)
        {
            ServerPort = newPort;
        }

        public void ChangeServerVersion(string newVersion)
        {
            ServerVersion = newVersion;
        }

        public void ChangeServerVersionCode(ushort newVersionCode)
        {
            ServerVersionCode = newVersionCode;
        }
    }
}