using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using OperatorInterface.Core.Domain.Model.Exceptions;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model;

public class ClientSession : Entity<Guid>
{
    public TicketNumber TicketNumber { get; }
    public DateTime AssignmentTime { get; }
    public DateTime? StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public ClientSessionResult? Result { get; private set; }

    [ExcludeFromCodeCoverage]
    private ClientSession() {} // For EF

    public ClientSession(TicketNumber ticketNumber, DateTime assignmentTime)
    {
        if (ticketNumber == null)
            throw new ArgumentNullException(nameof(ticketNumber));
        
        Id = Guid.CreateVersion7();
        
        TicketNumber = ticketNumber;
        AssignmentTime = assignmentTime;
        StartTime = null;
        EndTime = null;
        Result = null;
    }

    public void StartSession()
    {
        if (StartTime != null)
            throw new OperatorDomainException("Session already started");

        StartTime = DateTime.UtcNow;
    }

    public TimeSpan CompleteSession()
    {
        if (StartTime == null)
            throw new OperatorDomainException("Cannot complete session that was not started");
            
        if (EndTime != null)
            throw new OperatorDomainException("Session already completed");

        EndTime = DateTime.UtcNow;
        Result = ClientSessionResult.ServiceCompleted;
        return EndTime.Value - StartTime.Value;
    }

    public void MarkAsNotCame(string reason)
    {
        if (StartTime != null)
            throw new OperatorDomainException("Cannot mark as 'not came' - session already started");

        Result = ClientSessionResult.ClientNotCame;
    }

    public bool IsActive => StartTime != null && EndTime == null;
    public bool IsCompleted => EndTime != null;
    public bool IsAssigned => Result == null;
}