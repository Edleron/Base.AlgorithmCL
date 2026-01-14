using System;
using System.Collections.Generic;

namespace DesignPatterns.Mediator
{
    // --- 1. Mediator Interface (Arabulucu Arayüzü) ---
    // Tüm bileşenlerin (Component) iletişim kurmak için kullanacağı ortak protokol.
    public interface IGameMediator
    {
        void Notify(object sender, string eventCode);
    }

    // --- 2. Base Component (Temel Bileşen) ---
    // Sistemin parçası olan sınıfların atası. Hepsi Mediator'ı tanır.
    public abstract class BaseComponent
    {
        protected IGameMediator _mediator;

        public BaseComponent(IGameMediator mediator = null)
        {
            _mediator = mediator;
        }

        public void SetMediator(IGameMediator mediator)
        {
            _mediator = mediator;
        }
    }

    // --- 3. Concrete Components (Somut Bileşenler) ---

    // Oyuncu Cüzdanı: Parayı yönetir.
    public class WalletSystem : BaseComponent
    {
        public int Gold { get; private set; } = 500;

        public bool TrySpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                Console.WriteLine($"[Cüzdan] {amount} altın harcandı. Kalan: {Gold}");
                return true;
            }
            else
            {
                Console.WriteLine("[Cüzdan] Yetersiz bakiye!");
                // Mediator'a haber veriyoruz: Para yetmedi!
                _mediator.Notify(this, "INSUFFICIENT_FUNDS");
                return false;
            }
        }
    }

    // Envanter: Eşyaları saklar.
    public class InventorySystem : BaseComponent
    {
        private List<string> _items = new List<string>();

        public void AddItem(string itemName)
        {
            _items.Add(itemName);
            Console.WriteLine($"[Envanter] '{itemName}' çantaya eklendi.");
            // Mediator'a haber ver: Eşya alındı.
            _mediator.Notify(this, "ITEM_ADDED");
        }
    }

    // Ses Sistemi: Efektleri çalar.
    public class SoundSystem : BaseComponent
    {
        public void PlaySound(string soundName)
        {
            Console.WriteLine($"[Audio] Ses Çalınıyor: ♫ {soundName} ♫");
        }
    }

    // UI Sistemi: Ekrana yazı yazar veya butonları yönetir.
    public class UISystem : BaseComponent
    {
        public void ShowMessage(string message)
        {
            Console.WriteLine($"[UI] Ekrana Yazıldı: >> {message} <<");
        }

        // Kullanıcı arayüzünden tetiklenen bir olay (Örn: Butona tıklandı)
        public void BuyButtonClicked(string itemName, int price)
        {
            Console.WriteLine($"\n[UI] Kullanıcı '{itemName}' almak için butona bastı.");
            // UI sistemi, parayı veya envanteri BİLMEZ. Sadece isteği iletir.
            // Bu event kodunu Mediator yakalayacak ve gerekli yerlere dağıtacak.
            _mediator.Notify(this, $"BUY_REQUEST:{itemName}:{price}");
        }
    }

    // --- 4. Concrete Mediator (Somut Arabulucu) ---
    // Trafik polisimiz burası. Kimin ne zaman ne yapacağını bilir.
    public class ShopMediator : IGameMediator
    {
        // Mediator, yöneteceği tüm aktörleri tanımak zorundadır.
        public WalletSystem Wallet { get; set; }
        public InventorySystem Inventory { get; set; }
        public SoundSystem Sound { get; set; }
        public UISystem UI { get; set; }

        // MERKEZİ İLETİŞİM MANTIĞI
        public void Notify(object sender, string eventCode)
        {
            // Gelen mesajı parse et ve mantığı işlet
            if (eventCode.StartsWith("BUY_REQUEST"))
            {
                // Format: BUY_REQUEST:ItemName:Price
                var parts = eventCode.Split(':');
                string item = parts[1];
                int price = int.Parse(parts[2]);

                // Mantık Zinciri:
                // 1. Parayı düşmeye çalış
                if (Wallet.TrySpendGold(price))
                {
                    // 2. Para yettiyse envantere ekle
                    Inventory.AddItem(item);
                    // 3. Başarılı sesi çal
                    Sound.PlaySound("kaching.wav");
                    // 4. UI'da bilgi ver
                    UI.ShowMessage("Satın alma başarılı!");
                }
                else
                {
                    // Para yetmediyse başarısız sesi çal
                    Sound.PlaySound("error_buzz.wav");
                }
            }
            else if (eventCode == "INSUFFICIENT_FUNDS")
            {
                // Cüzdan hata verirse UI'ı uyar
                UI.ShowMessage("Paranız yetmiyor fakir dostum!");
            }
            else if (eventCode == "ITEM_ADDED")
            {
                // Envanter dolduğunda belki parti efekti patlatırız?
                // Şu anlık boş.
            }
        }
    }

    // --- 5. Client (Oyun Döngüsü) ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Mediator Pattern (Mağaza Sistemi) ---\n");

            // 1. Bileşenleri oluştur
            var wallet = new WalletSystem();
            var inventory = new InventorySystem();
            var sound = new SoundSystem();
            var ui = new UISystem();

            // 2. Mediator'ı oluştur ve bileşenleri bağla
            var mediator = new ShopMediator
            {
                Wallet = wallet,
                Inventory = inventory,
                Sound = sound,
                UI = ui
            };

            // 3. Bileşenlere Mediator'ı tanıt (Çift yönlü bağlantı)
            wallet.SetMediator(mediator);
            inventory.SetMediator(mediator);
            sound.SetMediator(mediator);
            ui.SetMediator(mediator);

            // --- SENARYOLAR ---

            // Senaryo 1: Başarılı Satın Alma (Demir Kılıç: 100 Altın)
            // Başlangıç parası: 500
            ui.BuyButtonClicked("Demir Kılıç", 100);

            Console.WriteLine("\n" + new string('-', 40));

            // Senaryo 2: Yetersiz Bakiye (Efsanevi Zırh: 1000 Altın)
            // Kalan para: 400
            ui.BuyButtonClicked("Efsanevi Zırh", 1000);

            Console.ReadKey();
        }
    }
}