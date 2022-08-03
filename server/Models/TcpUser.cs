using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using server.Models;

namespace server.Models
{
    public class TcpUser : Tcp
    {
       
        public string Name;
        public DateTime lastMessageTime;
        public int warnCount = 0;

        public TcpUser(int id)
        {
            this.Id = id;
        }

        public void Disconnect()
        {

            Server.SendMessageAllSocket(Id, $"{Name} adlı {Messages.Messages.UserOffline}");
            Console.WriteLine($"{Socket.Client.RemoteEndPoint} {Messages.Messages.UserOffline}.");
            DisconnectSocket();
            this.Stream = null;
            this.Socket = null;
            Name = $"{Id}.Anonymous";

        }

        public void Connect(TcpClient socket)
        {
            ConnectSocket(socket);
            Console.WriteLine($"{socket.Client.RemoteEndPoint} {Messages.Messages.UserConnected} ");
            ReadStream();
        }

        public override void StartStreamRead()
        {

            DataTransferObject dataTransferObject = JsonConvert.DeserializeObject<DataTransferObject>(InComingText);
            if (!Equals(lastMessageTime.ToString(), DateTime.Now.ToString()))
            {
                lastMessageTime = DateTime.Now;
                RequestType(dataTransferObject);
            }
            else
            {
                warnCount += 1;
                if (warnCount == 2)
                {
                    SendMessage(Messages.Messages.ServerKicked);
                    Disconnect();
                    warnCount = 0;

                }
                else
                {
                    SendMessage(Messages.Messages.NotSendQuickMessage);
                }
            }

            
            
        }

        public void LoginNameRequest(DataTransferObject dataTransferObject)
        {
            if (dataTransferObject.Request == "")
            {
                dataTransferObject.Request = "Anonymous";
            }
            Name = dataTransferObject.Request;
            Server.SendMessageAllSocket(Id, $"{Name} {Messages.Messages.UserOnline}");
            SendMessage(Messages.Messages.Login);
        }

        public void RenameRequest(DataTransferObject dataTransferObject)
        {
            Server.SendMessageAllSocket(Id, $"{Name} isimli kullanıcı adını {dataTransferObject.Request} olarak değiştirdi.");
            Name = dataTransferObject.Request;
            SendMessage(Messages.Messages.NameChanged);
        }

        public void RequestType(DataTransferObject dataTransferObject)
        {
            switch (dataTransferObject.RequestType)
            {
                case "message":
                    Server.SendMessageAllSocket(Id, Name + " : " + dataTransferObject.Request);
                    break;
                case "loginName":
                    LoginNameRequest(dataTransferObject);
                    break;
                case "rename":
                    RenameRequest(dataTransferObject);
                    break;
            }
        }
    }
}
