using FluentAssertions;

namespace ErrorOrValue.Tests;

public class TryWithExceptionTests
{
    [Fact]
    public void Try_Action_ThrowSpecificException_ReturnsCorrectException()
    {
        var error = ErrorOr.Try(() => throw new ArgumentException());
        error.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public void Try_Action_NoException_ReturnsNull()
    {
        var error = ErrorOr.Try(() => Console.WriteLine());
        error.Should().BeNull();
    }

    [Fact]
    public void Try_ActionWithCustomException_ThrowSpecificException_ReturnsCustomException()
    {
        var error = ErrorOr.Try(() => throw new ArgumentException(), ex => new InvalidOperationException(ex.Message));
        error.Should().BeOfType<InvalidOperationException>();
    }

    [Fact]
    public void Try_ActionWithExpectedExceptions_ThrowExpectedException_ReturnsException()
    {
        var error = ErrorOr.Try(() => throw new ArgumentException(), typeof(ArgumentException));
        error.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public void Try_ActionWithExpectedExceptions_ThrowNotExpectedException_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => 
            ErrorOr.Try(() => throw new ArgumentException(), typeof(InvalidOperationException)));
    }
}