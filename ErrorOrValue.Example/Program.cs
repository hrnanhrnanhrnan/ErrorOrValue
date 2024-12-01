namespace ErrorOrValue.Example;

class Program
{
    static async Task Main()
    {
        // Supports a simple Try for actions which returns a nullable Exception, to  
        var error = ErrorOr.Try(() => System.Console.WriteLine("hejsan"), ex => new InvalidDataException());

        if (error is not null)
        {
            Console.WriteLine(error.Message);
        }

        // 
        var errorFromTask = await ErrorOr.TryAsync(() => UserCreator.FireAndForgetAsync(), ex => new InvalidDataException());

        if (errorFromTask is not null)
        {
            Console.WriteLine(errorFromTask.Message);
        }

        // You can use the Result object directly, which has propertys as Error, Value, IsSuccess and IsFailure
        var result = await ErrorOr.TryAsync(
            () => UserCreator.CreateUserAsync(""), // will throw ArgumentException
            (ex) => new InvalidCastException(ex.Message)
        );

        if (result.IsFailure)
        {
            Console.WriteLine(result.Error); // System.InvalidCastException
        }
        else
        {
            Console.WriteLine(result.Value);
        }
        
        // Or you can use deconstruction to get a tuple of possible exception or the value
        var (userCreationError, user) = await ErrorOr.TryAsync(() => UserCreator.CreateUserAsync("Rob"));
        
        if (userCreationError is not null)
        {
            Console.WriteLine(userCreationError.Message);
        }

        Console.WriteLine(user.Name); // Robin

        // And you can also specify the exceptions you are expecting, 
        // if the exception thrown is not among the expected exceptions, the exception will not be catched
        var (argumentException, otherUser) = await ErrorOr.TryAsync(
            () => UserCreator.CreateUserAsync(""),
            typeof(ArgumentException), typeof(ArgumentNullException)
        );

        if (argumentException is not null)
        {
            Console.WriteLine(argumentException); // System.ArgumentException
        }
    }
}