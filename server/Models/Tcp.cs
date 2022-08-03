using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server.Models
{
    public class Tcp
    {
        public int Id;
        public static int Buffersize = 4096;
        public byte[] Buffer = new byte[Buffersize];
        public TcpClient Socket;
        public NetworkStream Stream;
        public byte[] Data;
        public string InComingText;
        public bool ReadFlag;

        public void SendMessage(string Message)
        {
            try
            {
                if (Stream != null)
                {
                    Stream.BeginWrite(Encoding.UTF8.GetBytes(Message), 0,
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
            Stream.EndWrite(asyncResult);
            
        }

        public void ConnectSocket(TcpClient socket)
        {
            this.Socket = socket;
            socket.ReceiveBufferSize = Buffersize;
            socket.SendBufferSize = Buffersize;
            Stream = socket.GetStream();
        }

        public void DisconnectSocket()
        {
            Socket.Close();
        }

        public void ReadStream()
        {
            Stream.BeginRead(Buffer, 0, Buffersize, ReveiveCallBack, null);
        }

        private void ReveiveCallBack(IAsyncResult asyncResult)
        {
            try
            {
                int dataLength = Stream.EndRead(asyncResult);
                if (dataLength <= 0)
                {
                    Server.clients[Id].Disconnect();
                    Console.WriteLine("Bağlantı Koptu.");
                    return;
                }
                Data = new byte[dataLength];
                Array.Copy(Buffer, Data, dataLength);
                InComingText = Encoding.UTF8.GetString(Data);
                StartStreamRead();
                if (Stream != null)
                {
                    ReadStream();
                }
            }
            catch (Exception e)
            {
                Server.clients[Id].Disconnect();
                return;
            }

        }

        public virtual void StartStreamRead()
        {

        }


    }
}
