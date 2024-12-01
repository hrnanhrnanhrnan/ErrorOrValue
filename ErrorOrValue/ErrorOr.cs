using ErrorOrValue.Results;

namespace ErrorOrValue;

public static class ErrorOr
{
    public static Exception? Try(Action action, params Type[]? expectedExceptions)
        => TryExecute<Exception>(action, expectedExceptions: expectedExceptions);

    public static TException? Try<TException>(
        Action action,
        Func<Exception, TException> catchHandler)
            where TException : Exception
        => TryExecute(action, catchHandler);

    public static ErrorOr<TResult> Try<TResult>(
        Func<TResult> func,
        params Type[]? expectedExceptions)
        => TryExecute(func, expectedExceptions);

    public static ErrorOr<TResult, TException> Try<TResult, TException>(
        Func<TResult> func,
        Func<Exception, TException> catchHandler)
        where TException : Exception
        => TryExecute(func, catchHandler);

    public static Task<Exception?> TryAsync(
        Func<Task> func,
        params Type[]? expectedExceptions)
        => TryExecuteAsync<Exception>(func, expectedExceptions: expectedExceptions);

    public static Task<TException?> TryAsync<TException>(
        Func<Task> func,
        Func<Exception, TException> catchHandler)
            where TException : Exception
        => TryExecuteAsync(func, catchHandler);

    public static Task<ErrorOr<TResult>> TryAsync<TResult>(
        Func<Task<TResult>> func,
        params Type[]? expectedExceptions)
        => TryExecuteAsync(func, expectedExceptions);

    public static Task<ErrorOr<TResult, TException>> TryAsync<TResult, TException>(
        Func<Task<TResult>> func,
        Func<Exception, TException> catchHandler)
        where TException : Exception
        => TryExecuteAsync(func, catchHandler);

    private static TException? TryExecute<TException>(
        Action action,
        Func<Exception, TException>? catchHandler = null,
        Type[]? expectedExceptions = null)
            where TException : Exception
    {
        try
        {
            action();
            return null;
        }
        catch (Exception ex)
        {
            if (IsExceptionExpected(expectedExceptions, ex))
            {
                return catchHandler?.Invoke(ex) ?? (TException)ex;
            }

            throw;
        }
    }

    private static ErrorOr<TResult> TryExecute<TResult>(
        Func<TResult> func,
        Type[]? expectedExceptions = null)
    {
        try
        {
            return new ErrorOr<TResult>(func());
        }
        catch (Exception ex)
        {
            if (IsExceptionExpected(expectedExceptions, ex))
            {
                return new ErrorOr<TResult>(ex);
            }

            throw;
        }
    }

    private static ErrorOr<TResult, TException> TryExecute<TResult, TException>(
        Func<TResult> func,
        Func<Exception, TException> catchHandler)
        where TException : Exception
    {
        try
        {
            return new ErrorOr<TResult, TException>(func());
        }
        catch (Exception ex)
        {
            return new ErrorOr<TResult, TException>(catchHandler(ex));
        }
    }

    private static async Task<ErrorOr<TResult>> TryExecuteAsync<TResult>(
        Func<Task<TResult>> func,
        Type[]? expectedExceptions = null)
    {
        try
        {
            return new ErrorOr<TResult>(await func());
        }
        catch (Exception ex)
        {
            if (IsExceptionExpected(expectedExceptions, ex))
            {
                return new ErrorOr<TResult>(ex);
            }

            throw;
        }
    }

    private static async Task<ErrorOr<TResult, TException>> TryExecuteAsync<TResult, TException>(
        Func<Task<TResult>> func,
        Func<Exception, TException> catchHandler)
            where TException : Exception
    {
        try
        {
            return new ErrorOr<TResult, TException>(await func());
        }
        catch (Exception ex)
        {
            return new ErrorOr<TResult, TException>(catchHandler(ex));
        }
    }

    private static async Task<TException?> TryExecuteAsync<TException>(
        Func<Task> func,
        Func<Exception, TException>? catchHandler = default,
        Type[]? expectedExceptions = null)
        where TException : Exception
    {
        try
        {
            await func();
            return null;
        }
        catch (Exception ex)
        {
            if (IsExceptionExpected(expectedExceptions, ex))
            {
                return catchHandler?.Invoke(ex) ?? (TException)ex;
            }

            throw;
        }
    }

    private static bool IsExceptionExpected(Type[]? expectedTypes, Exception exception)
        => expectedTypes is not { Length: > 0 }  || expectedTypes.Contains(exception.GetType());
}