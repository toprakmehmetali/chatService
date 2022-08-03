using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using server.Models;

namespace server.Config
{
    public class Config
    {
        public static ConfigJson ConfigJson;
        public static void LoadConfigJson()
        {
           
            using (StreamReader ProgramConfig = new StreamReader("C:\\project\\chatService\\server\\ProgramConfig.json"))
            {
                var jsonconfig = ProgramConfig.ReadToEnd();
                ConfigJson = JsonConvert.DeserializeObject<ConfigJson>(jsonconfig);
            }
        }
    }
}
