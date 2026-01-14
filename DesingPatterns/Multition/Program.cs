using System;
using System.Collections.Concurrent; // Thread-Safe Dictionary için
using System.Collections.Generic;

namespace DesignPatterns.Multiton
{
    // --- 1. Multiton Class ---
    // Her anahtar (Key) için tek bir instance tutan sınıf.
    public class ServerConnection
    {
        // Örnekleri sakladığımız statik kasa (Registry).
        // Thread-Safe olması için ConcurrentDictionary kullanıyoruz.
        // Anahtar: Bölge Kodu (string), Değer: Bağlantı Nesnesi (ServerConnection)
        private static readonly ConcurrentDictionary<string, ServerConnection> _instances 
            = new ConcurrentDictionary<string, ServerConnection>();

        // Her instance'ın kendi verisi
        public string RegionCode { get; private set; }
        public string IPAddress { get; private set; }
        
        // Simüle edilmiş bağlantı durumu
        private bool _isConnected;

        // Constructor PRIVATE olmalı (Singleton kuralı).
        // Sadece bu sınıf kendi içinde instance üretebilir.
        private ServerConnection(string region)
        {
            RegionCode = region;
            // Rastgele bir IP atayalım
            IPAddress = $"192.168.{new Random().Next(1, 255)}.1";
            _isConnected = false;
            
            Console.WriteLine($"[Sistem] YENİ bağlantı nesnesi oluşturuldu: {RegionCode} ({IPAddress})");
        }

        // --- Global Erişim Noktası (Multiton'ın Kalbi) ---
        // İstemci bir anahtar (key) verir, biz ona o anahtara ait TEKİL nesneyi döneriz.
        public static ServerConnection GetInstance(string regionKey)
        {
            // GetOrAdd: Anahtar varsa getir, yoksa yeni oluştur ve ekle. (Atomic işlem)
            // Lazy loading mantığı burada işler.
            return _instances.GetOrAdd(regionKey, (key) => new ServerConnection(key));
        }

        // --- İş Mantığı Metotları ---
        
        public void Connect()
        {
            if (!_isConnected)
            {
                Console.WriteLine($"-> [{RegionCode}] Sunucusuna bağlanılıyor...");
                _isConnected = true;
            }
            else
            {
                Console.WriteLine($"-> [{RegionCode}] Zaten bağlısınız.");
            }
        }

        public void SendData(string data)
        {
            Console.WriteLine($"   >> [{RegionCode}] Veri gönderildi: '{data}'");
        }
    }

    // --- 2. Client (Oyun Mantığı) ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Multiton Pattern (Sunucu Yöneticisi) ---\n");

            // Senaryo 1: Türkiye sunucusuna ilk erişim
            // Bu noktada "TR" anahtarı yok, yeni nesne oluşacak.
            Console.WriteLine("1. İstek: TR Sunucusu");
            ServerConnection tr1 = ServerConnection.GetInstance("TR");
            tr1.Connect();
            tr1.SendData("Login İsteği");

            Console.WriteLine();

            // Senaryo 2: Avrupa sunucusuna ilk erişim
            // "EU" anahtarı yok, yeni nesne oluşacak.
            Console.WriteLine("2. İstek: EU Sunucusu");
            ServerConnection eu1 = ServerConnection.GetInstance("EU");
            eu1.Connect();

            Console.WriteLine();

            // Senaryo 3: Türkiye sunucusuna TEKRAR erişim
            // "TR" anahtarı ZATEN VAR. Yeni nesne oluşmayacak, tr1 ile aynı nesne gelecek.
            Console.WriteLine("3. İstek: TR Sunucusu (Tekrar)");
            ServerConnection tr2 = ServerConnection.GetInstance("TR");
            tr2.Connect(); // "Zaten bağlısınız" demeli.
            tr2.SendData("Ping");

            Console.WriteLine("\n" + new string('-', 40) + "\n");

            // KANIT: Referans Eşitliği Kontrolü
            if (ReferenceEquals(tr1, tr2))
            {
                Console.WriteLine("BAŞARILI: tr1 ve tr2 bellekte AYNI nesnedir.");
            }
            else
            {
                Console.WriteLine("HATA: Nesneler farklı!");
            }

            if (!ReferenceEquals(tr1, eu1))
            {
                Console.WriteLine("BAŞARILI: tr1 ve eu1 FARKLI nesnelerdir.");
            }

            Console.ReadKey();
        }
    }
}