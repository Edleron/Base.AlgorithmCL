using System;
using System.Collections.Generic;

namespace DesignPatterns.Visitor
{
    // --- 1. Element Interface (Ziyaret Edilebilir Öğe) ---
    // Ziyaretçiyi kabul eden (Accept) arayüz.
    // Tüm oyun birimleri bunu uygular.
    public interface IGameUnit
    {
        void Accept(IPowerUpVisitor visitor);
        // Örnek veri: Can ve İsim
        string Name { get; }
        int Health { get; set; }
    }

    // --- 2. Concrete Elements (Somut Öğeler) ---
    
    // Asker Sınıfı
    public class Soldier : IGameUnit
    {
        public string Name { get; private set; }
        public int Health { get; set; } = 100;
        public int Stamina { get; set; } = 100; // Askere özel özellik

        public Soldier(string name)
        {
            Name = name;
        }

        // Kilit Nokta: Double Dispatch
        // Kendini (this) ziyaretçiye parametre olarak verir.
        // Böylece Visitor, 'Visit(Soldier s)' metodunu çalıştıracağını anlar.
        public void Accept(IPowerUpVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // Tank Sınıfı
    public class Tank : IGameUnit
    {
        public string Name { get; private set; }
        public int Health { get; set; } = 500;
        public int Fuel { get; set; } = 100; // Tanka özel özellik

        public Tank(string name)
        {
            Name = name;
        }

        public void Accept(IPowerUpVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // --- 3. Visitor Interface (Ziyaretçi Arayüzü) ---
    // Ziyaretçinin gidebileceği tüm sınıflar için birer metot tanımlanır.
    // Metot overloading (Aşırı yükleme) kullanılır.
    public interface IPowerUpVisitor
    {
        void Visit(Soldier soldier);
        void Visit(Tank tank);
    }

    // --- 4. Concrete Visitors (Somut Ziyaretçiler) ---
    // Her bir güçlendirmenin (PowerUp) mantığı burada tutulur.
    
    // Güçlendirme 1: İlk Yardım / Tamir Kiti
    public class RepairKitVisitor : IPowerUpVisitor
    {
        public void Visit(Soldier soldier)
        {
            // Asker mantığı: Canı az artar ama Stamina da dolar.
            soldier.Health += 10;
            soldier.Stamina = 100;
            Console.WriteLine($"[RepairKit] Asker {soldier.Name} pansuman edildi. (+10 HP, Full Stamina)");
        }

        public void Visit(Tank tank)
        {
            // Tank mantığı: Sadece metal tamiri yapılır.
            tank.Health += 50;
            Console.WriteLine($"[RepairKit] Tank {tank.Name} kaynak yapıldı. (+50 HP)");
        }
    }

    // Güçlendirme 2: Süper Yakıt / Adrenalin
    public class SuperFuelVisitor : IPowerUpVisitor
    {
        public void Visit(Soldier soldier)
        {
            // Asker yakıt içemez, belki zehirlenir veya sadece hızlanır?
            Console.WriteLine($"[SuperFuel] Asker {soldier.Name} bunu kullanamaz! (Etkisiz)");
        }

        public void Visit(Tank tank)
        {
            // Tank yakıtı sever.
            tank.Fuel += 50;
            Console.WriteLine($"[SuperFuel] Tank {tank.Name} nitro doldurdu! (+50 Fuel)");
        }
    }

    // Güçlendirme 3: (Yeni özellik eklemek çok kolay) Radyasyon Kalkanı
    // Sadece yeni bir Visitor sınıfı yazarak tüm oyuna yeni bir mekanik ekleyebiliriz.
    public class ShieldVisitor : IPowerUpVisitor
    {
        public void Visit(Soldier soldier)
        {
            Console.WriteLine($"[Shield] Asker {soldier.Name} nano-kalkan giydi.");
        }

        public void Visit(Tank tank)
        {
            Console.WriteLine($"[Shield] Tank {tank.Name} reaktif zırhı aktifleştirdi.");
        }
    }

    // --- 5. Object Structure & Client ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Visitor Pattern (PowerUp Sistemi) ---\n");

            // 1. Oyun Sahnesindeki Birimler
            List<IGameUnit> units = new List<IGameUnit>
            {
                new Soldier("Er Ryan"),
                new Tank("Koca Yusuf"),
                new Soldier("Çavuş Fury")
            };

            // 2. Güçlendirmeler (Visitors)
            IPowerUpVisitor repairKit = new RepairKitVisitor();
            IPowerUpVisitor fuelCanister = new SuperFuelVisitor();
            IPowerUpVisitor shield = new ShieldVisitor();

            Console.WriteLine(">>> Senaryo 1: Gökyüzünden Tamir Kitleri yağıyor!");
            // Tüm birimler bu kiti kabul eder
            foreach (var unit in units)
            {
                unit.Accept(repairKit);
            }

            Console.WriteLine("\n" + new string('-', 40) + "\n");

            Console.WriteLine(">>> Senaryo 2: Yakıt Tankeri patladı (Herkes Yakıt alıyor)");
            foreach (var unit in units)
            {
                // Polymorphism sayesinde 'unit'in ne olduğunu bilmesek de
                // Visitor doğru metoda (Soldier mı Tank mı) yönlenecek.
                unit.Accept(fuelCanister);
            }

            Console.WriteLine("\n" + new string('-', 40) + "\n");

            Console.WriteLine(">>> Senaryo 3: Yeni özellik eklendi (Kalkan)");
            // Sınıflara (Soldier/Tank) dokunmadan yeni özellik uyguluyoruz.
            foreach (var unit in units)
            {
                unit.Accept(shield);
            }

            Console.ReadKey();
        }
    }
}