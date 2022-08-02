using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public class Tcp
    {
        public readonly int id;
        public string Name;
        public static int buffersize = 4096;
        public TcpClient socket;
        public NetworkStream stream;
        public byte[] buffer = new byte[buffersize];
        public byte[] bufferSend = new byte[buffersize];
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

            Console.WriteLine($"{id} kullanıcı ayrıldı.");
        }
        public void Connect(TcpClient socket)
        {
            this.socket = socket;
            socket.ReceiveBufferSize = buffersize;
            socket.SendBufferSize = buffersize;
            stream = socket.GetStream();
            stream.BeginRead(buffer, 0, buffersize, new AsyncCallback(ReveiveCallBack), null);
            
            Console.WriteLine($"Bağlantı gerçekleşti.{socket.Client.RemoteEndPoint}");
        }

        public void ReveiveCallBack(IAsyncResult asyncResult)
        {
            try
            {
                int gelenVeriUzunluğu = stream.EndRead(asyncResult);
                if (gelenVeriUzunluğu <= 0)
                {
                    Server.clients[id].Disconnect();
                    Console.WriteLine("Bağlantı Koptu.");
                    return;
                }

                byte[] data = new byte[gelenVeriUzunluğu];
                Array.Copy(buffer, data, gelenVeriUzunluğu);
                string gelenmetin = Encoding.UTF8.GetString(data);
                if ( !Equals(lastMessageTime.ToString(), DateTime.Now.ToString()) )
                {
                    lastMessageTime = DateTime.Now;
                    Server.SendAllSocketMessage(id,$"{id}:{gelenmetin}");
                }
                else
                {
                    warnCount += 1;
                    if (warnCount ==2)
                    {
                        SendMessage("Sunucudan atıldın.");
                        Server.clients[id].Disconnect();
                        warnCount = 0;
                        
                    }
                   else
                   {
                       SendMessage("Aynı anda birden fazla yazı gönderemezsiniz.");
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
            if (stream != null)
            {
                    stream.BeginWrite(Encoding.UTF8.GetBytes(Message), 0,
                Encoding.UTF8.GetBytes(Message).Length, SendCallBack, null);
            }
            
        }
        public void SendCallBack(IAsyncResult asyncResult)
        {
            stream.EndWrite(asyncResult);
            
        }
    }
}
