class UserCreator
{
    public static void FireAndForget()
    {
        throw new NotImplementedException();
    }

    public static async Task FireAndForgetAsync()
    {
        await Task.Run(() => FireAndForget());
    }

    public static User CreateUser(string name)
    {
        if (name is not { Length: > 2 })
        {
            throw new ArgumentException(name);
        }

        return new User(name);
    }

    public static Task<User> CreateUserAsync(string name)
    {
        return Task.FromResult(CreateUser(name));
    }
}