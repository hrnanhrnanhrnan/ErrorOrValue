using ErrorOrValue.Results;

namespace ErrorOrValue;

public static class ErrorOr
{
    public static ErrorOr<TResult, Exception> Try<TResult>(Func<TResult> func)
    {
        return TryWithWantedException<TResult, Exception>(func);
    }

    public static ErrorOr<TResult, TException> Try<TResult, TException>(
        Func<TResult> func, 
        Func<Exception, TException>? wantedException = default)
        where TException : Exception
    {
        return TryWithWantedException(func, wantedException);
    }

    public static ErrorOr<TResult, Exception> Try<TResult>(
        Func<TResult> func, 
        params Type[] expectedExceptions)
    {
        return TryWithExpectedExceptions(func, expectedExceptions);
    }

    public static Task<ErrorOr<TResult, Exception>> TryAsync<TResult>(Func<Task<TResult>> func)
    {
        return TryWithWantedExceptionAsync<TResult, Exception>(func);
    }

    public static Task<ErrorOr<TResult, TException>> TryAsync<TResult, TException>(
        Func<Task<TResult>> func,
        Func<Exception, TException>? wantedException = default)
        where TException : Exception
    {
        return TryWithWantedExceptionAsync(func, wantedException);
    }

    public static async Task<Exception?> TryAsync<TResult>(
        Func<Task> func)
    {
        try
        {
            await func();
            return default;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Exception? Try(Action action)
    {
        try
        {
            action();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    private static ErrorOr<TResult, TException> TryWithWantedException<TResult, TException>(
        Func<TResult> func,
        Func<Exception, TException>? wantedException = default
    )   where TException : Exception
    {
        try
        {
            return new ErrorOr<TResult, TException>(func());
        }
        catch (Exception ex)
        {
            return new ErrorOr<TResult, TException>(wantedException?.Invoke(ex) ?? (TException)ex);
        }
    }

    private static async Task<ErrorOr<TResult, TException>> TryWithWantedExceptionAsync<TResult, TException>(
        Func<Task<TResult>> func,
        Func<Exception, TException>? wantedException = default
    )   where TException : Exception
    {
        try
        {
            return new ErrorOr<TResult, TException>(await func());
        }
        catch (Exception ex)
        {
            return new ErrorOr<TResult, TException>(wantedException?.Invoke(ex) ?? (TException)ex);
        }
    }

    private static ErrorOr<TResult, Exception> TryWithExpectedExceptions<TResult>(
        Func<TResult> func,
        params Type[] expectedExceptions)
    {
        try
        {
            return new ErrorOr<TResult, Exception>(func());
        }
        catch (Exception ex)
        {
            var exceptionType = ex.GetType();

            if (!expectedExceptions.Contains(exceptionType))
            {
                throw;
            }

            return new ErrorOr<TResult, Exception>(ex);
        }
    }
}
