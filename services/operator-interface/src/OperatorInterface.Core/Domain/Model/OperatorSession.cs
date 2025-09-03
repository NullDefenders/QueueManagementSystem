using System.Diagnostics.CodeAnalysis;
using OperatorInterface.Core.Domain.Model.DomainEvents;
using OperatorInterface.Core.Domain.Model.Exceptions;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model;

public class OperatorSession : Aggregate<Guid>
{
    public SessionId SessionId => new SessionId(Id);
    public OperatorId OperatorId { get; }
    public WorkplaceCode WorkplaceCode { get; }
    public SessionStatus Status { get; private set; }
    public DateTime? SessionStartTime { get; private set; }
    public DateTime? SessionEndTime { get; private set; }
    public IReadOnlyList<ServiceInfo> AssignedServices { get; private set; }
    public List<ClientSession> ClientSessions { get; private set; }
    public ClientSession? CurrentClientSession => ClientSessions?.SingleOrDefault(x => x.IsAssigned);

    /// <summary>
    /// For EF
    /// </summary>
    [ExcludeFromCodeCoverage]
    private OperatorSession() {}

    private OperatorSession(
        OperatorId operatorId,
        WorkplaceCode workplaceCode,
        List<ServiceInfo> assignedServices)
    {
        Id = Guid.CreateVersion7();

        OperatorId = operatorId;
        WorkplaceCode = workplaceCode;
        AssignedServices = assignedServices.AsReadOnly();
        Status = SessionStatus.Authorized;
        ClientSessions = new();
        
        RaiseDomainEvent(new OperatorSessionCreated(
            SessionId,
            OperatorId,
            WorkplaceCode,
            assignedServices
        ));
    }
    
    public static OperatorSession Create(
        OperatorId operatorId,
        WorkplaceCode workplaceCode,
        List<ServiceInfo> assignedServices)
    {
        if (operatorId == null) throw new ArgumentNullException(nameof(operatorId));
        if (workplaceCode == null) throw new ArgumentNullException(nameof(workplaceCode));
        if (assignedServices == null) throw new ArgumentNullException(nameof(assignedServices));

        return new OperatorSession(operatorId, workplaceCode, assignedServices);
    }

    public void OpenSession()
    {
        if (Status != SessionStatus.Authorized)
            throw new InvalidSessionStateException("StartWork", Status);

        Status = SessionStatus.ReadyToWork;
        SessionStartTime = DateTime.UtcNow;

        RaiseDomainEvent(new OperatorSessionOpened(
            SessionId,
            SessionStartTime.Value
        ));
    }

    public void RequestClient()
    {
        if (Status != SessionStatus.ReadyToWork)
            throw new InvalidSessionStateException("RequestClient", Status);

        Status = SessionStatus.WaitingAssignment;

        RaiseDomainEvent(new ClientRequested(
            SessionId
        ));
    }

    public void AssignClient(TicketNumber ticketNumber)
    {
        if (Status != SessionStatus.WaitingAssignment)
            throw new InvalidSessionStateException("AssignClient", Status);

        if (CurrentClientSession != null)
            throw new OperatorDomainException("Cannot assign new client - current client session is not completed");

        var assignmentTime = DateTime.UtcNow;
        var clientSession = new ClientSession(ticketNumber, assignmentTime);
        ClientSessions.Add(clientSession);

        Status = SessionStatus.WaitingClient;

        RaiseDomainEvent(new ClientAssigned(
            SessionId,
            ticketNumber,
            assignmentTime
        ));
    }

    public void StartClientSession()
    {
        if (Status != SessionStatus.WaitingClient)
            throw new InvalidSessionStateException("StartClientSession", Status);

        if (CurrentClientSession == null)
            throw new OperatorDomainException("No client assigned for session");

        var clientSession = CurrentClientSession;
        clientSession.StartSession();
        Status = SessionStatus.ServingClient;

        RaiseDomainEvent(new ClientSessionStarted(
            SessionId,
            clientSession.TicketNumber,
            clientSession.StartTime!.Value
        ));
    }

    public void CompleteClientSession()
    {
        if (Status != SessionStatus.ServingClient)
            throw new InvalidSessionStateException("CompleteClientSession", Status);

        if (CurrentClientSession == null)
            throw new OperatorDomainException("No active client session to complete");

        var clientSession = CurrentClientSession;
        var serviceDuration = clientSession.CompleteSession();
        Status = SessionStatus.ReadyToWork;

        RaiseDomainEvent(new ClientSessionCompleted(
            SessionId,
            clientSession.TicketNumber,
            clientSession.EndTime!.Value,
            serviceDuration
        ));
    }

    public void MarkClientAsNotCame(string reason = "Client did not come to the window")
    {
        if (Status != SessionStatus.WaitingClient)
            throw new InvalidSessionStateException("MarkClientAsNotCame", Status);

        if (CurrentClientSession == null)
            throw new OperatorDomainException("No client assigned to mark as not came");

        var currentSession = CurrentClientSession;
        currentSession.MarkAsNotCame(reason);
        Status = SessionStatus.ReadyToWork;

        RaiseDomainEvent(new ClientDidNotCome(
            SessionId,
            currentSession.TicketNumber,
            reason
        ));
    }

    public void CloseSession()
    {
        if (Status != SessionStatus.ReadyToWork)
            throw new InvalidSessionStateException("CloseSession", Status);

        Status = SessionStatus.Closed;
        SessionEndTime = DateTime.UtcNow;

        var sessionDuration = SessionEndTime.Value - SessionStartTime!.Value;

        RaiseDomainEvent(new OperatorSessionClosed(
            SessionId,
            SessionEndTime.Value,
            sessionDuration
        ));
    }

    public bool CanCloseSession()
    {
        return Status != SessionStatus.ServingClient;
    }

    public bool IsWorking()
    {
        return Status == SessionStatus.ReadyToWork ||
               Status == SessionStatus.WaitingAssignment ||
               Status == SessionStatus.WaitingClient ||
               Status == SessionStatus.ServingClient;
    }
}