using client;
using client.Config;
using client.Models;
using ServerSettings = client.ServerSettings;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Config.LoadConfigJson();
            ServerSettings.SetServerSettings();
            Client.Connect();
            Console.WriteLine("Bağlanıldı");
            Console.WriteLine("metin girin");
            while (true)
            {
                
                if (Client.networkStream.CanRead)
                {
                    string text = Console.ReadLine();
                    Requests.Request(text);
                }
            }
            
            
        }
    }
}