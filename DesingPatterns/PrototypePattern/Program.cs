using System;
using System.Collections.Generic;

namespace DesignPatterns.Prototype
{
    // 1. Prototype Interface
    // C#'ın kendi ICloneable arayüzü vardır ancak tip güvenliği (Type Safety) sağlamaz (object döner).
    // Kendi arayüzümüzü yazmak genellikle daha temizdir.
    public interface IMonsterPrototype
    {
        IMonsterPrototype Clone();
        void ShowStats();
    }

    // 2. Concrete Prototype (Somut Prototip)
    public class Zombie : IMonsterPrototype
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Speed { get; set; }

        // Referans tipi örneği: Zombinin taşıdığı eşyalar
        public List<string> Inventory { get; set; }

        public Zombie(string name, int health, int speed)
        {
            Name = name;
            Health = health;
            Speed = speed;
            Inventory = new List<string>();

            // Simülasyon: Bu nesnenin yaratılması maliyetli bir iş olsun.
            Console.WriteLine($"[YÜKLEME] {Name} için texture ve ses dosyaları yükleniyor... (Ağır İşlem)");
        }

        // Kopyalama Mantığı
        public IMonsterPrototype Clone()
        {
            // Adım 1: Shallow Copy (Sığ Kopya)
            // MemberwiseClone, C#'ın yerleşik metodudur.
            // Value Type'ları (int, float) kopyalar. Reference Type'ların (List, Class) sadece adresini kopyalar.
            var clone = (Zombie)this.MemberwiseClone();

            // Adım 2: Deep Copy (Derin Kopya) İşlemi
            // Eğer bunu yapmazsak, klonlanan zombinin envanterine eklenen eşya,
            // orijinal zombide de gözükürdü (çünkü aynı Listeyi işaret ederlerdi).
            clone.Inventory = new List<string>(this.Inventory);

            // Klon olduğunu belli etmek için isme ek yapalım (Opsiyonel)
            clone.Name += " (Klon)";
            
            return clone;
        }

        public void ShowStats()
        {
            string items = Inventory.Count > 0 ? string.Join(", ", Inventory) : "Boş";
            Console.WriteLine($"Canavar: {Name} | HP: {Health} | Hız: {Speed} | Envanter: [{items}]");
        }
    }

    // 3. Spawner / Manager
    // Prototip nesneyi saklar ve istek geldikçe onu kopyalar.
    public class MonsterSpawner
    {
        private IMonsterPrototype _prototype;

        public MonsterSpawner(IMonsterPrototype prototype)
        {
            _prototype = prototype;
        }

        public IMonsterPrototype SpawnMonster()
        {
            return _prototype.Clone();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Prototype Pattern (Unity Instantiate Mantığı) ---\n");

            // 1. Orijinal Prototip Yaratılıyor (Ağır işlem burada sadece 1 kere çalışır)
            Console.WriteLine(">>> Oyun Başlatılıyor, Prototip Hazırlanıyor...");
            Zombie originalZombie = new Zombie("Baş Zombi", 100, 5);
            originalZombie.Inventory.Add("Çürük Et");
            
            Console.WriteLine("\n>>> Orijinal Durum:");
            originalZombie.ShowStats();

            // Spawner'a prototipi veriyoruz.
            MonsterSpawner spawner = new MonsterSpawner(originalZombie);

            Console.WriteLine("\n------------------------------------------------\n");

            // 2. Klonlama İşlemi (Constructor çalışmaz, doğrudan bellek kopyalanır - Hızlıdır)
            Console.WriteLine(">>> Zombi Dalgası Geliyor (Klonlama Başladı)...");
            
            var zombie1 = (Zombie)spawner.SpawnMonster();
            // Klonu özelleştirelim
            zombie1.Health = 50; 
            zombie1.Inventory.Add("Beyin"); // Deep Copy sayesinde orijinali etkilemeyecek.

            var zombie2 = (Zombie)spawner.SpawnMonster();
            zombie2.Speed = 10;

            // 3. Sonuçları Görelim
            Console.WriteLine("\n>>> Son Durumlar:");
            
            Console.Write("Orijinal: "); 
            originalZombie.ShowStats(); // Envanterde sadece "Çürük Et" olmalı.

            Console.Write("Klon 1:   ");
            zombie1.ShowStats();        // Envanterde "Çürük Et, Beyin" olmalı. HP: 50 olmalı.

            Console.Write("Klon 2:   ");
            zombie2.ShowStats();        // Envanterde "Çürük Et" olmalı. Hız: 10 olmalı.

            Console.ReadKey();
        }
    }
}