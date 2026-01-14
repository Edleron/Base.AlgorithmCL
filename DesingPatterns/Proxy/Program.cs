using System;
using System.Threading;

namespace DesignPatterns.Proxy
{
    // --- 1. Subject (Ortak Arayüz) ---
    // İstemci (Client) hem gerçek nesneyle hem de vekille bu arayüz üzerinden konuşur.
    public interface IGraphicAsset
    {
        void Draw();
        string GetName();
    }

    // --- 2. Real Subject (Gerçek Nesne) ---
    // Yaratılması maliyetli olan, ağır kaynak tüketen sınıf.
    // Örn: Yüksek çözünürlüklü 3D model veya Texture.
    public class RealHighResModel : IGraphicAsset
    {
        private string _fileName;

        public RealHighResModel(string fileName)
        {
            _fileName = fileName;
            LoadFromDisk(fileName); // Constructor çalıştığı an ağır işlem başlar!
        }

        private void LoadFromDisk(string fileName)
        {
            Console.WriteLine($"[RealObject] '{fileName}' diskten yükleniyor... (Ağır İşlem)");
            // Simülasyon: Ağır yükleme işlemi
            Thread.Sleep(1500); 
            Console.WriteLine($"[RealObject] '{fileName}' belleğe yüklendi!");
        }

        public void Draw()
        {
            Console.WriteLine($"[RealObject] '{_fileName}' ekrana render ediliyor.");
        }

        public string GetName()
        {
            return _fileName;
        }
    }

    // --- 3. Proxy (Vekil Nesne) ---
    // Gerçek nesneyi taklit eder. Gerçek nesneye sadece İHTİYAÇ duyulduğunda erişir.
    public class ModelProxy : IGraphicAsset
    {
        private RealHighResModel _realModel;
        private string _fileName;

        public ModelProxy(string fileName)
        {
            _fileName = fileName;
            // DİKKAT: Burada RealHighResModel'i new'lemiyoruz!
            // Sadece dosya adını saklıyoruz, maliyet sıfıra yakın.
        }

        public void Draw()
        {
            // Lazy Loading (Tembel Yükleme) Mantığı:
            // Nesne daha önce oluşturulmamışsa, tam şu an oluştur.
            if (_realModel == null)
            {
                Console.WriteLine($"[Proxy] Draw isteği geldi, gerçek nesne yaratılıyor...");
                _realModel = new RealHighResModel(_fileName);
            }

            // İsteği gerçek nesneye ilet.
            _realModel.Draw();
        }

        public string GetName()
        {
            // Basit veriler için gerçek nesneyi yüklemeye gerek yok.
            // Proxy bu bilgiyi zaten taşıyor.
            return _fileName;
        }
    }

    // --- 4. Client (Oyun Motoru) ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Proxy Pattern (Lazy Loading Senaryosu) ---\n");

            // Senaryo: Oyun sahnesi yükleniyor.
            // Sahnede 3 tane ağır model var.
            // Proxy kullandığımız için oyun anında açılacak, bekleme olmayacak.

            Console.WriteLine(">>> Oyun Sahnesi Başlatılıyor...");
            
            IGraphicAsset model1 = new ModelProxy("Hero_4K_Skin.mesh");
            IGraphicAsset model2 = new ModelProxy("Dragon_Boss.mesh");
            IGraphicAsset model3 = new ModelProxy("Castle_Environment.mesh");

            Console.WriteLine(">>> Sahne nesneleri listeye eklendi (Henüz RAM kullanımı yok).");
            Console.WriteLine("-------------------------------------------------------------");

            // Oyuncu sadece kaleye bakıyor diyelim.
            // Sadece model3'ün Draw metodu çağrılacak.
            Console.WriteLine("\n>>> Oyuncu Kaleye bakıyor:");
            model3.Draw(); // İLK ÇAĞRI: Yükleme yapılır + Çizilir.

            Console.WriteLine("\n>>> Oyuncu hala Kaleye bakıyor (İkinci Frame):");
            model3.Draw(); // İKİNCİ ÇAĞRI: Yükleme YAPILMAZ, doğrudan çizilir.

            // model1 ve model2'nin Draw metodu hiç çağrılmadı.
            // Dolayısıyla "Hero" ve "Dragon" hiçbir zaman belleğe yüklenmedi.
            // Kaynak tasarrufu sağlandı.

            Console.ReadKey();
        }
    }
}