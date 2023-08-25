using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Agarme_Server.Misc;
using Agarme_Server.Gamemode;
using Agarme_Server.World;
using Agarme_Server.Entity;

namespace Agarme_Server
{
    class Dispatcher
    {
        static void Main()
        {
            // 处理配置文件
            List<ServerConfig> configs = SolveConfigs();
            MapSolver mapSolver = new MapSolver(configs);
            mapSolver.Start();

            while (!mapSolver.server.closeServer)
            {
                string input = Console.ReadLine();
                if (input.Trim() == "Stop")
                    mapSolver.server.CloseServer();
            }

            Console.ReadLine();
        }

        static List<ServerConfig> SolveConfigs()
        {
            string configFolderPath = "Configs";
            string configFilePath = Path.Combine(configFolderPath, "Standard.yaml");

            // 检查是否存在Config文件夹，如果不存在则创建
            if (!Directory.Exists(configFolderPath))
                Directory.CreateDirectory(configFolderPath);

            List<ServerConfig> configList = new List<ServerConfig>();

            // 检查配置文件是否存在
            if (Directory.GetFiles(configFolderPath, "*.yaml").Length > 0)
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                // 读取配置文件夹下的所有yaml文件并反序列化为ServerConfig对象
                foreach (string filePath in Directory.GetFiles(configFolderPath, "*.yaml"))
                {
                    string yaml = File.ReadAllText(filePath);
                    var serverConfigDict = deserializer.Deserialize<Dictionary<string, object>>(yaml);
                    var serverConfig = ConvertToServerConfig(serverConfigDict);
                    configList.Add(serverConfig);
                }
            }
            else
            {
                // 创建新的配置文件并实例化ServerConfig
                ServerConfig serverConfig = new ServerConfig();

                // 将ServerConfig实例加入配置集合
                configList.Add(serverConfig);

                // 序列化ServerConfig并保存到配置文件中
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                string yaml = serializer.Serialize(ConvertToDictionary(serverConfig));
                File.WriteAllText(configFilePath, yaml);
            }

            Logger.Log("配置处理完成！", LogLevel.System);

            return configList;
        }

        static ServerConfig ConvertToServerConfig(Dictionary<string, object> configDict)
        {
            var serverConfig = new ServerConfig();
            var properties = typeof(ServerConfig).GetProperties();

            foreach (var kvp in configDict)
            {
                var property = Array.Find(properties, prop => prop.Name.Equals(kvp.Key, StringComparison.OrdinalIgnoreCase));
                if (property != null && property.CanWrite)
                {
                    var convertedValue = Convert.ChangeType(kvp.Value, property.PropertyType);
                    property.SetValue(serverConfig, convertedValue);
                }
            }

            return serverConfig;
        }

        static Dictionary<string, object> ConvertToDictionary(ServerConfig serverConfig)
        {
            var configDict = new Dictionary<string, object>();
            var properties = typeof(ServerConfig).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(serverConfig);
                configDict[property.Name] = value;
            }

            return configDict;
        }
    }
    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}