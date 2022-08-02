using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.Models;

namespace client
{
    public class Requests
    {
        static DataTransferObject dataTransferObject = new DataTransferObject();

        public static void Request(string text)
        {
            switch (text)
            {
                case "rename":
                    dataTransferObject.RequestType = "rename";
                    Console.WriteLine("Yeni adınızı giriniz.");
                    dataTransferObject.Request = Console.ReadLine();
                    Client.SendMessage(dataTransferObject);
                    break;
                default:
                    dataTransferObject.RequestType = "message";
                    dataTransferObject.Request = text;
                    Client.SendMessage(dataTransferObject);
                    break;
            }
        }
    }
}
