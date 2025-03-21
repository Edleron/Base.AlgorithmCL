namespace  DependecyInversionPrinciple.CorrectUse.User;

public class UserManager
{
    private readonly Interface.IDatabase _database;

    public UserManager(Interface.IDatabase database)
    {
        _database = database;
    }

    public void SaveUser(string user)
    {
        _database.Save(user);
    }
}