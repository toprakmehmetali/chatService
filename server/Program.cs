﻿namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.Config.LoadConfigJson();
            Server.SetServerSettings();
            Server.SetClientsEmptyArray();
            Server.StartServer();
            while (true)
            {
               Server.ListenServer(); 
            }
            
            
        }
        
    }
}