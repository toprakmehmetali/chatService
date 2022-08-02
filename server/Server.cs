
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using server.Models;
using server.Config;

namespace server
{
    public class Server
    {
        public static int _maxUser { get;private set; }
        public static int _port { get; private set;}
        public static TcpListener serverListener;

        public static Tcp[] clients;

        public static void StartServer()
        {
            
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
                if (client.socket == null)
                {
                    client.Connect(socket);
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
                clients[i] = new Tcp(i);
            }
        }

        public static void SendMessageAllSocket(int id ,string Message)
        {
            foreach (var client in clients)
            {
                if (client.id != id)
                {
                    client.SendMessage(Message);
                }
            }
        }
        
        public static void SetServerSettings()
        {
            _maxUser = Config.Config.ConfigJson.ServerSettings.MaxUser;
            _port = Config.Config.ConfigJson.ServerSettings.Port;
        }

        public static void SetClientsEmptyArray()
        {
            clients = new Tcp[_maxUser];
        }
    }
}
