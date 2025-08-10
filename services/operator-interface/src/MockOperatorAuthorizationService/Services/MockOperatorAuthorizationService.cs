using Grpc.Core;
using MockOperatorAuthorizationService.Grpc;
using MockOperatorAuthorizationService.Models;

namespace MockOperatorAuthorizationService.Services;

public class MockOperatorAuthorizationService : OperatorAuthorizationService.OperatorAuthorizationServiceBase
{
    private readonly ILogger<MockOperatorAuthorizationService> _logger;
    
    // Мок данные операторов
    private static readonly Dictionary<string, MockOperator> MockOperators = new()
    {
        {
            "ivanov",
            new MockOperator
            {
                Login = "ivanov",
                Password = "password123",
                FullName = "Иванов Иван Иванович",
                AssignedWorkplace = "WP005",
                WindowCode = "WIN005",
                WindowDisplayName = "Окно 5",
                Location = "1 этаж, центральный зал",
                IsActive = true
            }
        },
        {
            "petrov",
            new MockOperator
            {
                Login = "petrov",
                Password = "password456",
                FullName = "Петров Петр Петрович",
                AssignedWorkplace = "WP007",
                WindowCode = "WIN007",
                WindowDisplayName = "Окно 7",
                Location = "1 этаж, правое крыло",
                IsActive = true
            }
        },
        {
            "sidorov",
            new MockOperator
            {
                Login = "sidorov",
                Password = "password789",
                FullName = "Сидоров Сидор Сидорович",
                AssignedWorkplace = "WP001",
                WindowCode = "WIN001",
                WindowDisplayName = "Касса А",
                Location = "1 этаж, левое крыло",
                IsActive = false // Заблокированный оператор
            }
        }
    };

    // Активные сессии
    private static readonly Dictionary<string, ActiveSession> ActiveSessions = new();

    public MockOperatorAuthorizationService(ILogger<MockOperatorAuthorizationService> logger)
    {
        _logger = logger;
    }

    public override async Task<AuthorizeOperatorResponse> AuthorizeOperator(
        AuthorizeOperatorRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("Authorization attempt for operator: {Login}, workplace: {Workplace}",
            request.OperatorLogin, request.WorkplaceCode);

        await Task.Delay(500); // Имитация задержки

        // Валидация входных данных
        if (string.IsNullOrWhiteSpace(request.OperatorLogin))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Логин не может быть пустым"));
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Пароль не может быть пустым"));
        }

        if (string.IsNullOrWhiteSpace(request.WorkplaceCode))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Код рабочего места не может быть пустым"));
        }

        // Поиск оператора
        if (!MockOperators.TryGetValue(request.OperatorLogin.ToLowerInvariant(), out var mockOperator))
        {
            _logger.LogWarning("Operator not found: {Login}", request.OperatorLogin);
            
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Неверный логин или пароль"));
        }

        // Проверка пароля
        if (mockOperator.Password != request.Password)
        {
            _logger.LogWarning("Invalid password for operator: {Login}", request.OperatorLogin);
            
            var trailers = new Metadata
            {
                { "error-code", "INVALID_CREDENTIALS" },
                { "retry-after", "30" }
            };
            
            throw new RpcException(
                new Status(StatusCode.Unauthenticated, "Неверный логин или пароль"),
                trailers);
        }

        // Проверка активности оператора
        if (!mockOperator.IsActive)
        {
            _logger.LogWarning("Operator is inactive: {Login}", request.OperatorLogin);
            
            var trailers = new Metadata
            {
                { "error-code", "OPERATOR_INACTIVE" },
                { "blocked-since", "2024-07-15T10:30:00Z" },
                { "contact-admin", "admin@company.com" }
            };
            
            throw new RpcException(
                new Status(StatusCode.PermissionDenied, "Учетная запись заблокирована"),
                trailers);
        }

        // Проверка рабочего места
        if (mockOperator.AssignedWorkplace != request.WorkplaceCode.ToUpperInvariant())
        {
            _logger.LogWarning("Workplace mismatch for operator {Login}. Expected: {Expected}, Actual: {Actual}",
                request.OperatorLogin, mockOperator.AssignedWorkplace, request.WorkplaceCode);
            
            var trailers = new Metadata
            {
                { "error-code", "WORKPLACE_MISMATCH" },
                { "operator-name", mockOperator.FullName },
                { "assigned-workplace", mockOperator.AssignedWorkplace },
                { "assigned-window", mockOperator.WindowDisplayName },
                { "current-workplace", request.WorkplaceCode }
            };
            
            throw new RpcException(
                new Status(StatusCode.PermissionDenied, "Оператор назначен в другое рабочее место"),
                trailers);
        }

        // Проверка неизвестного рабочего места
        if (!IsValidWorkplace(request.WorkplaceCode))
        {
            var trailers = new Metadata
            {
                { "error-code", "WORKPLACE_NOT_FOUND" },
                { "requested-workplace", request.WorkplaceCode },
                { "available-workplaces", "WP001,WP002,WP003,WP005,WP007" }
            };
            
            throw new RpcException(
                new Status(StatusCode.NotFound, "Рабочее место не найдено"),
                trailers);
        }

        // Успешная авторизация - создаем сессию
        var sessionId = Guid.NewGuid().ToString();
        ActiveSessions[sessionId] = new ActiveSession
        {
            SessionId = sessionId,
            OperatorLogin = mockOperator.Login,
            CreatedAt = DateTime.UtcNow,
            LastActivity = DateTime.UtcNow
        };

        _logger.LogInformation("Successful authorization for operator: {Login}, session: {SessionId}",
            request.OperatorLogin, sessionId);

        return new AuthorizeOperatorResponse
        {
            SessionId = sessionId,
            OperatorLogin = mockOperator.Login,
            FullName = mockOperator.FullName,
            WindowAssignment = new WindowAssignment
            {
                WindowCode = mockOperator.WindowCode,
                WindowDisplayName = mockOperator.WindowDisplayName,
                Location = mockOperator.Location
            },
            ServiceTopics =
            {
                new ServiceTopic { TopicCode = "PASSPORT_SERVICES", TopicName = "Паспортные услуги" },
                new ServiceTopic { TopicCode = "DOCUMENT_CONSULTATION", TopicName = "Консультации по документам" },
                new ServiceTopic { TopicCode = "CERTIFICATES", TopicName = "Справки и выписки" }
            }
        };
    }

    public override async Task<SessionValidationResponse> IsSessionActive(
        SessionValidationRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("Session validation request for: {SessionId}", request.SessionId);

        await Task.Delay(100); // Имитация задержки

        if (string.IsNullOrWhiteSpace(request.SessionId))
        {
            return new SessionValidationResponse
            {
                IsActive = false,
                Reason = "Invalid session ID"
            };
        }

        if (!ActiveSessions.TryGetValue(request.SessionId, out var session))
        {
            _logger.LogWarning("Session not found: {SessionId}", request.SessionId);
            
            return new SessionValidationResponse
            {
                IsActive = false,
                Reason = "Session not found"
            };
        }

        // Проверяем, не истекла ли сессия (8 часов)
        var sessionAge = DateTime.UtcNow - session.CreatedAt;
        if (sessionAge > TimeSpan.FromHours(8))
        {
            _logger.LogInformation("Session expired: {SessionId}", request.SessionId);
            ActiveSessions.Remove(request.SessionId);
            
            return new SessionValidationResponse
            {
                IsActive = false,
                Reason = "Session expired"
            };
        }

        // Обновляем последнюю активность
        session.LastActivity = DateTime.UtcNow;

        _logger.LogInformation("Session is active: {SessionId}", request.SessionId);
        
        return new SessionValidationResponse
        {
            IsActive = true,
            Reason = "Session is valid"
        };
    }

    private static bool IsValidWorkplace(string workplaceCode)
    {
        var validWorkplaces = new[] { "WP001", "WP002", "WP003", "WP005", "WP007" };
        return validWorkplaces.Contains(workplaceCode.ToUpperInvariant());
    }
}