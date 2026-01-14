using System;
using System.Collections.Generic;

namespace DesignPatterns.Memento
{
    // --- 1. Memento (Hatıra) ---
    // Nesnenin o anki durumunun donmuş bir kopyasıdır.
    // Püf Nokta: Memento immutable (değiştirilemez) olmalıdır. Veri bir kez yazılır, değişmez.
    public class GameStateMemento
    {
        public int Health { get; }
        public int Level { get; }
        public string Weapon { get; }
        public DateTime SaveTime { get; }

        public GameStateMemento(int health, int level, string weapon)
        {
            Health = health;
            Level = level;
            Weapon = weapon;
            SaveTime = DateTime.Now;
        }
    }

    // --- 2. Originator (Yaratıcı/Asıl Nesne) ---
    // Durumu kaydedilecek olan ana oyun nesnesi (Örn: Oyuncu Karakteri).
    // Memento yaratır (Save) ve Memento'dan kendini geri yükler (Load).
    public class Player
    {
        // Oyuncunun iç durumu (Private olabilir, Memento sayesinde dışarı sızdırmadan kaydederiz)
        private int _health;
        private int _level;
        private string _weapon;

        public Player()
        {
            _health = 100;
            _level = 1;
            _weapon = "Tahta Kılıç";
        }

        // Oyun içi eylemler
        public void TakeDamage(int damage)
        {
            _health -= damage;
            Console.WriteLine($"[Oyun] Oyuncu hasar aldı: -{damage} HP. (Güncel: {_health})");
        }

        public void LevelUp()
        {
            _level++;
            _health = 100; // Can yenilenir
            _weapon = "Çelik Kılıç"; // Silah gelişir
            Console.WriteLine($"[Oyun] TEBRİKLER! Seviye atlandı. (Lvl: {_level}, Silah: {_weapon})");
        }

        public void ShowStatus()
        {
            Console.WriteLine($"   -> DURUM: Can: {_health} | Lvl: {_level} | Silah: {_weapon}");
        }

        // --- Memento Metotları ---

        // SAVE: Şu anki durumu bir kutuya (Memento) koyup verir.
        public GameStateMemento SaveState()
        {
            Console.WriteLine($"[Sistem] Oyun kaydediliyor... (Zaman: {DateTime.Now.ToShortTimeString()})");
            return new GameStateMemento(_health, _level, _weapon);
        }

        // LOAD: Verilen kutudaki (Memento) bilgileri kendine uygular.
        public void RestoreState(GameStateMemento memento)
        {
            if (memento == null) return;

            _health = memento.Health;
            _level = memento.Level;
            _weapon = memento.Weapon;

            Console.WriteLine($"[Sistem] Oyun yüklendi! ({memento.SaveTime.ToShortTimeString()} zamanına dönüldü)");
            ShowStatus();
        }
    }

    // --- 3. Caretaker (Bakıcı) ---
    // Memento'ları saklayan ve yöneten sınıftır (Örn: SaveManager veya GameHistory).
    // Memento'nun içeriğini BİLMEZ ve DEĞİŞTİRMEZ. Sadece saklar ve istendiğinde geri verir.
    public class SaveManager
    {
        // Geçmiş durumları tutmak için bir yığın (Stack) kullanıyoruz. (Undo mantığı için)
        private Stack<GameStateMemento> _history = new Stack<GameStateMemento>();

        public void SaveGame(Player player)
        {
            GameStateMemento snapshot = player.SaveState();
            _history.Push(snapshot);
        }

        public void Undo(Player player)
        {
            if (_history.Count > 0)
            {
                Console.WriteLine("[Manager] 'Geri Al' komutu verildi...");
                GameStateMemento previousState = _history.Pop();
                player.RestoreState(previousState);
            }
            else
            {
                Console.WriteLine("[Manager] Geri alınacak kayıt yok!");
            }
        }
    }

    // --- 4. Client (Oyun Döngüsü) ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Memento Pattern (Save/Load & Undo Sistemi) ---\n");

            Player hero = new Player();
            SaveManager gameManager = new SaveManager();

            // 1. Oyun Başlangıcı
            Console.WriteLine(">>> Oyun Başladı");
            hero.ShowStatus();

            // KAYIT 1: Başlangıç noktasını kaydedelim.
            gameManager.SaveGame(hero);

            Console.WriteLine("\n--- Macera İlerliyor ---");
            hero.TakeDamage(20); // Can: 80
            hero.TakeDamage(30); // Can: 50
            
            // KAYIT 2: Zorlu bir savaştan önce kayıt alalım.
            gameManager.SaveGame(hero);

            Console.WriteLine("\n--- Boss Savaşı ---");
            hero.LevelUp();      // Can: 100, Lvl: 2, Silah: Çelik
            hero.TakeDamage(90); // Can: 10
            hero.ShowStatus();

            Console.WriteLine("\n>>> EYVAH! Oyuncu ölmek üzere. Son hatayı geri alalım (Undo).");
            // Boss savaşı öncesine (Kayıt 2) dönmek istiyoruz.
            gameManager.Undo(hero); 
            // Beklenen: Can 50, Lvl 1, Tahta Kılıç

            Console.WriteLine("\n>>> Bir daha geri alalım (En başa dönelim).");
            // Başlangıç noktasına (Kayıt 1) dönmek istiyoruz.
            gameManager.Undo(hero);
            // Beklenen: Can 100, Lvl 1, Tahta Kılıç

            Console.ReadKey();
        }
    }
}