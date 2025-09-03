using MediatR;
using OperatorInterface.Core.Application.UseCases.Commands.OpenOperatorSession;
using OperatorInterface.Core.Domain.Model.Exceptions;
using OperatorInterface.Core.Ports;

namespace OperatorInterface.Core.Application.UseCases.Commands.StartWork;

public class OpenOperatorSessionHandler : IRequestHandler<OpenOperatorSessionCommand>
{
    private readonly IOperatorSessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OpenOperatorSessionHandler(IOperatorSessionRepository sessionRepository, IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(OpenOperatorSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.SessionId);
        if (session == null)
            throw new SessionNotFoundException(request.SessionId);
        
        session.OpenSession();
        
        await _sessionRepository.UpdateAsync(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
    }
}