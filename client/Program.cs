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
            DataTransferObject dataTransferObject = new DataTransferObject();
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
                    if (metin == "rename")
                    {
                        dataTransferObject.RequestType = "rename";
                        Console.WriteLine("Yeni adınızı giriniz.");
                        dataTransferObject.Request = Console.ReadLine();
                        Client.SendMessage(dataTransferObject);
                        continue;
                    }

                    dataTransferObject.RequestType = "message";
                    dataTransferObject.Request = metin;
                    Client.SendMessage(dataTransferObject);

                }
            }
            
            
        }
    }
}