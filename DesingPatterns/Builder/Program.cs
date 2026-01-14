using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Builder
{
    // 1. Product (Karmaşık Ürün)
    // Birçok farklı parçadan oluşan karmaşık nesnemiz.
    public class Hero
    {
        public string HeroType { get; set; }
        public string Weapon { get; set; }
        public string Armor { get; set; }
        public List<string> Skills { get; private set; }

        public Hero()
        {
            Skills = new List<string>();
        }

        public void AddSkill(string skill)
        {
            Skills.Add(skill);
        }

        public void ShowInfo()
        {
            Console.WriteLine($"\n--- {HeroType} Karakter Özeti ---");
            Console.WriteLine($"Silah: {Weapon}");
            Console.WriteLine($"Zırh : {Armor}");
            Console.WriteLine($"Yetenekler: {string.Join(", ", Skills)}");
            Console.WriteLine("---------------------------------");
        }
    }

    // 2. Builder Interface (Soyut Kurucu)
    // Ürünün parçalarını oluşturmak için gereken adımları tanımlar.
    public interface IHeroBuilder
    {
        void Reset(); // Yeni bir obje inşasına başla
        void SetHeroType();
        void BuildWeapon();
        void BuildArmor();
        void BuildSkills();
        Hero GetHero(); // İnşa edilen ürünü teslim et
    }

    // 3. Concrete Builder A (Savaşçı Oluşturucu)
    public class WarriorBuilder : IHeroBuilder
    {
        private Hero _hero;

        public WarriorBuilder()
        {
            Reset();
        }

        public void Reset()
        {
            _hero = new Hero();
        }

        public void SetHeroType()
        {
            _hero.HeroType = "Savaşçı (Warrior)";
        }

        public void BuildWeapon()
        {
            _hero.Weapon = "Çift Elli Büyük Balta";
        }

        public void BuildArmor()
        {
            _hero.Armor = "Ağır Plaka Zırh";
        }

        public void BuildSkills()
        {
            _hero.AddSkill("Öfke Patlaması");
            _hero.AddSkill("Kasırga Dönüşü");
        }

        public Hero GetHero()
        {
            Hero result = _hero;
            Reset(); // Bir sonraki kullanım için builder'ı temizle
            return result;
        }
    }

    // 4. Concrete Builder B (Büyücü Oluşturucu)
    public class MageBuilder : IHeroBuilder
    {
        private Hero _hero;

        public MageBuilder()
        {
            Reset();
        }

        public void Reset()
        {
            _hero = new Hero();
        }

        public void SetHeroType()
        {
            _hero.HeroType = "Büyücü (Mage)";
        }

        public void BuildWeapon()
        {
            _hero.Weapon = "Kadim Asa";
        }

        public void BuildArmor()
        {
            _hero.Armor = "İpek Cübbe";
        }

        public void BuildSkills()
        {
            _hero.AddSkill("Ateş Topu");
            _hero.AddSkill("Buz Duvarı");
            _hero.AddSkill("Teleport");
        }

        public Hero GetHero()
        {
            Hero result = _hero;
            Reset();
            return result;
        }
    }

    // 5. Director (Yönetici)
    // İnşa adımlarını belirli bir sırada çalıştıran sınıftır.
    // Client (Main) doğrudan builder ile de çalışabilir ama Director süreci standartlaştırır.
    public class CharacterCreator
    {
        private IHeroBuilder _builder;

        // Director hangi builder ile çalışacağını bilir
        public CharacterCreator(IHeroBuilder builder)
        {
            _builder = builder;
        }

        // Çalışma zamanında builder değiştirilebilir (Örn: Kullanıcı ekranda Warrior'dan Mage'e geçti)
        public void ChangeBuilder(IHeroBuilder builder)
        {
            _builder = builder;
        }

        // Standart bir karakter yaratma reçetesi
        public void CreateFullCharacter()
        {
            Console.WriteLine("Director: Karakter inşası başlatılıyor...");
            _builder.SetHeroType();
            _builder.BuildWeapon();
            _builder.BuildArmor();
            _builder.BuildSkills();
            Console.WriteLine("Director: Karakter inşası tamamlandı.");
        }

        // Daha minimalist bir karakter yaratma reçetesi
        public void CreateBasicCharacter()
        {
            Console.WriteLine("Director: Temel karakter inşası başlatılıyor...");
            _builder.SetHeroType();
            _builder.BuildWeapon();
            // Zırh ve yetenek yok (belki bir NPC veya tutorial karakteridir)
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Builder Pattern Demo ---\n");

            // Builder'ları oluştur
            WarriorBuilder warriorBuilder = new WarriorBuilder();
            MageBuilder mageBuilder = new MageBuilder();

            // Director'ı oluştur ve Warrior Builder ile başlat
            CharacterCreator director = new CharacterCreator(warriorBuilder);

            // Senaryo 1: Tam Donanımlı Savaşçı
            director.CreateFullCharacter();
            Hero warrior = warriorBuilder.GetHero();
            warrior.ShowInfo();

            Console.WriteLine();

            // Senaryo 2: Büyücüye geçiş yap ama sadece temel özelliklerini yarat
            director.ChangeBuilder(mageBuilder);
            director.CreateBasicCharacter();
            Hero basicMage = mageBuilder.GetHero();
            basicMage.ShowInfo();

            // Senaryo 3: Director kullanmadan manuel inşa (Custom Build)
            // Bazen oyunculara özel bir build yapma şansı veririz, Director buna karışmaz.
            Console.WriteLine("--- Custom (Manuel) İnşa ---");
            mageBuilder.Reset();
            mageBuilder.SetHeroType();
            mageBuilder.BuildSkills(); // Silah ve Zırh almayan çıplak bir büyücü
            Hero customMage = mageBuilder.GetHero();
            customMage.ShowInfo();

            Console.ReadKey();
        }
    }
}