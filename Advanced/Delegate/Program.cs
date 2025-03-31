/* - Delegate - */

using System;

namespace MainDelegateExample
{
    class Program
    {
        // Delegate nedir?
        // C# dilinde delegate, belirli bir imzaya sahip metot referanslarını tutan, tip güvenli (type-safe)
        // bir temsilcidir. Bu yapı sayesinde metotları değişkenler gibi ele alabilir, metotları parametre olarak
        // geçebilir ve çalışma zamanında hangi metotların çağrılacağını dinamik olarak belirleyebilirsiniz.
        //
        // Örneğin, aşağıdaki delegate tanımı, imzası "int (int, int)" olan metotları referans olarak tutar:
        // delegate int MatematikIslemi(int a, int b);
        //
        // Delegate'ler özellikle olay yönetimi (event handling) ve geri çağırma (callback) mekanizmalarında
        // sıklıkla kullanılır. Bu sayede, esnek ve modüler bir kod yapısı elde edilmiş olur.

        // Matematik işlemleri için delegate tanımı

        /*
        C# programlama dilinde delegate, belirli bir imzaya sahip metotları temsil eden bir türdür. Başka bir değişle
        bir delegate, metodlaron refaranslarını tutabilen bir nesnedir. Bu metotlara parrametre olarak iletilbemize,
        bir metodu bir değişkene atayabilmemesize ve bir metotu başka bir metot içinde çağırabilmemize olanak tanır.

        Delegate'ler, özellikle olay yönetimi (event handling) ve geri çağırma (callback) mekanizmalarında sıklıkla kullanılır.
        */
        delegate int MatematikIslemi(int x, int y);

        // Toplama işlemini yapan metot
        static int Topla(int a, int b)
        {
            return a + b;
        }
        
        static void Main(string[] args)
        {
            // Delegate değişkeni oluşturuluyor ve Topla metodu bu delegate'e atanıyor.
            MatematikIslemi islem = Topla;
            
            // Delegate üzerinden Topla metodu çağrılarak işlemin sonucu alınıyor.
            int sonuc = islem(3, 4);
            Console.WriteLine("Sonuç: " + sonuc);

            // Bu örnek, delegate'lerin nasıl çalıştığını basitçe göstermektedir.
            // Yani delegate, belirli bir imzaya sahip metot referanslarını tutar ve
            // çalışma zamanında çağırılacak metodu esnek bir şekilde belirlemenizi sağlar.
            
            Console.ReadLine();
        }
    }
}
