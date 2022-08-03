using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using client.Models;
using Newtonsoft.Json;
using server;

namespace client
{
    public class Client
    {
        public static TcpClient socket = new TcpClient();
        public static NetworkStream networkStream;
        public static byte[] buffer = new byte[4096];
        public static void Connect()
        {
            try
            {
                socket.BeginConnect(Config.Config.ConfigJson.ServerSettings.Host, Config.Config.ConfigJson.ServerSettings.Port, new AsyncCallback(ConnectCallBack), null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        private static void ConnectCallBack(IAsyncResult asyncResult)
        {
            socket.EndConnect(asyncResult);
            networkStream = socket.GetStream();
            Read();
            
        }

        public static void Read()
        {
            networkStream.BeginRead(buffer, 0, 4096, ReveiveCallBack, null);
        }

        public static void ReveiveCallBack(IAsyncResult asyncResult)
        {
            try
            {
                
                    int gelenVeriUzunluğu = networkStream.EndRead(asyncResult);
               
                    byte[] data = new byte[gelenVeriUzunluğu];
                    Array.Copy(buffer, data, gelenVeriUzunluğu);
                    string gelenmetin = Encoding.UTF8.GetString(data);

                if (gelenmetin != "")
                {
                    Console.WriteLine($"{gelenmetin}");
                    networkStream.BeginRead(buffer, 0, 4096, ReveiveCallBack, null);

                }
                else
                {
                    networkStream = null;
                    Console.WriteLine("Bağlantı Kesildi");
                }


            }
            catch (Exception e)
            {
                
                return;
            }
        }
        public static void SendMessage(DataTransferObject Dto)
        {
            string JsonString = JsonConvert.SerializeObject(Dto);
            var result = Encoding.UTF8.GetBytes(JsonString);

            try
            {
                if (networkStream != null)
                {
                   networkStream.BeginWrite(result, 0, result.Length,SendCallBack, null);
            
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        public static void SendCallBack(IAsyncResult asyncResult)
        {
            
            networkStream.EndWrite(asyncResult);
        }

        public static void LoginName()
        {
            if (networkStream != null)
            {
                Requests.Request("loginName");
            }
            
        }
    }
}
