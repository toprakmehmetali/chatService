
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public class Server
    {
        public static int _maxUser { get;private set; }

        public static int _port { get; private set;}

        public static TcpListener serverListener;

        public static SortedDictionary<int, Client> clients = new SortedDictionary<int, Client>();

        public static void StartServer(int maxUser, int port)
        {
            _maxUser=maxUser;
            _port=port;
            InitializeServerData();
            serverListener = new TcpListener(IPAddress.Any, _port);
            Console.WriteLine($"Server Kuruldu ! : Maksimum user sayısı {_maxUser} : Dinlenen port {_port}");
            serverListener.Start();
            Console.WriteLine("Server Başlatıldı");
            Console.WriteLine("Kullanıcılar Bekleniyor");
        }

        public static void ListenServer()
        {
            serverListener.BeginAcceptTcpClient(AcceptClientCallBack, null);
            
        }

        public static void AcceptClientCallBack(IAsyncResult asyncResult)
        {
            TcpClient socket = serverListener.EndAcceptTcpClient(asyncResult);
            foreach (var client in clients)
            {
                if (client.Value.tcp.socket == null)
                {
                    client.Value.tcp.Connect(socket);
                    return;
                }
            }
            socket.Close();
            Console.WriteLine("server Dolu.");
        }

        public static void InitializeServerData()
        {
            for (int i = 0; i < _maxUser; i++)
            {
               clients.Add(i,new Client(i));
            }
        }

        public static void SendAllSocketMessage(int id ,string Message)
        {
            foreach (var client in clients)
            {
                if (client.Value.id != id)
                {
                    client.Value.tcp.SendMessage(Message);
                }
                
            }
        }
    }
}
