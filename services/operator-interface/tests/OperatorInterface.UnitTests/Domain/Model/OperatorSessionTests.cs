using OperatorInterface.Core.Domain.Model;
using OperatorInterface.Core.Domain.Model.DomainEvents;
using OperatorInterface.Core.Domain.Model.Exceptions;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.UnitTests.Domain.Model;

public class OperatorSessionShould
{
    private readonly OperatorId _validOperatorId = new("OP123");
    private readonly WorkplaceCode _validWorkplaceCode = new("WP001");
    private readonly TicketNumber _validTicketNumber = new("T12345");
    private readonly List<ServiceInfo> _validAssignedServices = new()
    {
        new ServiceInfo("S001", "Passport Service"),
        new ServiceInfo("S002", "Visa Service")
    };

    [Fact]
    public void BeCreatedAuthorized_WhenValidParametersProvided()
    {
        // Act
        var session = OperatorSession.Create(
            _validOperatorId,
            _validWorkplaceCode,
            _validAssignedServices);

        // Assert
        Assert.Equal(_validOperatorId, session.OperatorId);
        Assert.Equal(_validWorkplaceCode, session.WorkplaceCode);
        Assert.Equal(_validAssignedServices, session.AssignedServices);
        Assert.Equal(SessionStatus.Authorized, session.Status);
        Assert.Null(session.SessionStartTime);
        Assert.Null(session.SessionEndTime);
        Assert.Null(session.CurrentClientSession);
        Assert.Empty(session.ClientSessions);
        Assert.Single(session.GetDomainEvents());
        Assert.IsType<OperatorAuthorized>(session.GetDomainEvents().First());
    }

    [Fact]
    public void ThrowException_WhenCreatedWithNullOperatorId()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            OperatorSession.Create(null!, _validWorkplaceCode, _validAssignedServices));
    }

    [Fact]
    public void ThrowException_WhenCreatedWithNullWorkplaceCode()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            OperatorSession.Create(_validOperatorId, null!, _validAssignedServices));
    }

    [Fact]
    public void ThrowException_WhenCreatedWithNullAssignedServices()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            OperatorSession.Create(_validOperatorId, _validWorkplaceCode, null!));
    }

    [Fact]
    public void StartWork_WhenStatusIsAuthorized()
    {
        // Arrange
        var session = CreateAuthorizedSession();

        // Act
        session.StartWork();

        // Assert
        Assert.Equal(SessionStatus.ReadyToWork, session.Status);
        Assert.NotNull(session.SessionStartTime);
        Assert.Contains(session.GetDomainEvents(), e => e is WorkSessionStarted);
    }

    [Fact]
    public void ThrowException_WhenStartWorkCalledFromInvalidStatus()
    {
        // Arrange
        var session = CreateAuthorizedSession();
        session.StartWork(); // Move to ReadyToWork

        // Act & Assert
        var exception = Assert.Throws<InvalidSessionStateException>(() => session.StartWork());
        Assert.Contains("StartWork", exception.Message);
        Assert.Contains("ReadyToWork", exception.Message);
    }

    [Fact]
    public void RequestClient_WhenStatusIsReadyToWork()
    {
        // Arrange
        var session = CreateReadyToWorkSession();

        // Act
        session.RequestClient();

        // Assert
        Assert.Equal(SessionStatus.WaitingAssignment, session.Status);
        Assert.Contains(session.GetDomainEvents(), e => e is ClientRequested);
    }

    [Fact]
    public void ThrowException_WhenRequestClientCalledFromInvalidStatus()
    {
        // Arrange
        var session = CreateAuthorizedSession(); // Not ReadyToWork

        // Act & Assert
        var exception = Assert.Throws<InvalidSessionStateException>(() => session.RequestClient());
        Assert.Contains("RequestClient", exception.Message);
        Assert.Contains("Authorized", exception.Message);
    }

    [Fact]
    public void AssignClient_WhenStatusIsWaitingAssignment()
    {
        // Arrange
        var session = CreateWaitingAssignmentSession();

        // Act
        session.AssignClient(_validTicketNumber);

        // Assert
        Assert.Equal(SessionStatus.WaitingClient, session.Status);
        Assert.NotNull(session.CurrentClientSession);
        Assert.Equal(_validTicketNumber, session.CurrentClientSession.TicketNumber);
        Assert.Single(session.ClientSessions);
        Assert.Contains(session.GetDomainEvents(), e => e is ClientAssigned);
    }

    [Fact]
    public void ThrowException_WhenAssignClientCalledFromInvalidStatus()
    {
        // Arrange
        var session = CreateReadyToWorkSession(); // Not WaitingAssignment

        // Act & Assert
        var exception = Assert.Throws<InvalidSessionStateException>(() => session.AssignClient(_validTicketNumber));
        Assert.Contains("AssignClient", exception.Message);
    }

    [Fact]
    public void ThrowException_WhenAssignClientCalledWithExistingClient()
    {
        // Arrange
        var session = CreateReadyToWorkSession(); // Not WaitingAssignment

        // Act & Assert
        var exception = Assert.Throws<InvalidSessionStateException>(() => session.AssignClient(_validTicketNumber));
        Assert.Contains("AssignClient", exception.Message);
    }

    [Fact]
    public void StartClientSession_WhenStatusIsWaitingClient()
    {
        // Arrange
        var session = CreateWaitingClientSession();

        // Act
        session.StartClientSession();

        // Assert
        Assert.Equal(SessionStatus.ServingClient, session.Status);
        Assert.NotNull(session.CurrentClientSession);
        Assert.True(session.CurrentClientSession.IsActive);
        Assert.Contains(session.GetDomainEvents(), e => e is ClientSessionStarted);
    }

    [Fact]
    public void ThrowException_WhenStartClientSessionCalledFromInvalidStatus()
    {
        // Arrange
        var session = CreateReadyToWorkSession(); // Not WaitingClient

        // Act & Assert
        var exception = Assert.Throws<InvalidSessionStateException>(() => session.StartClientSession());
        Assert.Contains("StartClientSession", exception.Message);
    }

    [Fact]
    public void ThrowException_WhenStartClientSessionCalledWithoutAssignedClient()
    {
        // Arrange
        var session = CreateWaitingAssignmentSession();
        // Don't assign client, but somehow get to WaitingClient status
        session.GetType().GetProperty("Status")!.SetValue(session, SessionStatus.WaitingClient);

        // Act & Assert
        var exception = Assert.Throws<OperatorDomainException>(() => session.StartClientSession());
        Assert.Contains("No client assigned", exception.Message);
    }

    [Fact]
    public void CompleteClientSession_WhenStatusIsServingClient()
    {
        // Arrange
        var session = CreateServingClientSession();

        // Act
        session.CompleteClientSession();

        // Assert
        Assert.Equal(SessionStatus.ReadyToWork, session.Status);
        Assert.Null(session.CurrentClientSession);
        Assert.Single(session.ClientSessions);
        Assert.True(session.ClientSessions.First().IsCompleted);
        Assert.Contains(session.GetDomainEvents(), e => e is ClientSessionCompleted);
    }

    [Fact]
    public void ThrowException_WhenCompleteClientSessionCalledFromInvalidStatus()
    {
        // Arrange
        var session = CreateWaitingClientSession(); // Not ServingClient

        // Act & Assert
        var exception = Assert.Throws<InvalidSessionStateException>(() => session.CompleteClientSession());
        Assert.Contains("CompleteClientSession", exception.Message);
    }

    [Fact]
    public void MarkClientAsNotCame_WhenStatusIsWaitingClient()
    {
        // Arrange
        var session = CreateWaitingClientSession();
        const string reason = "Client did not show up";

        // Act
        session.MarkClientAsNotCame(reason);

        // Assert
        Assert.Equal(SessionStatus.ReadyToWork, session.Status);
        Assert.Null(session.CurrentClientSession);
        Assert.Single(session.ClientSessions);
        Assert.Equal(ClientSessionResult.ClientNotCame, session.ClientSessions.First().Result);
        Assert.Contains(session.GetDomainEvents(), e => e is ClientDidNotCome);
    }

    [Fact]
    public void ThrowException_WhenMarkClientAsNotCameCalledFromInvalidStatus()
    {
        // Arrange
        var session = CreateServingClientSession(); // Not WaitingClient

        // Act & Assert
        var exception = Assert.Throws<InvalidSessionStateException>(() => 
            session.MarkClientAsNotCame("reason"));
        Assert.Contains("MarkClientAsNotCame", exception.Message);
    }

    [Fact]
    public void CloseSession_WhenStatusIsNotServingClient()
    {
        // Arrange
        var session = CreateReadyToWorkSession();

        // Act
        session.CloseSession();

        // Assert
        Assert.Equal(SessionStatus.Closed, session.Status);
        Assert.NotNull(session.SessionEndTime);
        Assert.Contains(session.GetDomainEvents(), e => e is WorkSessionClosed);
    }

    [Fact]
    public void ThrowException_WhenCloseSessionCalledWhileServingClient()
    {
        // Arrange
        var session = CreateServingClientSession();

        // Act & Assert
        var exception = Assert.Throws<InvalidSessionStateException>(() => session.CloseSession());
        Assert.Contains("CloseSession", exception.Message);
        Assert.Contains("ServingClient", exception.Message);
    }

    [Fact]
    public void ReturnFalse_WhenCanCloseSessionWhileServingClient()
    {
        // Arrange
        var session = CreateServingClientSession();

        // Act & Assert
        Assert.False(session.CanCloseSession());
    }

    [Fact]
    public void ReturnTrue_WhenCanCloseSessionWhileNotServingClient()
    {
        // Arrange
        var session = CreateReadyToWorkSession();

        // Act & Assert
        Assert.True(session.CanCloseSession());
    }

    [Theory]
    [InlineData(SessionStatus.ReadyToWork, true)]
    [InlineData(SessionStatus.WaitingAssignment, true)]
    [InlineData(SessionStatus.WaitingClient, true)]
    [InlineData(SessionStatus.ServingClient, true)]
    [InlineData(SessionStatus.Authorized, false)]
    [InlineData(SessionStatus.Closed, false)]
    public void ReturnCorrectIsWorking_ForDifferentStatuses(SessionStatus status, bool expectedIsWorking)
    {
        // Arrange
        var session = CreateAuthorizedSession();
        session.GetType().GetProperty("Status")!.SetValue(session, status);

        // Act & Assert
        Assert.Equal(expectedIsWorking, session.IsWorking());
    }

    [Fact]
    public void HandleMultipleClients_ThroughoutSession()
    {
        // Arrange
        var session = CreateReadyToWorkSession();
        var firstTicket = new TicketNumber("T001");
        var secondTicket = new TicketNumber("T002");

        // Act & Assert - First client
        session.RequestClient();
        session.AssignClient(firstTicket);
        session.StartClientSession();
        session.CompleteClientSession();

        Assert.Equal(SessionStatus.ReadyToWork, session.Status);
        Assert.Null(session.CurrentClientSession);
        Assert.Single(session.ClientSessions);

        // Act & Assert - Second client
        session.RequestClient();
        session.AssignClient(secondTicket);
        session.MarkClientAsNotCame("Client did not come");

        Assert.Equal(SessionStatus.ReadyToWork, session.Status);
        Assert.Null(session.CurrentClientSession);
        Assert.Equal(2, session.ClientSessions.Count);
        Assert.Equal(ClientSessionResult.ServiceCompleted, session.ClientSessions.First().Result);
        Assert.Equal(ClientSessionResult.ClientNotCame, session.ClientSessions.Last().Result);
    }

    [Fact]
    public void GenerateCorrectDomainEvents_ThroughFullWorkflow()
    {
        // Arrange - Create session WITHOUT clearing events
        var session = OperatorSession.Create(
            _validOperatorId,
            _validWorkplaceCode,
            _validAssignedServices);

        // Act - Complete workflow
        session.StartWork();
        session.RequestClient();
        session.AssignClient(_validTicketNumber);
        session.StartClientSession();
        session.CompleteClientSession();
        session.CloseSession();

        // Assert
        var events = session.GetDomainEvents();
        Assert.Contains(events, e => e is OperatorAuthorized);
        Assert.Contains(events, e => e is WorkSessionStarted);
        Assert.Contains(events, e => e is ClientRequested);
        Assert.Contains(events, e => e is ClientAssigned);
        Assert.Contains(events, e => e is ClientSessionStarted);
        Assert.Contains(events, e => e is ClientSessionCompleted);
        Assert.Contains(events, e => e is WorkSessionClosed);
    }

    [Fact]
    public void ClearDomainEvents_WhenMethodCalled()
    {
        // Arrange
        var session = CreateAuthorizedSession();
        session.StartWork(); // Generate some events

        // Act
        session.ClearDomainEvents();

        // Assert
        Assert.Empty(session.GetDomainEvents());
    }

    [Fact]
    public void MaintainReadOnlyCollections_ForClientSessionsAndDomainEvents()
    {
        // Arrange
        var session = CreateWaitingClientSession();

        // Act & Assert
        Assert.IsAssignableFrom<IReadOnlyList<ClientSession>>(session.ClientSessions);
        Assert.IsAssignableFrom<IReadOnlyList<DomainEvent>>(session.GetDomainEvents());
        Assert.IsAssignableFrom<IReadOnlyList<ServiceInfo>>(session.AssignedServices);
    }

    [Fact]
    public void TransitionCorrectly_ThroughStatusWorkflow()
    {
        // Arrange
        var session = CreateAuthorizedSession();

        // Act & Assert - Authorized → ReadyToWork
        Assert.Equal(SessionStatus.Authorized, session.Status);
        session.StartWork();
        Assert.Equal(SessionStatus.ReadyToWork, session.Status);

        // Act & Assert - ReadyToWork → WaitingAssignment
        session.RequestClient();
        Assert.Equal(SessionStatus.WaitingAssignment, session.Status);

        // Act & Assert - WaitingAssignment → WaitingClient
        session.AssignClient(_validTicketNumber);
        Assert.Equal(SessionStatus.WaitingClient, session.Status);

        // Act & Assert - WaitingClient → ServingClient
        session.StartClientSession();
        Assert.Equal(SessionStatus.ServingClient, session.Status);

        // Act & Assert - ServingClient → ReadyToWork
        session.CompleteClientSession();
        Assert.Equal(SessionStatus.ReadyToWork, session.Status);

        // Act & Assert - ReadyToWork → Closed
        session.CloseSession();
        Assert.Equal(SessionStatus.Closed, session.Status);
    }

    [Fact]
    public void TransitionCorrectly_WhenClientDoesNotCome()
    {
        // Arrange
        var session = CreateWaitingClientSession();

        // Act
        session.MarkClientAsNotCame("No show");

        // Assert
        Assert.Equal(SessionStatus.ReadyToWork, session.Status);
        Assert.Null(session.CurrentClientSession);
    }

    // Helper methods for test setup
    private OperatorSession CreateAuthorizedSession()
    {
        var session = OperatorSession.Create(
            _validOperatorId,
            _validWorkplaceCode,
            _validAssignedServices);
        session.ClearDomainEvents(); // Clear creation events for cleaner testing
        return session;
    }

    private OperatorSession CreateReadyToWorkSession()
    {
        var session = CreateAuthorizedSession();
        session.StartWork();
        session.ClearDomainEvents();
        return session;
    }

    private OperatorSession CreateWaitingAssignmentSession()
    {
        var session = CreateReadyToWorkSession();
        session.RequestClient();
        session.ClearDomainEvents();
        return session;
    }

    private OperatorSession CreateWaitingClientSession()
    {
        var session = CreateWaitingAssignmentSession();
        session.AssignClient(_validTicketNumber);
        session.ClearDomainEvents();
        return session;
    }

    private OperatorSession CreateServingClientSession()
    {
        var session = CreateWaitingClientSession();
        session.StartClientSession();
        session.ClearDomainEvents();
        return session;
    }
}