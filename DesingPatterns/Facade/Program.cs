using System;

namespace DesignPatterns.Facade
{
    // --- Subsystems (Alt Sistemler) ---
    // Bu sınıflar karmaşık işlemleri yapan, detaylı konfigürasyon gerektiren parçalardır.
    // Facade olmasa, Client bu sınıfların hepsini tek tek bilmek ve yönetmek zorunda kalırdı.

    public class AudioEngine
    {
        public void Initialize()
        {
            Console.WriteLine("AudioEngine: Ses sürücüleri yükleniyor...");
        }

        public void LoadBanks()
        {
            Console.WriteLine("AudioEngine: Ses bankaları (SFX, Music) belleğe alındı.");
        }

        public void Stop()
        {
            Console.WriteLine("AudioEngine: Ses sistemi durduruldu.");
        }
    }

    public class VideoEngine
    {
        public void SetResolution(int width, int height)
        {
            Console.WriteLine($"VideoEngine: Çözünürlük {width}x{height} olarak ayarlandı.");
        }

        public void EnableVSync()
        {
            Console.WriteLine("VideoEngine: VSync aktif edildi.");
        }

        public void Stop()
        {
            Console.WriteLine("VideoEngine: Grafik kartı kaynakları serbest bırakıldı.");
        }
    }

    public class NetworkEngine
    {
        public void Connect(string ip)
        {
            Console.WriteLine($"NetworkEngine: {ip} sunucusuna bağlanılıyor...");
        }

        public void Disconnect()
        {
            Console.WriteLine("NetworkEngine: Bağlantı kesildi.");
        }
    }

    // --- Facade (Ön Yüz) ---
    // Karmaşık alt sistemleri sarar ve dış dünyaya basit bir API sunar.
    public class GameEngineFacade
    {
        private readonly AudioEngine _audio;
        private readonly VideoEngine _video;
        private readonly NetworkEngine _network;

        public GameEngineFacade()
        {
            _audio = new AudioEngine();
            _video = new VideoEngine();
            _network = new NetworkEngine();
        }

        // İstemci (Client) için "Tek Tuşla Başlat" kolaylığı
        public void StartGame()
        {
            Console.WriteLine("\n[FACADE] Oyun Başlatılıyor...");
            Console.WriteLine("--------------------------------");
            
            // Alt sistemlerin karmaşık başlatma sıralarını (initialization order) burada yönetiriz.
            _video.SetResolution(1920, 1080);
            _video.EnableVSync();
            
            _audio.Initialize();
            _audio.LoadBanks();
            
            _network.Connect("127.0.0.1");

            Console.WriteLine("--------------------------------");
            Console.WriteLine("[FACADE] Oyun Hazır ve Çalışıyor!\n");
        }

        // İstemci için "Tek Tuşla Durdur" kolaylığı
        public void StopGame()
        {
            Console.WriteLine("\n[FACADE] Oyun Kapatılıyor...");
            Console.WriteLine("--------------------------------");

            _network.Disconnect();
            _audio.Stop();
            _video.Stop();

            Console.WriteLine("--------------------------------");
            Console.WriteLine("[FACADE] Oyun Güvenli Şekilde Kapatıldı.\n");
        }
    }

    // --- Client (İstemci) ---
    class Program
    {
        static void Main(string[] args)
        {
            // Facade OLMADAN (Kötü Yöntem):
            // Client, AudioEngine'i, VideoEngine'i, NetworkEngine'i bilmek zorunda kalırdı.
            // Sıralamayı (önce video mu, ses mi?) kendisi yönetmek zorunda kalırdı.
            
            // Facade İLE (İyi Yöntem):
            // Client sadece Facade sınıfını bilir.
            
            GameEngineFacade gameEngine = new GameEngineFacade();

            // Sadece tek bir metod çağırarak karmaşık bir başlatma zincirini tetikliyoruz.
            gameEngine.StartGame();

            // Oyun döngüsü simülasyonu...
            Console.WriteLine("... OYUN OYNANIYOR ...");
            System.Threading.Thread.Sleep(1000); 

            // Çıkış işlemleri
            gameEngine.StopGame();

            Console.ReadKey();
        }
    }
}