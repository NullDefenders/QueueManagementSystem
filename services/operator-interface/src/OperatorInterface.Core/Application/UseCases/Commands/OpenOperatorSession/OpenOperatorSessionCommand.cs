using MediatR;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Application.UseCases.Commands.OpenOperatorSession;

public record OpenOperatorSessionCommand(SessionId SessionId) : IRequest;

