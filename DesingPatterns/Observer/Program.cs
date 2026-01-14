using System;
using System.Collections.Generic;

namespace DesignPatterns.Observer
{
    // --- 1. Observer Interface (Gözlemci) ---
    // Olayları dinleyecek olan sınıfların uygulaması gereken arayüz.
    public interface IGameObserver
    {
        // Subject'ten bir bildirim geldiğinde tetiklenecek metot.
        void OnNotify(string eventCode, object contextData);
    }

    // --- 2. Subject Interface (Yayıncı/Konu) ---
    // Olayı gerçekleştiren ve dinleyicileri yöneten arayüz.
    public interface ISubject
    {
        void Attach(IGameObserver observer); // Abone ol
        void Detach(IGameObserver observer); // Abonelikten çık
        void NotifyObservers(string eventCode, object contextData); // Herkese haber ver
    }

    // --- 3. Concrete Subject (Somut Yayıncı) ---
    // Örn: Oyuncunun istatistiklerini tutan sınıf.
    public class PlayerStats : ISubject
    {
        private List<IGameObserver> _observers = new List<IGameObserver>();

        public string PlayerName { get; private set; }
        public int Health { get; private set; }
        public int Gold { get; private set; }

        public PlayerStats(string name)
        {
            PlayerName = name;
            Health = 100;
            Gold = 0;
        }

        // --- Gözlemci Yönetimi ---
        public void Attach(IGameObserver observer)
        {
            _observers.Add(observer);
            Console.WriteLine($"[Sistem] Yeni bir gözlemci eklendi: {observer.GetType().Name}");
        }

        public void Detach(IGameObserver observer)
        {
            _observers.Remove(observer);
            Console.WriteLine($"[Sistem] Bir gözlemci ayrıldı: {observer.GetType().Name}");
        }

        public void NotifyObservers(string eventCode, object contextData)
        {
            foreach (var observer in _observers)
            {
                observer.OnNotify(eventCode, contextData);
            }
        }

        // --- Oyun Mantığı ---
        
        public void TakeDamage(int damage)
        {
            Health -= damage;
            Console.WriteLine($"\n> {PlayerName} hasar aldı! (Yeni HP: {Health})");
            
            // Tüm dinleyicilere haber ver
            NotifyObservers("PLAYER_DAMAGED", Health);

            if (Health <= 0)
            {
                NotifyObservers("PLAYER_DIED", null);
            }
        }

        public void CollectGold(int amount)
        {
            Gold += amount;
            Console.WriteLine($"\n> {PlayerName} {amount} altın buldu! (Toplam: {Gold})");
            
            // Tüm dinleyicilere haber ver
            NotifyObservers("GOLD_GAINED", Gold);
        }
    }

    // --- 4. Concrete Observers (Somut Gözlemciler) ---

    // UI Sistemi: Sadece ekrandaki barları günceller.
    public class UIManager : IGameObserver
    {
        public void OnNotify(string eventCode, object contextData)
        {
            switch (eventCode)
            {
                case "PLAYER_DAMAGED":
                    int currentHealth = (int)contextData;
                    Console.WriteLine($"[UI] Can barı güncellendi: %{currentHealth}");
                    break;
                case "GOLD_GAINED":
                    int currentGold = (int)contextData;
                    Console.WriteLine($"[UI] Altın sayacı güncellendi: {currentGold}");
                    break;
                case "PLAYER_DIED":
                    Console.WriteLine("[UI] 'GAME OVER' ekranı açılıyor...");
                    break;
            }
        }
    }

    // Başarım Sistemi: Belirli koşullar sağlandı mı diye bakar.
    public class AchievementSystem : IGameObserver
    {
        private bool _richBadgeUnlocked = false;

        public void OnNotify(string eventCode, object contextData)
        {
            if (eventCode == "GOLD_GAINED")
            {
                int gold = (int)contextData;
                if (gold >= 100 && !_richBadgeUnlocked)
                {
                    Console.WriteLine("[BAŞARIM] Tebrikler! 'Hazine Avcısı' rozeti kazanıldı!");
                    _richBadgeUnlocked = true;
                }
            }
        }
    }

    // Ses Sistemi: Olaylara göre ses çalar.
    public class AudioManager : IGameObserver
    {
        public void OnNotify(string eventCode, object contextData)
        {
            if (eventCode == "PLAYER_DAMAGED")
            {
                Console.WriteLine("[AUDIO] 'Uh! Ah!' (Hasar alma sesi çalınıyor)");
            }
            else if (eventCode == "GOLD_GAINED")
            {
                Console.WriteLine("[AUDIO] 'Ching!' (Para sesi çalınıyor)");
            }
        }
    }

    // --- 5. Client (Oyun Döngüsü) ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Observer Pattern (Event Sistemi) ---\n");

            // 1. Subject (Yayıncı) oluşturulur
            PlayerStats player = new PlayerStats("Ertuğrul");

            // 2. Observerlar (Dinleyiciler) oluşturulur
            UIManager ui = new UIManager();
            AchievementSystem achievements = new AchievementSystem();
            AudioManager audio = new AudioManager();

            // 3. Abonelikler başlatılır (Wiring)
            // Player kimin onu dinlediğini bilmez, sadece listeye ekler.
            player.Attach(ui);
            player.Attach(achievements);
            player.Attach(audio);

            Console.WriteLine("--------------------------------------");

            // 4. Oyun Olayları Tetikleniyor
            
            // Senaryo 1: Altın Toplama
            player.CollectGold(50); 
            // Beklenen: UI güncellenir, Ses çalar. Başarım açılmaz (50 < 100).

            // Senaryo 2: Daha fazla altın
            player.CollectGold(60); 
            // Beklenen: Toplam 110 oldu. UI güncellenir, Ses çalar VE Başarım açılır.

            // Senaryo 3: Hasar Alma
            player.TakeDamage(20);
            // Beklenen: UI güncellenir, Hasar sesi çalar.

            // Senaryo 4: Oyuncu Ölümü
            // Dinamik olarak UI'ı sistemden çıkaralım (Örn: UI çöktü veya kapandı)
            Console.WriteLine("\n[Sistem] UI sistemi devreden çıktı...");
            player.Detach(ui);

            player.TakeDamage(80); // Can 0'a düşer.
            // Beklenen: Ses çalar ama UI güncellemesi ("Can barı güncellendi") YAZMAZ.

            Console.ReadKey();
        }
    }
}