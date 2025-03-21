namespace LiskovSubstitutionPrinciple.WrongUse.Abstract;

public abstract class Bird
{
    public virtual void Fly()
    {
        Console.WriteLine("Uçabiliyor.");
    }
}