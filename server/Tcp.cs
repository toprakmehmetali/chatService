using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using server.Models;

namespace server
{
    public class Tcp
    {
        public int id;
        public string Name;
        public static int buffersize = 4096;
        public TcpClient socket;
        public NetworkStream stream;
        public byte[] buffer = new byte[buffersize];
        public DateTime lastMessageTime;
        public int warnCount = 0;

        public Tcp(int id)
        {
            this.id = id;
        }

        public void Disconnect()
        {
            socket.Close();
            stream = null;
            socket = null;
            Server.SendMessageAllSocket(id,$"{id}. {Messages.Messages.UserOffline}");
            Console.WriteLine($"{id}. {Messages.Messages.UserOffline}.");
        }
        public void Connect(TcpClient socket)
        {
            this.socket = socket;
            socket.ReceiveBufferSize = buffersize;
            socket.SendBufferSize = buffersize;

            stream = socket.GetStream();
            stream.BeginRead(buffer, 0, buffersize, new AsyncCallback(ReveiveCallBack), null);
            Server.SendMessageAllSocket(id, $"{id} {Messages.Messages.UserOnline}");
            Console.WriteLine($"{Messages.Messages.UserConnected} {socket.Client.RemoteEndPoint}");
        }

        public void ReveiveCallBack(IAsyncResult asyncResult)
        {
            try
            {
                int dataLength = stream.EndRead(asyncResult);
                if (dataLength <= 0)
                {
                    Server.clients[id].Disconnect();
                    Console.WriteLine("Bağlantı Koptu.");
                    return;
                }

                byte[] data = new byte[dataLength];
                Array.Copy(buffer, data, dataLength);
                string gelenmetin = Encoding.UTF8.GetString(data);
                DataTransferObject dataTransferObject = JsonConvert.DeserializeObject<DataTransferObject>(gelenmetin);
                if ( !Equals(lastMessageTime.ToString(), DateTime.Now.ToString()) )
                {
                    lastMessageTime = DateTime.Now;
                    RequestType(dataTransferObject);
                }
                else
                {
                    warnCount += 1;
                    if (warnCount ==2)
                    {
                        SendMessage(Messages.Messages.ServerKicked);
                        Server.clients[id].Disconnect();
                        warnCount = 0;
                        
                    }
                   else
                   {
                       SendMessage(Messages.Messages.NotSendQuickMessage);
                   }
                }

                if (stream != null)
                {
                    stream.BeginRead(buffer, 0, buffersize, new AsyncCallback(ReveiveCallBack), null);

                }


            }
            catch (Exception e)
            {
                Server.clients[id].Disconnect();
                return;
            }
        }

        public void SendMessage(string Message)
        {
            try
            {
                if (stream != null)
                {
                    stream.BeginWrite(Encoding.UTF8.GetBytes(Message), 0,
                Encoding.UTF8.GetBytes(Message).Length, SendCallBack, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            
        }
        public void SendCallBack(IAsyncResult asyncResult)
        {
            stream.EndWrite(asyncResult);
            
        }

        public void RequestType(DataTransferObject dataTransferObject)
        {
            switch (dataTransferObject.RequestType)
            {
                case "message":
                    Server.SendMessageAllSocket(id,Name+": "+dataTransferObject.Request);
                    break;
                case "rename":
                    Name = dataTransferObject.Request;
                    break;
            }
        }
    }
}
