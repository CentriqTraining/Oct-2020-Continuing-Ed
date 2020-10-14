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
        public static string GetServiceBusListenerConnectionString()
        {
            string JsonText = File.ReadAllText(@"..\..\..\TimeKeeper\Configuration\SBSvcConfig.json");
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonText);
            return result["secondaryConnectionString"];
        }
    }
}
