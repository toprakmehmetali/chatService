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
            while (true)
            {
               Server.ListenServer(); 
            }
            
            
        }
        
    }
}