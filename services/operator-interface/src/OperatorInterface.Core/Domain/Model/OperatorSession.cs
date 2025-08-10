using System.Diagnostics.CodeAnalysis;
using OperatorInterface.Core.Domain.Model.DomainEvents;
using OperatorInterface.Core.Domain.Model.Exceptions;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model;

public class OperatorSession : Aggregate<Guid>
{
    private ClientSession? _currentClientSession;

    public SessionId SessionId => new SessionId(Id);
    public OperatorId OperatorId { get; }
    public WorkplaceCode WorkplaceCode { get; }
    public SessionStatus Status { get; private set; }
    public DateTime? SessionStartTime { get; private set; }
    public DateTime? SessionEndTime { get; private set; }
    public IReadOnlyList<ServiceInfo> AssignedServices { get; private set; }
    public List<ClientSession> ClientSessions { get; private set; } 
    public ClientSession? CurrentClientSession => _currentClientSession;

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
        
        RaiseDomainEvent(new OperatorAuthorized(
            DateTime.UtcNow,
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

    public void StartWork()
    {
        if (Status != SessionStatus.Authorized)
            throw new InvalidSessionStateException("StartWork", Status);

        Status = SessionStatus.ReadyToWork;
        SessionStartTime = DateTime.UtcNow;

        RaiseDomainEvent(new WorkSessionStarted(
            DateTime.UtcNow,
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
            DateTime.UtcNow,
            SessionId
        ));
    }

    public void AssignClient(TicketNumber ticketNumber)
    {
        if (Status != SessionStatus.WaitingAssignment)
            throw new InvalidSessionStateException("AssignClient", Status);
/*
        if (_currentClientSession != null)
            throw new OperatorDomainException("Cannot assign new client - current client session is not completed");
*/
        var assignmentTime = DateTime.UtcNow;
        var clientSession = new ClientSession(ticketNumber, assignmentTime);
        ClientSessions.Add(clientSession);
        //_currentClientSession = clientSession;

        Status = SessionStatus.WaitingClient;

        RaiseDomainEvent(new ClientAssigned(
            DateTime.UtcNow,
            SessionId,
            ticketNumber,
            assignmentTime
        ));
    }

    public void StartClientSession()
    {
        if (Status != SessionStatus.WaitingClient)
            throw new InvalidSessionStateException("StartClientSession", Status);

        if (_currentClientSession == null)
            throw new OperatorDomainException("No client assigned for session");

        _currentClientSession.StartSession();
        Status = SessionStatus.ServingClient;

        RaiseDomainEvent(new ClientSessionStarted(
            DateTime.UtcNow,
            SessionId,
            _currentClientSession.TicketNumber,
            _currentClientSession.StartTime!.Value
        ));
    }

    public void CompleteClientSession()
    {
        if (Status != SessionStatus.ServingClient)
            throw new InvalidSessionStateException("CompleteClientSession", Status);

        if (_currentClientSession == null)
            throw new OperatorDomainException("No active client session to complete");

        var serviceDuration = _currentClientSession.CompleteSession();
        Status = SessionStatus.ReadyToWork;

        RaiseDomainEvent(new ClientSessionCompleted(
            DateTime.UtcNow,
            SessionId,
            _currentClientSession.TicketNumber,
            _currentClientSession.EndTime!.Value,
            serviceDuration
        ));

        _currentClientSession = null; // Завершили обслуживание
    }

    public void MarkClientAsNotCame(string reason = "Client did not come to the window")
    {
        if (Status != SessionStatus.WaitingClient)
            throw new InvalidSessionStateException("MarkClientAsNotCame", Status);

        if (_currentClientSession == null)
            throw new OperatorDomainException("No client assigned to mark as not came");

        _currentClientSession.MarkAsNotCame(reason);
        Status = SessionStatus.ReadyToWork;

        RaiseDomainEvent(new ClientDidNotCome(
            DateTime.UtcNow,
            SessionId,
            _currentClientSession.TicketNumber,
            reason
        ));

        _currentClientSession = null; // Клиент не пришел
    }

    public void CloseSession()
    {
        if (Status == SessionStatus.ServingClient)
            throw new InvalidSessionStateException("CloseSession", Status);

        Status = SessionStatus.Closed;
        SessionEndTime = DateTime.UtcNow;

        var sessionDuration = SessionEndTime.Value - SessionStartTime!.Value;

        RaiseDomainEvent(new WorkSessionClosed(
            DateTime.UtcNow,
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