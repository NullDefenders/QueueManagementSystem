using OperatorInterface.Core.Domain.Model;
using OperatorInterface.Core.Domain.Model.Exceptions;
using OperatorInterface.Core.Domain.SharedKernel;
using OperatorInterface.Core.Ports;

namespace OperatorInterface.Core.Application.UseCases;

public class OperatorSessionApplicationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOperatorSessionRepository _sessionRepository;
    private readonly IQueueService _queueService;

    public OperatorSessionApplicationService(
        IUnitOfWork unitOfWork,
        IOperatorSessionRepository sessionRepository,
        IQueueService queueService)
    {
        _unitOfWork = unitOfWork;
        _sessionRepository = sessionRepository;
        _queueService = queueService;
    }
/*
    public async Task<SessionId> CreateSessionAsync(
        OperatorId operatorId,
        WorkplaceCode workplaceCode,
        List<ServiceInfo> assignedServices)
    {
        // Проверяем, нет ли уже активной сессии
        var existingSession = await _sessionRepository
            .GetActiveSessionByOperatorAsync(operatorId);

        if (existingSession != null)
            throw new OperatorDomainException("Operator already has an active session");

        // Создаем новую сессию
        var session = OperatorSession.Create(
            operatorId,
            workplaceCode,
            assignedServices
        );

        await _sessionRepository.AddAsync(session);
        await _unitOfWork.SaveChangesAsync();
        return session.SessionId;
    }
*/
    public async Task StartWorkAsync(SessionId sessionId)
    {
        var session = await GetSessionAsync(sessionId);
        session.StartWork();
        await _sessionRepository.UpdateAsync(session);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RequestClientAsync(SessionId sessionId)
    {
        var session = await GetSessionAsync(sessionId);
        session.RequestClient();

        // Отправляем запрос в систему очереди
        await _queueService.RequestClientAssignmentAsync(
            session.OperatorId,
            session.WorkplaceCode);

        await _sessionRepository.UpdateAsync(session);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AssignClientAsync(SessionId sessionId, TicketNumber ticketNumber)
    {
        var session = await GetSessionAsync(sessionId);
        session.AssignClient(ticketNumber);
        await _sessionRepository.UpdateAsync(session);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task StartClientSessionAsync(SessionId sessionId)
    {
        var session = await GetSessionAsync(sessionId);
        session.StartClientSession();
        await _sessionRepository.UpdateAsync(session);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CompleteClientSessionAsync(SessionId sessionId)
    {
        var session = await GetSessionAsync(sessionId);
        session.CompleteClientSession();
        await _sessionRepository.UpdateAsync(session);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task MarkClientAsNotCameAsync(SessionId sessionId, string reason)
    {
        var session = await GetSessionAsync(sessionId);
        session.MarkClientAsNotCame(reason);
        await _sessionRepository.UpdateAsync(session);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CloseSessionAsync(SessionId sessionId)
    {
        var session = await GetSessionAsync(sessionId);
        session.CloseSession();
        await _sessionRepository.UpdateAsync(session);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<OperatorSession> GetSessionAsync(SessionId sessionId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId);
        if (session == null)
            throw new OperatorDomainException($"Session with ID {sessionId.Value} not found");
        return session;
    }
}