using client;
using client.Config;
using client.Models;


namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.LoadConfigJson();
            Client.Connect();
            Thread.Sleep(300);
            Client.LoginName();
            while (true)
            {
                if (Client.networkStream != null)
                {
                    string text = Console.ReadLine();
                    Requests.Request(text);
                }
                else
                {
                    break;

                }
            }
            Console.WriteLine("Bir tuşa basarak programı kapatabilirsiniz.");
            Console.ReadKey();
        }
    }
}