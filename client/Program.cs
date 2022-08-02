using client;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerSettings.ServerAyarla("127.0.0.1", 8081);
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