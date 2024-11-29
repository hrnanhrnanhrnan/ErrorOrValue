namespace ErrorOrValue.Results;

public readonly struct ErrorOr<TResult, TException> where TException : Exception
{
    public TResult Value { get; }
    public TException? Error { get; }
    public bool IsSuccess => Error is null;
    public bool IsFailure => !IsSuccess;

    public ErrorOr(TResult value)
    {
        Error = null;
        Value = value;
    }

    public ErrorOr(TException exception)
    {
        Error = exception;
        Value = default!;
    }

    public void Deconstruct(out TException? error, out TResult value)
    {
        error = Error;
        value = Value;
    }
}