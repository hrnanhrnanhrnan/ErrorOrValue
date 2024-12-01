# ErrorOrValue
An uncomplicated and minimalist way to get exceptions or errors as values in C#, inspired by the error handling approach in Go.

## Overview
The **ErrorOrValue** package provides a simple mechanism to handle exceptions and errors as values, allowing you to write cleaner and more expressive code without extensive try-catch blocks. This approach is similar to error handling in Go (Golang), where functions return error values that can be checked by the caller.

## Features
- Minimalist and easy-to-use API.
- Supports both synchronous and asynchronous operations.
- Allows specifying expected exceptions to handle.
- Provides typed error handling with custom exception transformations.
- Reduces boilerplate code associated with exception handling.

## Installation
Install the package via NuGet Package Manager:

```powershell
Install-Package ErrorOrValue
```

Or via the .NET CLI:
```bash
dotnet add package ErrorOrValue
```

## Usage
### Basic Usage
The **ErrorOr** class provides static methods to execute actions or functions and capture any exceptions as return values.

#### Synchronous Functions
```csharp
using ErrorOrValue;

var result = ErrorOr.Try(() => SomeFunction());

// Check if the operation was successful
if (result.IsSuccess)
{
    // Use result.Value
}
else
{
    // Handle result.Error
}
```

#### Asynchronous Functions
```csharp
var result = await ErrorOr.TryAsync(async () => await SomeAsyncFunction());

if (result.IsSuccess)
{
    // Use result.Value
}
else
{
    // Handle result.Error
}
```

#### Result Deconstruction

The **ErrorOr<TResult>** type also supports deconstruction 
```csharp
var (error, user) = await ErrorOr.TryAsync(() => GetuserAsync());

if (error is not null)
{
    Console.WriteLine(error.Message);
}
else
{
    Console.WriteLine(user.Name); // Robin
}
```