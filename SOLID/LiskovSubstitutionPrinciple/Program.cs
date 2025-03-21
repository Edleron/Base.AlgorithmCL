namespace LiskovSubstitutionPrinciple;

class Program
{
    /*
        LSP, SOLID prensiplerinin üçüncüsüdür ve şöyle der:

        "Bir üst sınıfın nesneleri, alt sınıfların nesneleriyle, programın doğruluğunu bozmadan değiştirilebilir olmalıdır."

        Kısaca: Alt sınıflar, üst sınıfın tüm davranışlarını aynen desteklemeli ve beklenmedik sonuçlar üretmemelidir.
    */
    static void Main(string[] args)
    {
        CorrectUsed();
        WrongUsed();
    }

    static void CorrectUsed()
    {
        CorrectUse.Abstract.Flying.FlyingBird pigeon = new CorrectUse.Logic.Pigeon();
        pigeon.Fly();
        pigeon.Eat(); 

        CorrectUse.Abstract.NonFlying.NonFlyingBird penguin = new CorrectUse.Logic.Penguin();
        penguin.Eat();
    }

    static void WrongUsed() {
        WrongUse.Abstract.Bird bird = new WrongUse.Logic.Penguin();
        bird.Fly();
    }
}
