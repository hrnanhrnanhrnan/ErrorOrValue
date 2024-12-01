namespace ErrorOrValue.Tests;
internal static class TestHelpers
{
    public static Task<int> GetIntOrThrowAsync(
        bool throwException = false, 
        Exception? exceptionToThrow = null)
    {
        if (throwException && exceptionToThrow is not null)
        {
            throw exceptionToThrow;
        }

        return Task.FromResult(1);
    }

    public static Task RunTaskOrThrowAsync(
        bool throwException = false, 
        Exception? exceptionToThrow = null)
        => GetIntOrThrowAsync(throwException, exceptionToThrow);
}