using System;

namespace DesignPatterns.NullObject
{
    // --- 1. Abstract / Interface (Arayüz) ---
    // İstemcinin kullanacağı ortak kontrat.
    public interface IAnalyticsService
    {
        void TrackEvent(string eventName);
        void TrackError(string errorMessage);
    }

    // --- 2. Real Object (Gerçek Nesne) ---
    // İşlevsel olan, gerçek işi yapan sınıf.
    public class CloudAnalyticsService : IAnalyticsService
    {
        private string _apiKey;

        public CloudAnalyticsService(string apiKey)
        {
            _apiKey = apiKey;
            Console.WriteLine($"[Cloud] Servis {_apiKey} anahtarı ile başlatıldı.");
        }

        public void TrackEvent(string eventName)
        {
            // Gerçek sunucuya istek atılan yer burasıdır.
            Console.WriteLine($"[Cloud] Etkinlik Gönderildi -> {eventName}");
        }

        public void TrackError(string errorMessage)
        {
            Console.WriteLine($"[Cloud] Hata Raporlandı -> {errorMessage}");
        }
    }

    // --- 3. Null Object (Boş Nesne) ---
    // Arayüzü uygular ama metodların içi BOŞTUR.
    // Hiçbir şey yapmaz, hata da vermez. Sadece "var gibi" davranır.
    public class NullAnalyticsService : IAnalyticsService
    {
        // Singleton olarak tutulabilir çünkü durumu (state) yoktur.
        // Her seferinde new NullAnalyticsService() demek yerine bellek tasarrufu sağlar.
        public static readonly NullAnalyticsService Instance = new NullAnalyticsService();

        private NullAnalyticsService() { }

        public void TrackEvent(string eventName)
        {
            // Hiçbir şey yapma. (Do nothing)
        }

        public void TrackError(string errorMessage)
        {
            // Hiçbir şey yapma.
        }
    }

    // --- 4. Client (İstemci) ---
    // Analitik servisini kullanan oyuncu sınıfı.
    // DİKKAT: Bu sınıf servisin "Gerçek" mi yoksa "Boş" mu olduğunu bilmez ve KONTROL ETMEZ.
    public class Player
    {
        private readonly IAnalyticsService _analytics;

        public string Name { get; set; }

        public Player(string name, IAnalyticsService analytics)
        {
            Name = name;
            // Eğer null gelirse varsayılan olarak Null Object atayarak garantileme yapabiliriz.
            _analytics = analytics ?? NullAnalyticsService.Instance;
        }

        public void LevelUp()
        {
            Console.WriteLine($"{Name} seviye atladı!");
            
            // BURADA 'if (_analytics != null)' KONTROLÜNE GEREK YOK!
            // Kod temiz, okunabilir ve güvenli.
            _analytics.TrackEvent("Level_Up_Event");
        }

        public void Die()
        {
            Console.WriteLine($"{Name} öldü.");
            _analytics.TrackEvent("Player_Death_Event");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Null Object Pattern (Analytics Senaryosu) ---\n");

            // SENARYO 1: Canlı Ortam (Production)
            // Gerçek bir servis kullanıyoruz. Veriler loglanacak.
            Console.WriteLine(">>> Oyun Modu: ONLINE (Gerçek Servis)");
            IAnalyticsService realService = new CloudAnalyticsService("API-XYZ-999");
            Player p1 = new Player("Ertuğrul", realService);
            
            p1.LevelUp(); // Log düşecek.

            Console.WriteLine("\n" + new string('-', 40) + "\n");

            // SENARYO 2: Geliştirme Ortamı (Development)
            // Verilerin bir yere gitmesini istemiyoruz.
            // Buraya 'null' versek bile Player sınıfı (Constructor'daki koruma sayesinde) NullObject kullanacak.
            // Veya doğrudan NullAnalyticsService.Instance verebiliriz.
            Console.WriteLine(">>> Oyun Modu: OFFLINE / TEST (Null Servis)");
            
            // Player'ın constructor'ında "null gelirse NullObject kullan" dediğimiz için null geçiyoruz.
            // Bu sayede Player kodunda hiçbir değişiklik yapmadan davranışı kapattık.
            Player p2 = new Player("TestKullanıcısı", null);
            
            p2.LevelUp(); // Ekrana hiçbir "Log gönderildi" yazısı çıkmayacak. Hata da vermeyecek.
            p2.Die();

            Console.ReadKey();
        }
    }
}