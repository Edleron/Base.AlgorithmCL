using System;
using System.Collections.Generic;

namespace DesignPatterns.Composite
{
    // --- 1. Component (Bileşen) ---
    // Hem tekil nesnelerin (Leaf) hem de grupların (Composite) uygulayacağı ortak arayüz.
    // Bu sayede dışarıdan bakan biri (Client), tekil mi yoksa grup mu olduğunu ayırt etmeden işlem yapabilir.
    public interface IMilitaryUnit
    {
        void Move(int x, int y);
        int GetFirepower(); // Saldırı gücü
        void DisplayHierarchy(int indent); // Ağaç yapısını konsola çizmek için yardımcı metot
    }

    // --- 2. Leaf (Yaprak) ---
    // Altında başka çocuk barındırmayan en temel birim (Örn: Tek bir asker).
    public class Soldier : IMilitaryUnit
    {
        public string Name { get; private set; }
        public int Damage { get; private set; }

        public Soldier(string name, int damage)
        {
            Name = name;
            Damage = damage;
        }

        public void Move(int x, int y)
        {
            Console.WriteLine($"{new string(' ', 2)}Asker {Name}: ({x},{y}) noktasına yürüyor.");
        }

        public int GetFirepower()
        {
            return Damage;
        }

        public void DisplayHierarchy(int indent)
        {
            Console.WriteLine($"{new string('-', indent)} Asker: {Name} (Güç: {Damage})");
        }
    }

    // --- 3. Composite (Bileşik) ---
    // İçinde başka bileşenleri (Leaf veya başka Composite'leri) barındıran kap.
    // Kendisine gelen emri çocuklarına iletir.
    public class Squad : IMilitaryUnit
    {
        private string _squadName;
        // Listede sadece Soldier değil, IMilitaryUnit tutuyoruz. 
        // Böylece squad içine başka squadlar da eklenebilir (İç içe yapı).
        private List<IMilitaryUnit> _units = new List<IMilitaryUnit>();

        public Squad(string name)
        {
            _squadName = name;
        }

        // Composite'e özgü çocuk yönetimi metotları
        public void AddUnit(IMilitaryUnit unit)
        {
            _units.Add(unit);
        }

        public void RemoveUnit(IMilitaryUnit unit)
        {
            _units.Remove(unit);
        }

        // --- Ortak Operasyonlar ---

        public void Move(int x, int y)
        {
            Console.WriteLine($"\n[{_squadName}] Takımı harekete geçiyor...");
            // Composite'in en önemli özelliği:
            // İşlemi kendisine bağlı tüm çocuklara delege eder (recursive olarak).
            foreach (var unit in _units)
            {
                unit.Move(x, y);
            }
        }

        public int GetFirepower()
        {
            int totalPower = 0;
            foreach (var unit in _units)
            {
                totalPower += unit.GetFirepower();
            }
            return totalPower;
        }

        public void DisplayHierarchy(int indent)
        {
            Console.WriteLine($"{new string('-', indent)} [{_squadName}] (Toplam Güç: {GetFirepower()})");
            foreach (var unit in _units)
            {
                unit.DisplayHierarchy(indent + 2);
            }
        }
    }

    // --- 4. Client (Oyun Mantığı) ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Composite Pattern (RTS Ordu Sistemi) ---\n");

            // 1. Tekil Birimler (Leaf) oluşturuluyor
            Soldier s1 = new Soldier("Rifleman A", 10);
            Soldier s2 = new Soldier("Rifleman B", 10);
            Soldier s3 = new Soldier("Sniper", 50); // Keskin nişancı daha güçlü

            // 2. Bir Takım (Composite) oluşturuluyor ve askerler ekleniyor
            Squad alphaSquad = new Squad("Alpha Timi");
            alphaSquad.AddUnit(s1);
            alphaSquad.AddUnit(s2);

            // 3. Başka bir takım
            Squad sniperTeam = new Squad("Recon Timi");
            sniperTeam.AddUnit(s3);

            // 4. İç içe yapı: Ana Ordu (Composite içinde Composite)
            Squad mainArmy = new Squad("Ana Ordu");
            mainArmy.AddUnit(alphaSquad); // Takımı ekle
            mainArmy.AddUnit(sniperTeam); // Diğer takımı ekle
            
            // Ayrıca orduya doğrudan general de ekleyebiliriz (Composite içine Leaf)
            mainArmy.AddUnit(new Soldier("General", 5)); 

            // --- TESTLER ---

            // Hiyerarşiyi Gör
            Console.WriteLine(">>> Ordu Hiyerarşisi:");
            mainArmy.DisplayHierarchy(0);

            Console.WriteLine("\n--------------------------------");

            // Tüm orduyu tek komutla hareket ettir
            // Client, içeride kaç katman olduğunu bilmek zorunda değil.
            Console.WriteLine(">>> Komut: Tüm Ordu İleri Marş!");
            mainArmy.Move(100, 200);

            Console.WriteLine("\n--------------------------------");

            // Toplam gücü hesapla
            Console.WriteLine($">>> Ordunun Toplam Saldırı Gücü: {mainArmy.GetFirepower()}");
            // Alpha Timi: 10 + 10 = 20
            // Recon Timi: 50
            // General: 5
            // Toplam: 75 olmalı.

            Console.ReadKey();
        }
    }
}