namespace LiskovSubstitutionPrinciple.WrongUse.Logic;

public class Penguin : Abstract.Bird
{
    public override void Fly()
    {
        throw new NotImplementedException("Penguen uçamaz.");
    }
}