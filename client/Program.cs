using client;
using client.Config;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.LoadConfigJson();
            Client.Connect();
            Thread.Sleep(500);
            Client.LoginName();
            Client.StartWrite();
        }
    }
}