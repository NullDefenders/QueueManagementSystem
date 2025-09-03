using MediatR;
using OperatorInterface.Core.Domain.Model.Exceptions;
using OperatorInterface.Core.Ports;

namespace OperatorInterface.Core.Application.UseCases.Commands.RequestClient;

public class RequestClientHandler : IRequestHandler<RequestClientCommand>
{
    private readonly IOperatorSessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQueueService _queueService;

    public RequestClientHandler(
        IOperatorSessionRepository sessionRepository,
        IUnitOfWork unitOfWork,
        IQueueService queueService)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
        _queueService = queueService;
    }

    public async Task Handle(RequestClientCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.SessionId);
        if (session == null)
            throw new SessionNotFoundException(request.SessionId);
        
        session.RequestClient();

        await _sessionRepository.UpdateAsync(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // Отправляем запрос в систему очереди
        await _queueService.RequestClientAssignmentAsync(session.OperatorId, session.WorkplaceCode);
    }
}