using ErrorOrValue;

namespace ErrorsAsValues.Example;

class Program
{
    static void Main()
    {
        var error = ErrorOr.Try(() => User.DoSomething());

        var (userCreationError, number) = ErrorOr.Try(
            () => 
            {
                using var disp = new DisposableClass();
                throw new InvalidOperationException();
                return disp.GetInt();
            },
            new Type[] { typeof(OperationCanceledException) }
        );

        if (userCreationError is not null)
        {
            System.Console.WriteLine(userCreationError.GetType());
            System.Console.WriteLine(userCreationError.Message);
        }
        
        System.Console.WriteLine(number);
    }

}

class DisposableClass : IDisposable
{
    public int GetInt() => 21;
    public void Dispose()
    {
        Console.WriteLine("Disposing");
    }
}

class User
{
    public string? Name { get; set;}
    public static void DoSomething()
    {
        Console.WriteLine("Hejsan");
    }

    public static Task DoSomethingAsync()
    {
        return Task.Run(() => System.Console.WriteLine("Hejsan"));
    }

    public static User CreateUser(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        return new User { Name = name };
    }
    public static Task<User> CreateUserAsync(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        return Task.FromResult(CreateUser(name));
    }
}
