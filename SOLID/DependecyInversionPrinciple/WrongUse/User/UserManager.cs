namespace  DependecyInversionPrinciple.WrongUse.User;

public class UserManager
{
    private readonly Base.MySQLDatabase _database;

    public UserManager()
    {
        _database = new Base.MySQLDatabase();
    }

    public void SaveUser(string user)
    {
        _database.Save(user);
    }
}