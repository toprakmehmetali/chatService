using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public class Client
    {
        
        public int id;
        public Tcp tcp;

        public Client(int id)
        {
            this.id = id;
            this.tcp = new Tcp(id);
        }
        public void Disconnect()
        {
            tcp.Disconnect();
         }
        
    }
    


    
}
