using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Messages
{
    public static class Messages
    {
        public static string ServerFull = "Server Dolu.";
        public static string ServerStart = "Server Başlatıldı.";
        public static string WaitingForUsersToConnect = "Kullanıcıların bağlanması bekleniyor.";
        public static string UserConnected = "Kullanıcı bağlandı.";
        public static string UserDisconnected = "Kullanıcının bağlantısı koptu.";
        public static string ServerKicked = "Sunucudan atıldın.";
        public static string NotSendQuickMessage = "Aynı anda birden fazla mesaj gönderemezsiniz.";
        public static string UserOffline = "Kullanıcı çevrimdışı.";
        public static string UserOnline = "Kullanıcı çevrimiçi.";
        public static string NameChanged = "Kullanıcı adınız değiştirildi.";
        public static string Login = "Başarıyla sunucuya giriş yaptınız. Sohbete başlayabilirsiniz.";
    }
}
