namespace LiskovSubstitutionPrinciple.CorrectUse.Logic;

public class Penguin :  Abstract.NonFlying.NonFlyingBird
{
     public override void Eat()
    {
        Console.WriteLine("Penguen balık yiyor.");
    }
}