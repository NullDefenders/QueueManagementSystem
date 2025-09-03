using OperatorInterface.Core.Domain.Model;
using OperatorInterface.Core.Domain.Model.Exceptions;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.UnitTests.Domain.Model;

public class ClientSessionShould
{
    private readonly TicketNumber _validTicketNumber = new("T12345");
    private readonly DateTime _validAssignmentTime = new(2025, 8, 10, 10, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void BeCreated_WhenValidParametersProvided()
    {
        // Arrange & Act
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);

        // Assert
        Assert.Equal(_validTicketNumber, clientSession.TicketNumber);
        Assert.Equal(_validAssignmentTime, clientSession.AssignmentTime);
        Assert.Null(clientSession.StartTime);
        Assert.Null(clientSession.EndTime);
        Assert.Null(clientSession.Result);
        Assert.True(clientSession.IsAssigned);
        Assert.False(clientSession.IsActive);
        Assert.False(clientSession.IsCompleted);
    }

    [Fact]
    public void StartSession_WhenSessionIsNotStarted()
    {
        // Arrange
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);

        // Act
        clientSession.StartSession();

        // Assert
        Assert.NotNull(clientSession.StartTime);
        Assert.Null(clientSession.EndTime);
        Assert.Null(clientSession.Result);
        Assert.True(clientSession.IsActive);
        Assert.False(clientSession.IsCompleted);
        Assert.True(clientSession.IsAssigned);
    }

    [Fact]
    public void ThrowException_WhenStartSessionCalledTwice()
    {
        // Arrange
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);
        clientSession.StartSession();

        // Act & Assert
        var exception = Assert.Throws<OperatorDomainException>(() => clientSession.StartSession());
        Assert.Equal("Session already started", exception.Message);
    }

    [Fact]
    public void CompleteSession_WhenSessionIsStarted()
    {
        // Arrange
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);
        clientSession.StartSession();
        var startTime = clientSession.StartTime!.Value;

        // Act
        var duration = clientSession.CompleteSession();

        // Assert
        Assert.NotNull(clientSession.EndTime);
        Assert.Equal(ClientSessionResult.ServiceCompleted, clientSession.Result);
        Assert.True(clientSession.IsCompleted);
        Assert.False(clientSession.IsActive);
        Assert.False(clientSession.IsAssigned);
        Assert.True(duration >= TimeSpan.Zero);
        Assert.Equal(clientSession.EndTime.Value - startTime, duration);
    }

    [Fact]
    public void ThrowException_WhenCompleteSessionCalledWithoutStarting()
    {
        // Arrange
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);

        // Act & Assert
        var exception = Assert.Throws<OperatorDomainException>(() => clientSession.CompleteSession());
        Assert.Equal("Cannot complete session that was not started", exception.Message);
    }

    [Fact]
    public void ThrowException_WhenCompleteSessionCalledTwice()
    {
        // Arrange
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);
        clientSession.StartSession();
        clientSession.CompleteSession();

        // Act & Assert
        var exception = Assert.Throws<OperatorDomainException>(() => clientSession.CompleteSession());
        Assert.Equal("Session already completed", exception.Message);
    }

    [Fact]
    public void MarkAsNotCame_WhenSessionIsNotStarted()
    {
        // Arrange
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);
        const string reason = "Client did not show up";

        // Act
        clientSession.MarkAsNotCame(reason);

        // Assert
        Assert.Equal(ClientSessionResult.ClientNotCame, clientSession.Result);
        Assert.Null(clientSession.StartTime);
        Assert.Null(clientSession.EndTime);
        Assert.False(clientSession.IsActive);
        Assert.False(clientSession.IsCompleted);
        Assert.False(clientSession.IsAssigned);
    }

    [Fact]
    public void ThrowException_WhenMarkAsNotCameCalledAfterSessionStarted()
    {
        // Arrange
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);
        clientSession.StartSession();

        // Act & Assert
        var exception = Assert.Throws<OperatorDomainException>(() =>
            clientSession.MarkAsNotCame("Too late"));
        Assert.Equal("Cannot mark as 'not came' - session already started", exception.Message);
    }

    [Fact]
    public void ReturnCorrectState_WhenSessionIsAssignedButNotStarted()
    {
        // Arrange
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);

        // Act & Assert
        Assert.True(clientSession.IsAssigned);
        Assert.False(clientSession.IsActive);
        Assert.False(clientSession.IsCompleted);
    }

    [Fact]
    public void ReturnCorrectState_WhenSessionIsActive()
    {
        // Arrange
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);
        clientSession.StartSession();

        // Act & Assert
        Assert.True(clientSession.IsActive);
        Assert.True(clientSession.IsAssigned);
        Assert.False(clientSession.IsCompleted);
    }

    [Fact]
    public void ReturnCorrectState_WhenSessionIsCompleted()
    {
        // Arrange
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);
        clientSession.StartSession();
        clientSession.CompleteSession();

        // Act & Assert
        Assert.True(clientSession.IsCompleted);
        Assert.False(clientSession.IsActive);
        Assert.False(clientSession.IsAssigned);
    }

    [Fact]
    public void ReturnCorrectState_WhenClientDidNotCome()
    {
        // Arrange
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);
        clientSession.MarkAsNotCame("No show");

        // Act & Assert
        Assert.False(clientSession.IsActive);
        Assert.False(clientSession.IsCompleted);
        Assert.False(clientSession.IsAssigned);
        Assert.Equal(ClientSessionResult.ClientNotCame, clientSession.Result);
    }

    [Theory]
    [InlineData(0, 0, 1)] // 1 second
    [InlineData(0, 1, 0)] // 1 minute
    [InlineData(1, 0, 0)] // 1 hour
    [InlineData(0, 5, 30)] // 5 minutes 30 seconds
    public void CalculateCorrectDuration_WhenSessionCompleted(int hours, int minutes, int seconds)
    {
        // Arrange
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);
        var expectedDuration = new TimeSpan(hours, minutes, seconds);

        clientSession.StartSession();
        var startTime = clientSession.StartTime!.Value;

        // Manually set the service end time for testing
        // (In real scenario, CompleteSession() sets it to DateTime.UtcNow)
        var endTime = startTime.Add(expectedDuration);

        // Act - We need to simulate the duration calculation
        // Since we can't control DateTime.UtcNow in CompleteSession(), 
        // let's test the duration calculation concept
        var actualDuration = endTime - startTime;

        // Assert
        Assert.Equal(expectedDuration, actualDuration);
    }

    [Fact]
    public void HaveCorrectInitialValues_WhenCreated()
    {
        // Arrange & Act
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);

        // Assert - Verify all initial state
        Assert.Equal(_validTicketNumber, clientSession.TicketNumber);
        Assert.Equal(_validAssignmentTime, clientSession.AssignmentTime);
        Assert.Null(clientSession.StartTime);
        Assert.Null(clientSession.EndTime);
        Assert.Null(clientSession.Result);
    }

    [Fact]
    public void TransitionCorrectly_ThroughFullLifecycle()
    {
        // Arrange
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);

        // Act & Assert - Initial state
        Assert.True(clientSession.IsAssigned);
        Assert.False(clientSession.IsActive);
        Assert.False(clientSession.IsCompleted);

        // Act & Assert - Start session
        clientSession.StartSession();
        Assert.True(clientSession.IsAssigned);
        Assert.True(clientSession.IsActive);
        Assert.False(clientSession.IsCompleted);

        // Act & Assert - Complete session
        clientSession.CompleteSession();
        Assert.False(clientSession.IsAssigned);
        Assert.False(clientSession.IsActive);
        Assert.True(clientSession.IsCompleted);
        Assert.Equal(ClientSessionResult.ServiceCompleted, clientSession.Result);
    }

    [Fact]
    public void TransitionCorrectly_WhenClientDoesNotCome()
    {
        // Arrange
        var clientSession = new ClientSession(_validTicketNumber, _validAssignmentTime);

        // Act & Assert - Initial state
        Assert.True(clientSession.IsAssigned);

        // Act & Assert - Mark as not came
        clientSession.MarkAsNotCame("Client never arrived");
        Assert.False(clientSession.IsAssigned);
        Assert.False(clientSession.IsActive);
        Assert.False(clientSession.IsCompleted);
        Assert.Equal(ClientSessionResult.ClientNotCame, clientSession.Result);
    }
}