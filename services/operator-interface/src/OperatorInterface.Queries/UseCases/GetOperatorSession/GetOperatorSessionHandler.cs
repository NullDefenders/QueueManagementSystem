using MediatR;
using OperatorInterface.Core.Domain.Model.Exceptions;
using OperatorInterface.Core.Ports;
using OperatorInterface.Queries.UseCases.Shared;

namespace OperatorInterface.Queries.UseCases.GetOperatorSession;

public class GetOperatorSessionHandler : IRequestHandler<GetOperatorSessionQuery, OperatorSessionDto>
{
    private readonly IOperatorSessionRepository _sessionRepository;

    public GetOperatorSessionHandler(IOperatorSessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<OperatorSessionDto> Handle(GetOperatorSessionQuery request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.SessionId);
        if (session == null)
            throw new OperatorDomainException($"Session with ID {request.SessionId.Value} not found");

        return new OperatorSessionDto(
            session.SessionId,
            session.OperatorId,
            session.WorkplaceCode,
            session.Status,
            session.SessionStartTime,
            session.SessionEndTime,
            session.AssignedServices.ToList(),
            session.ClientSessions.Select(cs => new ClientSessionDto(
                cs.TicketNumber.Value,
                cs.AssignmentTime,
                cs.StartTime,
                cs.EndTime,
                cs.Result,
                cs.IsActive,
                cs.IsCompleted
            )).ToList(),
            session.CurrentClientSession != null ? new ClientSessionDto(
                session.CurrentClientSession.TicketNumber.Value,
                session.CurrentClientSession.AssignmentTime,
                session.CurrentClientSession.StartTime,
                session.CurrentClientSession.EndTime,
                session.CurrentClientSession.Result,
                session.CurrentClientSession.IsActive,
                session.CurrentClientSession.IsCompleted
            ) : null
        );
    }
}