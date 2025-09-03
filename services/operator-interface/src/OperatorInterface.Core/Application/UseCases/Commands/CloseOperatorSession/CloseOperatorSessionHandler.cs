using MediatR;
using OperatorInterface.Core.Domain.Model.Exceptions;
using OperatorInterface.Core.Ports;

namespace OperatorInterface.Core.Application.UseCases.Commands.CloseOperatorSession;

public class CloseOperatorSessionHandler : IRequestHandler<CloseOperatorSessionCommand>
{
    private readonly IOperatorSessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CloseOperatorSessionHandler(IOperatorSessionRepository sessionRepository, IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CloseOperatorSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.SessionId);
        if (session == null)
            throw new SessionNotFoundException(request.SessionId);
        
        session.CloseSession();
        
        await _sessionRepository.UpdateAsync(session);
        await  _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}