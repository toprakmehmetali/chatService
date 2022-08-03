namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.Config.LoadConfigJson();
            Server.SetServerSettings();
            Server.SetEmptyArrayClients();
            Server.StartServer(); 
            Server.ListenServer();
            while (true)
            {
                Console.WriteLine("Serveri kapatmak için \"quit\" yazın.");
                var result = Console.ReadLine();
                if ( Equals(result, "quit"))
                {
                    break;
                }
            }
            
        }
        
    }
}