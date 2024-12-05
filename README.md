# ErrorOrValue 
A **minimalistic** NuGet package that provides a simple wrapper around traditional `try-catch` blocks. Instead of throwing exceptions, it returns them as values, making error handling more explicit and easier to work with. This pattern allows you to handle both synchronous and asynchronous operations without the usual complexity of exception handling, and it gives you full control over expected exceptions and custom exception transformations. 

## Key Features 

- **Minimalistic wrapper** around `try-catch`: No additional complexity, just a straightforward mechanism for handling exceptions as values. 

- **Handle void methods**: Run actions that return either `null` when successful, or an `Exception` if one occurs. 

- **Handle functions**: Execute functions that return either a result value or an exception, wrapped in an `ErrorOr` type. 

- **Custom exception transformation**: Supply a catch handler to transform the caught exception into a specific exception type. 

- **Specify expected exceptions**: Provide a list of exception types to catch. If the thrown exception is not in the list, it is rethrown as usual. 

- **Async support**: All functionalities are also available for asynchronous `Task`-returning methods, keeping your async code clean and consistent. 

## Installation 

Through the nuget package manager:

```powershell 
Install-Package ErrorOrValue 
```

Or via the .NET CLI:

```bash
dotnet add package ErrorOrValue
``` 
## Basic Usage 

### Running Actions 
Call a void-returning action that might throw: 

```csharp
using ErrorOrValue; 

Exception? error = ErrorOr.Try(() => 
{
  // Your code that may throw exceptions 
}); 

if (error is not null) 
{ 
  // Handle the exception 
}  
```

You can also specify which exceptions you expect. If a thrown exception isn't listed, it will be rethrown: 

```csharp 
Exception? error = ErrorOr.Try(() => 
  {
    // Your code 
  }, 
  typeof(ArgumentException), typeof(InvalidOperationException)
);
```

### Running Functions with Return Types 

Execute a function with a return type and get either a result or an exception:

```csharp 
using ErrorOrValue; 
using ErrorOrValue.Results; 

ErrorOr<int> result = ErrorOr.Try(() => 
{ 
  // Your code that returns an int 
}); 

if (result.IsSuccess) 
{ 
  int value = result.Value; // Use the value 
} 
else 
{ 
  Exception caught = result.Error; // Handle the exception 
} 

// Or with deconstruction
var (error, user) = ErrorOr.Try(() => GetUser()); 
```

### Transforming Exceptions with a Catch Handler 
You can provide a custom handler to transform the caught exception into a specific exception type: 
```csharp 
CustomException? customError = ErrorOr.Try(() => 
  { 
    // Your code 
  }, 
  ex => new CustomException("Custom message", ex)
);
```

For functions that return values: 
```csharp 
ErrorOr<string, CustomException> result = ErrorOr.Try(() => 
  { 
    // Your code returning a string 
  }, 
  ex => new CustomException("Custom message", ex)
); 

if (result.IsSuccess) 
{ 
  string value = result.Value; 
} 
else 
{ 
  CustomException customEx = result.Error; // Handle the transformed exception 
} 

// Or with deconstruction
var (customError, customer) = ErrorOr.Try(
  () => GetCustomer(), 
  ex => new CustomException("Custom message", ex)
); 
```

### Async Support 
All of these patterns also work asynchronously:

```csharp 
Exception? asyncError = await ErrorOr.TryAsync(async () => 
{ 
  // Your async code that may throw 
}); 

ErrorOr<int> asyncResult = await ErrorOr.TryAsync(async () => 
{ 
  // Your async code returning int 
}); 

// Or with deconstruction
var (customError, customer) = await ErrorOr.TryAsync(
  () => GetCustomerAsync(), 
  ex => new CustomException("Custom message", ex)
); 
```

As with synchronous methods, you can specify expected exceptions or provide a custom catch handler for asynchronous functions as well. 

## API Overview 
- `Try(Action action, params Type[]? expectedExceptions)` 
- `Try<TException>(Action action, Func<Exception, TException> catchHandler)` 
- `Try<TResult>(Func<TResult> func, params Type[]? expectedExceptions)` 
- `Try<TResult, TException>(Func<TResult> func, Func<Exception, TException> catchHandler)` 

And their async counterparts: 
- `TryAsync(Func<Task> func, params Type[]? expectedExceptions)` 
- `TryAsync<TException>(Func<Task> func, Func<Exception, TException> catchHandler)` 
- `TryAsync<TResult>(Func<Task<TResult>> func, params Type[]? expectedExceptions)` 
- `TryAsync<TResult, TException>(Func<Task<TResult>> func, Func<Exception, TException> catchHandler)` 

`ErrorOr<TResult>` and `ErrorOr<TResult, TException>` provide a clean, value-based approach to checking for success or failure without relying on thrown exceptions. 

## License
This project is licensed under the MIT License.