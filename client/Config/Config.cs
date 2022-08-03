using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.Models;
using Newtonsoft.Json;

namespace client.Config
{
    public class Config
    {
        public static ConfigJson ConfigJson;
        public static void LoadConfigJson()
        {
            string path = Directory.GetCurrentDirectory();
            path = path.Replace("bin\\Debug\\net6.0", "").Replace("\\", "\\\\");
            using (StreamReader ProgramConfig = new StreamReader($"{path}ProgramConfig.json"))
            {
                var jsonconfig = ProgramConfig.ReadToEnd();
                ConfigJson = JsonConvert.DeserializeObject<ConfigJson>(jsonconfig);
            }
        }
    }

}
