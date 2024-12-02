using FluentAssertions;

namespace ErrorOrValue.Tests;

public class TryWithErrorOrResultTests
{
    [Fact]
    public void Try_Func_NoException_ReturnsValueAndNoException()
    {
        var (error, number) = ErrorOr.Try(() => TestHelpers.GetNumberOneOrThrow());

        error.Should().BeNull();
        number.Should().Be(1);
    }

    [Fact]
    public void Try_Func_ThrowSpecificException_ReturnsDefaultValueAndCorrectException()
    {
        var (error, number) = ErrorOr.Try(
            () => TestHelpers.GetNumberOneOrThrow(new ArgumentException())
        );

        error.Should().NotBeNull().And.BeOfType<ArgumentException>();
        number.Should().Be(0);
    }

    [Fact]
    public void Try_FuncWithCustomException_ThrowSpecificException_ReturnsDefaultValueAndCustomException()
    {
        var (error, number) = ErrorOr.Try(
            () => TestHelpers.GetNumberOneOrThrow(new ArgumentException()), 
            ex => new InvalidOperationException(ex.Message)
        );

        error.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        number.Should().Be(0);
    }

    [Fact]
    public void Try_FuncWithExpectedExceptions_ThrowExpectedException_ReturnsDefaultValueAndThrownException()
    {
        var (error, number) = ErrorOr.Try(
            () => TestHelpers.GetNumberOneOrThrow(new ArgumentException()), 
            typeof(ArgumentException)
        );

        error.Should().BeOfType<ArgumentException>();
        number.Should().Be(0);
    }

    [Fact]
    public void Try_ActionWithExpectedExceptions_ThrowNotExpectedException_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => 
            ErrorOr.Try(() => TestHelpers.GetNumberOneOrThrow(new ArgumentException()), typeof(InvalidOperationException)));
    }
}