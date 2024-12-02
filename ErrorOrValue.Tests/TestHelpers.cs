namespace ErrorOrValue.Tests;
internal static class TestHelpers
{
    public static int GetNumberOneOrThrow(
        Exception? exceptionToThrow = null)
    {
        if (exceptionToThrow is not null)
        {
            throw exceptionToThrow;
        }

        return 1;
    }

    public static Task<int> GetNumberOneOrThrowAsync(
        Exception? exceptionToThrow = null)
        => Task.FromResult(GetNumberOneOrThrow(exceptionToThrow));

}