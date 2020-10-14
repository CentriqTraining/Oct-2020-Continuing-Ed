using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Azure.Configuration
{
    public static class ConnectionManager
    {
        public static string  GetRedisConnectionString()
        {
            string ConnectionTemplate = "CentriqRedisDemo.redis.cache.windows.net:6380,password={PWD},ssl=True,abortConnect=False";
            string JsonText = File.ReadAllText(@"Configuration\Config.json");
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonText);
            return ConnectionTemplate.Replace("{PWD}",
                    result["secondaryKey"]);
        }
        public static string GetServiceBusSenderConnectionString()
        {
            string JsonText = File.ReadAllText(@"Configuration\SBSenderConfig.json");
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonText);
            return result["secondaryConnectionString"];
        }
    }
}
