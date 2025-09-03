using MediatR;
using OperatorInterface.Core.Domain.Model.Exceptions;
using OperatorInterface.Core.Ports;

namespace OperatorInterface.Core.Application.UseCases.Commands.StartClientSession;

public class StartClientSessionHandler : IRequestHandler<StartClientSessionCommand>
{
    private readonly IOperatorSessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartClientSessionHandler(IOperatorSessionRepository sessionRepository, IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(StartClientSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.SessionId);
        if (session == null)
            throw new SessionNotFoundException(request.SessionId);
        
        session.StartClientSession();
        await _sessionRepository.UpdateAsync(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}