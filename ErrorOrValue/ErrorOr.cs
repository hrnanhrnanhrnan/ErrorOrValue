using ErrorOrValue.Results;

namespace ErrorOrValue;

/// <summary>
/// Provides a set of methods for executing actions and functions that may throw exceptions,
/// wrapping their outcomes in <see cref="ErrorOr{TResult}"/> or <see cref="ErrorOr{TResult,TException}"/>
/// structures or returning caught exceptions. Supports both synchronous and asynchronous operations.
/// </summary>
public static class ErrorOr
{
    /// <summary>
    /// Executes an <paramref name="action"/> and returns <c>null</c> if it succeeds, otherwise the caught exception.
    /// If <paramref name="expectedExceptions"/> is specified and an exception is thrown, 
    /// the exception thrown has to be among the <paramref name="expectedExceptions"/> to be returned, otherwise it is rethrown.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="expectedExceptions">An optional array of exception types that should be returned rather than rethrown.</param>
    /// <returns>
    /// <c>null</c> if the action completes successfully, otherwise the caught exception. 
    /// If <paramref name="expectedExceptions"/> is specified and thrown exception is among expeted types,
    /// caught exception is returned, otherwise rethrown.
    /// </returns>
    public static Exception? Try(Action action, params Type[]? expectedExceptions)
        => TryExecute<Exception>(action, expectedExceptions: expectedExceptions);

    /// <summary>
    /// Executes an <paramref name="action"/> and returns a transformed exception if one occurs.
    /// If the action completes successfully, <c>null</c> is returned. 
    /// Use <paramref name="catchHandler"/> to convert the caught exception into a <typeparamref name="TException"/>.
    /// </summary>
    /// <typeparam name="TException">The type of exception to be returned on failure.</typeparam>
    /// <param name="action">The action to execute.</param>
    /// <param name="catchHandler">A function that transforms any thrown exception into a <typeparamref name="TException"/>.</param>
    /// <returns>
    /// <c>null</c> if the action completes successfully, or a <typeparamref name="TException"/> if an exception is caught.
    /// </returns>
    public static TException? Try<TException>(
        Action action,
        Func<Exception, TException> catchHandler)
            where TException : Exception
        => TryExecute(action, catchHandler);

    /// <summary>
    /// Executes a function that returns a result. An <see cref="ErrorOr{TResult}"/> is returned with the caught exception on failure
    /// or the result of the function on success.
    /// If <paramref name="expectedExceptions"/> is specified and an exception is thrown, 
    /// the exception thrown has to be among the <paramref name="expectedExceptions"/> to be returned, otherwise it is rethrown.
    /// </summary>
    /// <typeparam name="TResult">The type of value returned on success.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="expectedExceptions">An optional array of exception types that should be captured rather than rethrown.</param>
    /// <returns>
    /// An <see cref="ErrorOr{TResult}"/> containing either the successful result or the captured exception.
    /// If <paramref name="expectedExceptions"/> is specified and thrown exception is among expeted types,
    /// caught exception is returned as part of <see cref="ErrorOr{TResult}"/>, otherwise rethrown.
    /// </returns>
    public static ErrorOr<TResult> Try<TResult>(
        Func<TResult> func,
        params Type[]? expectedExceptions)
        => TryExecute(func, expectedExceptions);

    /// <summary>
    /// Executes a function that returns a result. If an exception is thrown, it is transformed
    /// using <paramref name="catchHandler"/> into a <typeparamref name="TException"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of value returned on success.</typeparam>
    /// <typeparam name="TException">The type of exception returned on failure.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="catchHandler">A function that transforms a caught exception into <typeparamref name="TException"/>.</param>
    /// <returns>
    /// An <see cref="ErrorOr{TResult,TException}"/> containing either the successful result or the transformed exception.
    /// </returns>
    public static ErrorOr<TResult, TException> Try<TResult, TException>(
        Func<TResult> func,
        Func<Exception, TException> catchHandler)
        where TException : Exception
        => TryExecute(func, catchHandler);

    /// <summary>
    /// Asynchronously executes a function that returns a <see cref="Task"/>. <c>null</c> is returned if it succeeds, 
    /// otherwise the caught exception.
    /// If <paramref name="expectedExceptions"/> is specified and an exception is thrown, 
    /// the exception thrown has to be among the <paramref name="expectedExceptions"/> to be returned, 
    /// otherwise it is rethrown.
    /// </summary>
    /// <param name="func">The async function to execute.</param>
    /// <param name="expectedExceptions">An optional array of exception types that should be returned rather than rethrown.</param>
    /// <returns>A <see cref="Task{TResult}"/> wrapping an <see cref="Exception"/> if one is caught, or <c>null</c> on success.</returns>
    public static Task<Exception?> TryAsync(
        Func<Task> func,
        params Type[]? expectedExceptions)
        => TryExecuteAsync<Exception>(func, expectedExceptions: expectedExceptions);

    /// <summary>
    /// Asynchronously executes a function that returns a <see cref="Task"/>. If an exception occurs,
    /// it is transformed by <paramref name="catchHandler"/>. Returns <c>null</c> on success.
    /// </summary>
    /// <typeparam name="TException">The type of exception returned on failure.</typeparam>
    /// <param name="func">The async function to execute.</param>
    /// <param name="catchHandler">A function that transforms a caught exception into <typeparamref name="TException"/>.</param>
    /// <returns>A <see cref="Task{TResult}"/> that yields <c>null</c> if successful or <typeparamref name="TException"/> on failure.</returns>
    public static Task<TException?> TryAsync<TException>(
        Func<Task> func,
        Func<Exception, TException> catchHandler)
            where TException : Exception
        => TryExecuteAsync(func, catchHandler);

    /// <summary>
    /// Asynchronously executes a function that returns a <see cref="Task{TResult}"/>.
    /// On success, returns an <see cref="ErrorOr{TResult}"/> with the result.
    /// If an expected exception is thrown, returns an <see cref="ErrorOr{TResult}"/> containing the exception.
    /// If <paramref name="expectedExceptions"/> is specified and an exception is thrown, 
    /// the exception thrown has to be among the <paramref name="expectedExceptions"/> to be returned, 
    /// otherwise it is rethrown.
    /// </summary>
    /// <typeparam name="TResult">The type of value returned on success.</typeparam>
    /// <param name="func">The async function returning <typeparamref name="TResult"/>.</param>
    /// <param name="expectedExceptions">An optional array of exception types that should be captured rather than rethrown.</param>
    /// <returns>An <see cref="ErrorOr{TResult}"/> that either holds the successful result or the expected exception.</returns>
    public static Task<ErrorOr<TResult>> TryAsync<TResult>(
        Func<Task<TResult>> func,
        params Type[]? expectedExceptions)
        => TryExecuteAsync(func, expectedExceptions);

    /// <summary>
    /// Asynchronously executes a function returning a <see cref="Task{TResult}"/>.
    /// If an exception is thrown, it is transformed by <paramref name="catchHandler"/>.
    /// On success, returns an <see cref="ErrorOr{TResult,TException}"/> holding the result.
    /// On failure, returns one holding the transformed exception.
    /// </summary>
    /// <typeparam name="TResult">The type of value returned on success.</typeparam>
    /// <typeparam name="TException">The type of exception returned on failure.</typeparam>
    /// <param name="func">The async function returning <typeparamref name="TResult"/>.</param>
    /// <param name="catchHandler">A function that transforms a caught exception into <typeparamref name="TException"/>.</param>
    /// <returns>
    /// An <see cref="ErrorOr{TResult,TException}"/> containing either the successful result or the transformed exception.
    /// </returns>
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
            if (expectedExceptions.ShouldMatch(ex))
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
            if (expectedExceptions.ShouldMatch(ex))
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
            if (expectedExceptions.ShouldMatch(ex))
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

            if (expectedExceptions.ShouldMatch(ex))
            {
                return catchHandler?.Invoke(ex) ?? (TException)ex;
            }

            throw;
        }
    }

    private static bool ShouldMatch(this Type[]? expectedTypes, Exception exception)
        => expectedTypes is not { Length: > 0 } || expectedTypes.Contains(exception.GetType());
}