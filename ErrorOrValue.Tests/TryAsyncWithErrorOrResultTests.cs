using FluentAssertions;

namespace ErrorOrValue.Tests;

public class TryAsyncWithErrorOrResultTests
{
    [Fact]
    public async Task TryAsync_Func_NoException_ReturnsValueAndNoException()
    {
        var (error, number) = await ErrorOr.TryAsync(() => TestHelpers.GetNumberOneOrThrowAsync());

        error.Should().BeNull();
        number.Should().Be(1);
    }

    [Fact]
    public async Task TryAsync_Func_ThrowSpecificException_ReturnsDefaultValueAndCorrectException()
    {
        var (error, number) = await ErrorOr.TryAsync(
            () => TestHelpers.GetNumberOneOrThrowAsync(new ArgumentException())
        );

        error.Should().NotBeNull().And.BeOfType<ArgumentException>();
        number.Should().Be(0);
    }

    [Fact]
    public async Task TryAsync_FuncWithCustomException_ThrowSpecificException_ReturnsDefaultValueAndCustomException()
    {
        var (error, number) = await ErrorOr.TryAsync(
            () => TestHelpers.GetNumberOneOrThrowAsync(new ArgumentException()), 
            ex => new InvalidOperationException(ex.Message)
        );

        error.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        number.Should().Be(0);
    }

    [Fact]
    public async Task TryAsync_FuncWithExpectedExceptions_ThrowExpectedException_ReturnsDefaultValueAndThrownException()
    {
        var (error, number) = await ErrorOr.TryAsync(
            () => TestHelpers.GetNumberOneOrThrowAsync(new ArgumentException()), 
            typeof(ArgumentException)
        );

        error.Should().BeOfType<ArgumentException>();
        number.Should().Be(0);
    }

    [Fact]
    public async Task TryAsync_ActionWithExpectedExceptions_ThrowNotExpectedException_ThrowsException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => 
            ErrorOr.TryAsync(() => TestHelpers.GetNumberOneOrThrowAsync(new ArgumentException()), typeof(InvalidOperationException)));
    }
}