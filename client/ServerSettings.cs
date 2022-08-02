using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    public class ServerSettings
    {
        public static string Host { get; set; }
        public static int Port { get; set; }

        public static void SetServerSettings()
        {
            Host = Config.Config.ConfigJson.ServerSettings.Host;
            Port = Config.Config.ConfigJson.ServerSettings.Port;
        }
    }
}
