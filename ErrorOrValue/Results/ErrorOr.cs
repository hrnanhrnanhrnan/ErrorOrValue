namespace ErrorOrValue.Results;

/// <summary>
/// A readonly struct that stores either a successful result (<typeparamref name="TResult"/>)
/// or an exception (<typeparamref name="TError"/>). It is the base building block for <see cref="ErrorOr{TResult}"/>
/// and <see cref="ErrorOr{TResult,TError}"/>.
/// </summary>
/// <typeparam name="TResult">The type of the successful result value.</typeparam>
/// <typeparam name="TError">The type of the error, which must be an <see cref="Exception"/>.</typeparam>
internal readonly struct ErrorOrBase<TResult, TError> where TError : Exception
{
    /// <summary>
    /// Gets the value of a successful result.
    /// </summary>
    public TResult Value { get; }

    /// <summary>
    /// Gets the exception if the operation failed, or <c>null</c> if it succeeded.
    /// </summary>
    public TError? Error { get; }

    /// <summary>
    /// Indicates whether the operation succeeded (no error).
    /// </summary>
    public bool IsSuccess => Error is null;

    /// <summary>
    /// Indicates whether the operation failed (there is an error).
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Initializes a successful result.
    /// </summary>
    /// <param name="value">The successful result value.</param>
    public ErrorOrBase(TResult value)
    {
        Error = null;
        Value = value;
    }

    /// <summary>
    /// Initializes a failed result.
    /// </summary>
    /// <param name="exception">The exception representing the failure.</param>
    public ErrorOrBase(TError exception)
    {
        Error = exception;
        Value = default!;
    }

    /// <summary>
    /// Deconstructs into an error and value pair.
    /// </summary>
    /// <param name="error">Receives the error if present.</param>
    /// <param name="value">Receives the successful result if present.</param>
    public void Deconstruct(out TError? error, out TResult value)
    {
        error = Error;
        value = Value;
    }
}

/// <summary>
/// Represents the result of an operation that can either succeed with a <typeparamref name="TResult"/> 
/// or fail with a <typeparamref name="TException"/>.
/// </summary>
/// <typeparam name="TResult">The type of the operation's successful result.</typeparam>
/// <typeparam name="TException">The type of exception returned on failure.</typeparam>
public readonly struct ErrorOr<TResult, TException> where TException : Exception
{
    private readonly ErrorOrBase<TResult, TException> _base;

    /// <summary>
    /// The successful result, if the operation succeeded.
    /// </summary>
    public TResult Value => _base.Value;

    /// <summary>
    /// The exception if the operation failed; otherwise <c>null</c>.
    /// </summary>
    public TException? Error => _base.Error;

    /// <summary>
    /// Indicates whether the operation succeeded.
    /// </summary>
    public bool IsSuccess => _base.IsSuccess;

    /// <summary>
    /// Indicates whether the operation failed.
    /// </summary>
    public bool IsFailure => _base.IsFailure;

    /// <summary>
    /// Initializes a successful result.
    /// </summary>
    /// <param name="value">The successful result value.</param>
    public ErrorOr(TResult value) => _base = new ErrorOrBase<TResult, TException>(value);

    /// <summary>
    /// Initializes a failed result.
    /// </summary>
    /// <param name="exception">The exception representing the failure.</param>
    public ErrorOr(TException exception) => _base = new ErrorOrBase<TResult, TException>(exception);

    /// <summary>
    /// Deconstructs the instance into its error and value components.
    /// </summary>
    /// <param name="error">Receives the exception if present.</param>
    /// <param name="value">Receives the successful result if present.</param>
    public void Deconstruct(out TException? error, out TResult value)
    {
        error = Error;
        value = Value;
    }
}

/// <summary>
/// Represents the result of an operation that can either succeed with a <typeparamref name="TResult"/> 
/// or fail with an <see cref="Exception"/>.
/// </summary>
/// <typeparam name="TResult">The type of the operation's successful result.</typeparam>
public readonly struct ErrorOr<TResult>
{
    private readonly ErrorOrBase<TResult, Exception> _base;

    /// <summary>
    /// The successful result, if the operation succeeded.
    /// </summary>
    public TResult Value => _base.Value;

    /// <summary>
    /// The exception if the operation failed; otherwise <c>null</c>.
    /// </summary>
    public Exception? Error => _base.Error;

    /// <summary>
    /// Indicates whether the operation succeeded.
    /// </summary>
    public bool IsSuccess => _base.IsSuccess;

    /// <summary>
    /// Indicates whether the operation failed.
    /// </summary>
    public bool IsFailure => _base.IsFailure;

    /// <summary>
    /// Initializes a successful result.
    /// </summary>
    /// <param name="value">The successful result value.</param>
    public ErrorOr(TResult value) => _base = new ErrorOrBase<TResult, Exception>(value);

    /// <summary>
    /// Initializes a failed result.
    /// </summary>
    /// <param name="exception">The exception representing the failure.</param>
    public ErrorOr(Exception exception) => _base = new ErrorOrBase<TResult, Exception>(exception);

    /// <summary>
    /// Deconstructs the instance into its error and value components.
    /// </summary>
    /// <param name="error">Receives the exception if present.</param>
    /// <param name="value">Receives the successful result if present.</param>
    public void Deconstruct(out Exception? error, out TResult value)
    {
        error = Error;
        value = Value;
    }
}