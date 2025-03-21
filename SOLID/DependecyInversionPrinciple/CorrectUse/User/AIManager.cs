namespace  DependecyInversionPrinciple.CorrectUse.User;

public class AIManager
{
    private readonly Interface.ILogger _logger;

    // DI tekniğini kullanarak constructor üzerinden bağımlılık veriyoruz
    public AIManager(Interface.ILogger logger)
    {
        _logger = logger;
    }

    public void SaveAI(string name)
    {
        // işlemler...
        _logger.Log($"{name} kaydedildi.");
    }
}