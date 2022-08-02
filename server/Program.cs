namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.StartServer(500,8081);
            while (true)
            {
               Server.ListenServer(); 
            }
            
            
        }
        
    }
}