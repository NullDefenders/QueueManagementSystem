using MediatR;
using OperatorInterface.Core.Domain.Model;
using OperatorInterface.Core.Domain.Model.Exceptions;
using OperatorInterface.Core.Domain.SharedKernel;
using OperatorInterface.Core.Ports;

namespace OperatorInterface.Core.Application.UseCases.Commands.AuthorizeOperator;

public class AuthorizeOperatorHandler : IRequestHandler<AuthorizeOperatorCommand, SessionId>
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IOperatorSessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthorizeOperatorHandler(
        IAuthorizationService authorizationService,
        IOperatorSessionRepository sessionRepository,
        IUnitOfWork unitOfWork)
    {
        _authorizationService = authorizationService;
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SessionId> Handle(AuthorizeOperatorCommand command, CancellationToken cancellationToken)
    {
        var request = new AuthRequest(command.Login, command.Password, command.Workplace);

        var response = 
            await _authorizationService.AuthorizeOperatorAsync(request, cancellationToken);
        
        var session = OperatorSession.Create(
            new OperatorId(response.Login),
            new WorkplaceCode(response.Workplace),
            new List<ServiceInfo>()
            {
                new ServiceInfo("PASSPORT", "PASSPORT"),
                new ServiceInfo("BOOK", "BOOK"),
            }
        );

        await _sessionRepository.AddAsync(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return session.SessionId;
    }
}