using System;
using System.Threading.Tasks;

namespace DesignPatterns.Singleton
{
    // Sealed: Bu sınıftan miras alınmasını engeller, böylece başka bir instance türetilemez.
    public sealed class GameManager
    {
        // Lazy<T>: Nesnenin sadece ihtiyaç duyulduğunda (ilk çağrıldığında) oluşturulmasını sağlar.
        // Ayrıca varsayılan olarak "Thread-Safe"tir. Yani aynı anda iki farklı thread erişmeye çalışsa bile
        // tek bir instance oluşacağını garanti eder.
        private static readonly Lazy<GameManager> _lazy = 
            new Lazy<GameManager>(() => new GameManager());

        // Global erişim noktası.
        public static GameManager Instance 
        { 
            get { return _lazy.Value; } 
        }

        // Örnek bir state (durum) verisi
        public int HighScore { get; private set; }

        // Constructor PRIVATE olmalı ki dışarıdan 'new GameManager()' denilemesin.
        private GameManager()
        {
            Console.WriteLine("GameManager Instance'ı oluşturuldu (Constructor çalıştı).");
            HighScore = 0;
        }

        // Örnek bir metot
        public void UpdateScore(int score)
        {
            Console.WriteLine($"Skor güncelleniyor: {score}");
            HighScore = score;
        }
    }

    // Test için Client kodu
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Singleton Pattern Demo ---");

            // 1. Erişim: İlk kez çağrıldığında Constructor çalışacak.
            GameManager gm1 = GameManager.Instance;
            gm1.UpdateScore(100);

            Console.WriteLine();

            // 2. Erişim: İkinci kez çağrıldığında Constructor ÇALIŞMAYACAK, var olan instance gelecek.
            GameManager gm2 = GameManager.Instance;
            Console.WriteLine($"GM2 üzerinden okunan HighScore: {gm2.HighScore}");

            Console.WriteLine();

            // Referans eşitliği kontrolü
            if (ReferenceEquals(gm1, gm2))
            {
                Console.WriteLine("Başarılı: gm1 ve gm2 bellekteki AYNI nesneyi işaret ediyor.");
            }
            else
            {
                Console.WriteLine("Hata: Nesneler farklı!");
            }

            Console.WriteLine("\n--- Thread Safety Test ---");
            
            // Farklı thread'lerden aynı anda erişmeye çalışsak bile...
            Parallel.Invoke(
                () => AccessSingleton("Thread-1"),
                () => AccessSingleton("Thread-2"),
                () => AccessSingleton("Thread-3")
            );

            Console.ReadKey();
        }

        static void AccessSingleton(string threadName)
        {
            GameManager gm = GameManager.Instance;
            Console.WriteLine($"{threadName} erişti. Instance ID: {gm.GetHashCode()}");
        }
    }
}