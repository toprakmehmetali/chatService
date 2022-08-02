using client;
using client.Config;

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
                
                if (Client.socket.Connected)
                {
                    
                    string metin = Console.ReadLine();
                    Client.SendMessage(metin);

                }
            }
            
            
        }
    }
}