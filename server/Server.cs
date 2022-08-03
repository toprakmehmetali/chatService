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

        public static TcpUser[] clients;

        public static Tcp tcp;

        public static void StartServer()
        {
            InitializeServerData();
            serverListener = new TcpListener(IPAddress.Any, _port);
            Console.WriteLine($"Server Kuruldu ! : Maksimum user sayısı {_maxUser} : Dinlenen port {_port}");
            serverListener.Start();
            Console.WriteLine(Messages.Messages.ServerStart);
            Console.WriteLine(Messages.Messages.WaitingForUsersToConnect);
        }

        public static void ListenServer()
        {
            while (true)
            {
                serverListener.BeginAcceptTcpClient(AcceptClientCallBack, null);
            }
        }

        public static void AcceptClientCallBack(IAsyncResult asyncResult)
        {
            TcpClient socket = serverListener.EndAcceptTcpClient(asyncResult);
            foreach (var client in clients)
            {
                if (client.Socket == null)
                {
                    client.Connect(socket);
                    return;
                }
            }
            
            tcp.ConnectSocket(socket);
            tcp.SendMessage("Sunucu dolu.");
            tcp.DisconnectSocket();
            Console.WriteLine(Messages.Messages.ServerFull);
        }

        public static void InitializeServerData()
        {
            for (int i = 0; i < _maxUser; i++)
            {
                clients[i] = new TcpUser(i);
                clients[i].Name = $"{i}.Anonymous";
            }
        }

        public static void SendMessageAllSocket(int id ,string message)
        {
            foreach (var client in clients)
            {
                if (client.Id != id)
                {
                    client.SendMessage(message);
                }
            }
        }

        public static void SendMessageById(int id, string message)
        {
            clients[id].SendMessage(message);
        }
        public static void SendMessageByName(string name,string message)
        {
            foreach (var client in clients)
            {
                if (client.Name == name)
                {
                    client.SendMessage(message);
                }
            }
        }

        public static void SetServerSettings()
        {
            _maxUser = Config.Config.ConfigJson.ServerSettings.MaxUser;
            _port = Config.Config.ConfigJson.ServerSettings.Port;
        }

        public static void SetEmptyArrayClients()
        {
            clients = new TcpUser[_maxUser];
            tcp = new Tcp();
        }
    }
}
