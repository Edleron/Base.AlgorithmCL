using System;
using System.Collections.Generic;

/*
    Predicate<T>:
    isAdult adlı Predicate, bir Person nesnesinin yaşına bakarak yetişkin olup olmadığını belirliyor. 
    Böylece liste içerisinden FindAll metodu ile sadece yetişkinleri çekebiliyoruz.
*/

/*
    Func<T, TResult>:
    createGreeting adlı Func, bir Person nesnesini alıp kişiye özel bir selam mesajı üretiyor. 
    Func, dönüş tipi olan string sayesinde mesajı oluşturup döndürüyor.
*/

/*
    Action<T>:
    printMessage basit bir Action örneği. Ayrıca, compareAges ve zincirleme örneği 
    olan chainedActions gibi çok parametreli ve birleştirilmiş Action delegate’leri de kullanılmıştır.
*/

namespace AdvancedDelegateExample
{
    // Örnek bir model: Person
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
        
        public override string ToString() => $"Name: {Name}, Age: {Age}";
    }

    class Program
    {

        static List<Person> persons = new List<Person>
        {
            new Person("Alice", 30),
            new Person("Bob", 15),
            new Person("Charlie", 25),
            new Person("Dave", 12),
            new Person("Eve", 35)
        };

        static void Main(string[] args)
        {
            // Örnek veri: Kişiler listesi
            // Her konsept için ayrı metodlar çağrılıyor.
            RunPredicateExample();
            RunFuncExample();
            RunActionExample();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // Predicate kullanımını gösteren metod
        static void RunPredicateExample()
        {
            Console.WriteLine("Predicate Örneği - Yetişkin Filtreleme:");
            // Predicate<Person>: Kişinin 18 ve üzeri olup olmadığını kontrol ediyor.
            Predicate<Person> isAdult = p => p.Age >= 18;
            List<Person> adults = persons.FindAll(isAdult);
            
            // Yetişkin kişileri ekrana yazdırıyoruz.
            foreach (var person in adults)
            {
                Console.WriteLine(person);
            }
            Console.WriteLine();
        }


        // Func kullanımını gösteren metod
        static void RunFuncExample()
        {
            Console.WriteLine("Func Örneği - Selam Mesajı Oluşturma:");
            // Func<Person, string>: Person nesnesini alıp, selam mesajı oluşturuyor.
            Func<Person, string> createGreeting = p => $"Hello, {p.Name}! You are {p.Age} years old.";
            
            // Listedeki her kişi için selam mesajını oluşturup yazdırıyoruz.
            foreach (var person in persons)
            {
                string greeting = createGreeting(person);
                Console.WriteLine(greeting);
            }
            Console.WriteLine();
        }

        // Action kullanımını gösteren metod
        static void RunActionExample()
        {
            Console.WriteLine("Action Örneği - Mesaj Yazdırma, Yaş Karşılaştırma ve Zincirleme:");

            // Basit bir Action: string parametre alıp ekrana yazdırıyor.
            Action<string> printMessage = msg => Console.WriteLine(msg);
            printMessage("Bu bir Action mesajıdır.");

            // İki parametreli Action: İki kişinin yaşını karşılaştırıyor.
            Action<Person, Person> compareAges = (p1, p2) =>
            {
                if (p1.Age > p2.Age)
                    Console.WriteLine($"{p1.Name} is older than {p2.Name}");
                else if (p1.Age < p2.Age)
                    Console.WriteLine($"{p2.Name} is older than {p1.Name}");
                else
                    Console.WriteLine($"{p1.Name} and {p2.Name} are of the same age");
            };

            // Listedeki ilk iki kişiyi karşılaştırıyoruz.
            compareAges(persons[0], persons[1]);

            // Zincirleme Action örneği:
            Action<string> addExclamation = s => Console.WriteLine(s + "!!!");
            Action<string> printUpperCase = s => Console.WriteLine(s.ToUpper());

            // Zincirleme (chaining) ile Action'ları birleştiriyoruz.
            Action<string> chainedActions = addExclamation;
            chainedActions += printUpperCase;
            
            Console.WriteLine("Chained Actions:");
            chainedActions("Test message");
            Console.WriteLine();
        }
    }
}
