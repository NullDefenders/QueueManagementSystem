/*
using System.Threading.Channels;
using MediatR;

namespace OperatorInterface.BlazorApp.Application;

public class AuthorizeOperatorHandler : IRequestHandler<AuthorizeOperatorCommand, SessionId>
{
    private readonly Dictionary<OperatorId, OperatorInfo> _operators;
    private readonly Dictionary<SessionId, OperatorSessionDto> _sessions;
    private readonly ChannelWriter<DomainEvent> _eventWriter;

    public AuthorizeOperatorHandler(
        Dictionary<OperatorId, OperatorInfo> operators,
        Dictionary<SessionId, OperatorSessionDto> sessions,
        ChannelWriter<DomainEvent> eventWriter)
    {
        _operators = operators;
        _sessions = sessions;
        _eventWriter = eventWriter;

        // Initialize mock operators
        if (!_operators.Any())
        {
            _operators[new OperatorId("OP123")] = new OperatorInfo
            {
                OperatorId = new OperatorId("OP123"),
                Name = "Иван Иванов",
                IsActive = true
            };
            _operators[new OperatorId("DEMO_OP")] = new OperatorInfo
            {
                OperatorId = new OperatorId("DEMO_OP"),
                Name = "Демо Оператор",
                IsActive = true
            };
        }
    }

    public async Task<SessionId> Handle(AuthorizeOperatorCommand request, CancellationToken cancellationToken)
    {
        var operatorId = new OperatorId(request.Login);

        // Mock authorization - check if operator exists
        if (!_operators.ContainsKey(operatorId))
            throw new AuthorizationFailedException("Оператор не найден");

        if (!_operators[operatorId].IsActive)
            throw new AuthorizationFailedException("Оператор неактивен");

        // Simple password check (in real app would be hashed)
        if (request.Password != "password" && request.Password != "demo_password")
            throw new AuthorizationFailedException("Неверный пароль");

        // Create new session
        var sessionId = new SessionId(Guid.NewGuid());
        var session = new OperatorSessionDto
        {
            SessionId = sessionId,
            OperatorId = operatorId,
            WorkplaceCode = request.WorkplaceCode,
            Status = SessionStatus.Authorized,
            CreatedAt = DateTime.UtcNow,
            AssignedServices = new List<ServiceInfo>
            {
                new() { ServiceCode = "SRV001", ServiceName = "Консультация", Description = "Общая консультация граждан" },
                new() { ServiceCode = "SRV002", ServiceName = "Справки", Description = "Выдача справок и документов" }
            }
        };

        _sessions[sessionId] = session;

        // Publish event
        await _eventWriter.WriteAsync(new SessionCreatedEvent(sessionId, operatorId), cancellationToken);

        return sessionId;
    }
}

//===================== Application/Handlers/SessionHandlers.cs =====================

public class StartWorkHandler : IRequestHandler<StartWorkCommand>
{
    private readonly Dictionary<SessionId, OperatorSessionDto> _sessions;
    private readonly ChannelWriter<DomainEvent> _eventWriter;

    public StartWorkHandler(Dictionary<SessionId, OperatorSessionDto> sessions, ChannelWriter<DomainEvent> eventWriter)
    {
        _sessions = sessions;
        _eventWriter = eventWriter;
    }

    public async Task Handle(StartWorkCommand request, CancellationToken cancellationToken)
    {
        if (!_sessions.TryGetValue(request.SessionId, out var session))
            throw new InvalidOperationException("Сессия не найдена");

        if (session.Status != SessionStatus.Authorized)
            throw new InvalidOperationException("Невозможно начать работу из текущего статуса");

        session.Status = SessionStatus.ReadyToWork;
        session.SessionStartTime = DateTime.UtcNow;
    }
}

public class RequestClientHandler : IRequestHandler<RequestClientCommand>
{
    private readonly Dictionary<SessionId, OperatorSessionDto> _sessions;
    private readonly ChannelWriter<DomainEvent> _eventWriter;

    public RequestClientHandler(Dictionary<SessionId, OperatorSessionDto> sessions, ChannelWriter<DomainEvent> eventWriter)
    {
        _sessions = sessions;
        _eventWriter = eventWriter;
    }

    public async Task Handle(RequestClientCommand request, CancellationToken cancellationToken)
    {
        if (!_sessions.TryGetValue(request.SessionId, out var session))
            throw new InvalidOperationException("Сессия не найдена");

        if (session.Status != SessionStatus.ReadyToWork)
            throw new InvalidOperationException("Невозможно запросить клиента из текущего статуса");

        session.Status = SessionStatus.WaitingAssignment;

        // Simulate queue system assigning client after a delay
        _ = Task.Run(async () =>
        {
            await Task.Delay(2000, cancellationToken);

            if (_sessions.TryGetValue(request.SessionId, out var currentSession) &&
                currentSession.Status == SessionStatus.WaitingAssignment)
            {
                var ticketNumber = $"A{DateTime.Now:HHmmss}";
                var clientSession = new ClientSessionDto
                {
                    TicketNumber = ticketNumber,
                    AssignmentTime = DateTime.UtcNow,
                    Result = ClientSessionResult.None
                };

                currentSession.ClientSessions.Add(clientSession);
                currentSession.Status = SessionStatus.WaitingClient;

                await _eventWriter.WriteAsync(new ClientAssignedEvent(request.SessionId, ticketNumber), cancellationToken);
            }
        });
    }
}

public class StartClientSessionHandler : IRequestHandler<StartClientSessionCommand>
{
    private readonly Dictionary<SessionId, OperatorSessionDto> _sessions;
    private readonly ChannelWriter<DomainEvent> _eventWriter;

    public StartClientSessionHandler(Dictionary<SessionId, OperatorSessionDto> sessions, ChannelWriter<DomainEvent> eventWriter)
    {
        _sessions = sessions;
        _eventWriter = eventWriter;
    }

    public async Task Handle(StartClientSessionCommand request, CancellationToken cancellationToken)
    {
        if (!_sessions.TryGetValue(request.SessionId, out var session))
            throw new InvalidOperationException("Сессия не найдена");

        var current = session.CurrentClientSession;
        if (current == null || session.Status != SessionStatus.WaitingClient)
            throw new InvalidOperationException("Нет клиента для начала обслуживания");

        current.ServiceStartTime = DateTime.UtcNow;
        session.Status = SessionStatus.ServingClient;

        // Отправляем событие о начале обслуживания клиента
        await _eventWriter.WriteAsync(
            new ClientServiceStartedEvent(request.SessionId, current.TicketNumber),
            cancellationToken);
    }
}

public class CompleteClientSessionHandler : IRequestHandler<CompleteClientSessionCommand>
{
    private readonly Dictionary<SessionId, OperatorSessionDto> _sessions;
    private readonly ChannelWriter<DomainEvent> _eventWriter;

    public CompleteClientSessionHandler(Dictionary<SessionId, OperatorSessionDto> sessions, ChannelWriter<DomainEvent> eventWriter)
    {
        _sessions = sessions;
        _eventWriter = eventWriter;
    }

    public async Task Handle(CompleteClientSessionCommand request, CancellationToken cancellationToken)
    {
        if (!_sessions.TryGetValue(request.SessionId, out var session))
            throw new InvalidOperationException("Сессия не найдена");

        var current = session.CurrentClientSession;
        if (current == null || session.Status != SessionStatus.ServingClient)
            throw new InvalidOperationException("Нет активного обслуживания для завершения");

        current.ServiceEndTime = DateTime.UtcNow;
        current.Result = ClientSessionResult.ServiceCompleted;
        session.Status = SessionStatus.ReadyToWork;

        // Отправляем событие о завершении обслуживания клиента
        await _eventWriter.WriteAsync(
            new ClientServiceCompletedEvent(request.SessionId, current.TicketNumber, current.ServiceStartTime!.Value, current.ServiceEndTime.Value),
            cancellationToken);
    }
}

public class MarkClientNotCameHandler : IRequestHandler<MarkClientNotCameCommand>
{
    private readonly Dictionary<SessionId, OperatorSessionDto> _sessions;
    private readonly ChannelWriter<DomainEvent> _eventWriter;

    public MarkClientNotCameHandler(Dictionary<SessionId, OperatorSessionDto> sessions, ChannelWriter<DomainEvent> eventWriter)
    {
        _sessions = sessions;
        _eventWriter = eventWriter;
    }

    public async Task Handle(MarkClientNotCameCommand request, CancellationToken cancellationToken)
    {
        if (!_sessions.TryGetValue(request.SessionId, out var session))
            throw new InvalidOperationException("Сессия не найдена");

        var current = session.CurrentClientSession;
        if (current == null || session.Status != SessionStatus.WaitingClient)
            throw new InvalidOperationException("Нет клиента для отметки как не пришедший");

        current.Result = ClientSessionResult.ClientNotCame;
        session.Status = SessionStatus.ReadyToWork;

        // Отправляем событие о том, что клиент не пришел
        await _eventWriter.WriteAsync(
            new ClientNotCameEvent(request.SessionId, current.TicketNumber, request.Reason),
            cancellationToken);
    }
}


public class CloseSessionHandler : IRequestHandler<CloseSessionCommand>
{
    private readonly Dictionary<SessionId, OperatorSessionDto> _sessions;
    private readonly ChannelWriter<DomainEvent> _eventWriter;

    public CloseSessionHandler(Dictionary<SessionId, OperatorSessionDto> sessions, ChannelWriter<DomainEvent> eventWriter)
    {
        _sessions = sessions;
        _eventWriter = eventWriter;
    }

    public async Task Handle(CloseSessionCommand request, CancellationToken cancellationToken)
    {
        if (!_sessions.TryGetValue(request.SessionId, out var session))
            throw new InvalidOperationException("Сессия не найдена");

        var oldStatus = session.Status;
        session.Status = SessionStatus.Closed;
        session.SessionEndTime = DateTime.UtcNow;

        // Отправляем событие о закрытии сессии
        await _eventWriter.WriteAsync(
            new SessionClosedEvent(
                request.SessionId,
                session.OperatorId,
                session.SessionStartTime,
                session.SessionEndTime.Value,
                session.ClientSessions.Count,
                session.ClientSessions.Count(cs => cs.Result == ClientSessionResult.ServiceCompleted),
                session.ClientSessions.Count(cs => cs.Result == ClientSessionResult.ClientNotCame)
            ),
            cancellationToken);
    }
}

public class GetSessionHandler : IRequestHandler<GetSessionQuery, OperatorSessionDto?>
{
    private readonly Dictionary<SessionId, OperatorSessionDto> _sessions;

    public GetSessionHandler(Dictionary<SessionId, OperatorSessionDto> sessions)
    {
        _sessions = sessions;
    }

    public async Task<OperatorSessionDto?> Handle(GetSessionQuery request, CancellationToken cancellationToken)
    {
        return _sessions.TryGetValue(request.SessionId, out var session) ? session : null;
    }
}

public class GetOperatorInfoHandler : IRequestHandler<GetOperatorInfoQuery, OperatorInfo?>
{
    private readonly Dictionary<OperatorId, OperatorInfo> _operators;

    public GetOperatorInfoHandler(Dictionary<OperatorId, OperatorInfo> operators)
    {
        _operators = operators;
    }

    public async Task<OperatorInfo?> Handle(GetOperatorInfoQuery request, CancellationToken cancellationToken)
    {
        return _operators.TryGetValue(request.OperatorId, out var info) ? info : null;
    }
}
*/