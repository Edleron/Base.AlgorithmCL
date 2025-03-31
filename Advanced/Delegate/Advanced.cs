using System;

/*
Ertuğrul, gelişmiş kullanımda delegate'ler şu avantajları sağlar:

Multicast Delegate: Bir delegate, birden fazla metodu referans edebilir. 
Bu sayede aynı anda birden fazla işlemi tetikleyebilirsiniz.

Callback Mekanizması: Asenkron işlemlerde veya belirli bir iş tamamlandığında çalıştırılması 
gereken metotlar için callback olarak kullanılabilir.

Esneklik ve Gevşek Bağlılık (Loose Coupling): Kodunuzun farklı bölümleri arasında sıkı bağımlılığı azaltarak,
dinamik ve modüler yapılar kurmanıza olanak tanır.

Lambda İfadeleri ve Anonim Metotlar: Kodunuzu daha okunabilir ve kısa hale getirmek için lambda ifadeleri ve 
anonim metotlarla kullanılabilir.

*/
namespace AdvancedDelegateExample
{
    // Multicast delegate tanımı: Birden fazla metodu çağırabilir.
    delegate void BildirimHandler(string mesaj);

    class Program
    {
        // İlk bildirim metodu: Email gönderimi simülasyonu.
        static void EmailBildirim(string mesaj)
        {
            Console.WriteLine("Email Bildirimi: " + mesaj);
        }

        // İkinci bildirim metodu: SMS gönderimi simülasyonu.
        static void SmsBildirim(string mesaj)
        {
            Console.WriteLine("SMS Bildirimi: " + mesaj);
        }

        // Callback metodu: Bir iş tamamlandığında çağrılacak metod.
        static void IslemTamamlandi()
        {
            Console.WriteLine("İşlem tamamlandı!");
        }
        
        static void Main(string[] args)
        {
            // Multicast delegate kullanımı:
            // 'bildirim' delegate'i hem EmailBildirim hem de SmsBildirim metodlarını tutar.
            BildirimHandler bildirim = EmailBildirim;
            bildirim += SmsBildirim;

            // Delegate çağrıldığında, her iki metod da sırasıyla çalışır.
            bildirim("Sistem güncellemesi yapıldı.");

            // Callback olarak delegate kullanımı:
            // İşlem sonrası belirli bir metodun çağrılmasını sağlayan callback mekanizması.
            IslemSonrasi(islemCallback: IslemTamamlandi);
            
            // Lambda ifadesiyle anonim metot kullanımı:
            // Kısa ve okunabilir bildirim mesajı için lambda kullanımı.
            BildirimHandler lambdaBildirim = mesaj => Console.WriteLine("Lambda Bildirimi: " + mesaj);
            lambdaBildirim("Yeni mesaj geldi.");

            Console.ReadLine();
        }

        // İşlem tamamlandığında callback delegate'i ile bildirimin yapıldığı metod.
        static void IslemSonrasi(Action islemCallback)
        {
            // İşlemler burada yapılır...
            Console.WriteLine("İşlem yapılıyor...");

            // İşlem tamamlandığında callback metodu çağrılır.
            islemCallback?.Invoke();
        }
    }
}
