namespace ErrorOrValue.Results;

internal readonly struct ErrorOrBase<TResult, TError> where TError : Exception
{
    public TResult Value { get; }
    public TError? Error { get; }
    public bool IsSuccess => Error is null;
    public bool IsFailure => !IsSuccess;

    public ErrorOrBase(TResult value)
    {
        Error = null;
        Value = value;
    }

    public ErrorOrBase(TError exception)
    {
        Error = exception;
        Value = default!;
    }

    public void Deconstruct(out TError? error, out TResult value)
    {
        error = Error;
        value = Value;
    }
}


public readonly struct ErrorOr<TResult, TException> where TException : Exception
{
    private readonly ErrorOrBase<TResult, TException> _base;
    public TResult Value => _base.Value;
    public TException? Error => _base.Error;
    public bool IsSuccess => _base.IsSuccess;
    public bool IsFailure => _base.IsFailure;

    public ErrorOr(TResult value) => _base = new ErrorOrBase<TResult, TException>(value);
    public ErrorOr(TException exception) => _base = new ErrorOrBase<TResult, TException>(exception);

    public void Deconstruct(out TException? error, out TResult value)
    {
        error = Error;
        value = Value;
    }
}

public readonly struct ErrorOr<TResult>
{
    private readonly ErrorOrBase<TResult, Exception> _base;
    public TResult Value => _base.Value;
    public Exception? Error => _base.Error;
    public bool IsSuccess => _base.IsSuccess;
    public bool IsFailure => _base.IsFailure;

    public ErrorOr(TResult value) => _base = new ErrorOrBase<TResult, Exception>(value);
    public ErrorOr(Exception exception) => _base = new ErrorOrBase<TResult, Exception>(exception);

    public void Deconstruct(out Exception? error, out TResult value)
    {
        error = Error;
        value = Value;
    }
}