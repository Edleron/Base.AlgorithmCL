namespace LiskovSubstitutionPrinciple.CorrectUse.Logic;

public class Pigeon :  Abstract.Flying.FlyingBird
{
    public override void Fly()
    {
        Console.WriteLine("Güvercin uçuyor.");
    }

    public override void Eat()
    {
        Console.WriteLine("Güvercin yem yiyor.");
    }
}