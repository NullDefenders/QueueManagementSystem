using MediatR;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Application.UseCases.Commands.AuthorizeOperator;

public record AuthorizeOperatorCommand(
    string Login,
    string Password,
    string Workplace
) : IRequest<SessionId>;