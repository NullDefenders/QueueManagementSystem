using MediatR;
using OperatorInterface.Core.Ports;
using OperatorInterface.Queries.UseCases.GetOperatorSession;
using OperatorInterface.Queries.UseCases.Shared;

namespace OperatorInterface.Queries.UseCases.GetActiveOperatorSession;

public class GetActiveOperatorSessionHandler : IRequestHandler<GetActiveOperatorSessionQuery, OperatorSessionDto?>
{
    private readonly IOperatorSessionRepository _sessionRepository;
    private readonly IMediator _mediator;

    public GetActiveOperatorSessionHandler(
        IOperatorSessionRepository sessionRepository,
        IMediator mediator)
    {
        _sessionRepository = sessionRepository;
        _mediator = mediator;
    }

    public async Task<OperatorSessionDto?> Handle(GetActiveOperatorSessionQuery request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.FindActiveSession(request.OperatorId);
        if (session.HasNoValue)
            return null;

        return await _mediator.Send(new GetOperatorSessionQuery(session.Value), cancellationToken);
    }
}