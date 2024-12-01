using FluentAssertions;

namespace ErrorOrValue.Tests;

public class TryAsyncWithExceptionTests
{
    [Fact]
    public async Task TryAsync_Action_ThrowSpecificException_ReturnsCorrectException()
    {
        var error = await ErrorOr.TryAsync(() => throw new ArgumentException());
        error.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public async Task TryAsync_Action_NoException_ReturnsNull()
    {
        var error = await ErrorOr.TryAsync(() => Task.CompletedTask);
        error.Should().BeNull();
    }

    [Fact]
    public async Task TryAsync_ActionWithCustomException_ThrowSpecificException_ReturnsCustomException()
    {
        var error = await ErrorOr.TryAsync(
            () => throw new ArgumentException(), 
            ex => new InvalidOperationException(ex.Message));

        error.Should().BeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task TryAsync_ActionWithExpectedExceptions_ThrowExpectedException_ReturnsException()
    {
        var error = await ErrorOr.TryAsync(() => throw new ArgumentException(), typeof(ArgumentException));
        error.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public async Task TryAsync_ActionWithExpectedExceptions_ThrowNotExpectedException_ThrowsException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => 
            ErrorOr.TryAsync(() => throw new ArgumentException(), typeof(InvalidOperationException)));
    }
}