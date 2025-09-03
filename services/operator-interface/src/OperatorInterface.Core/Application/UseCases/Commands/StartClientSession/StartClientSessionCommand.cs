using MediatR;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Application.UseCases.Commands.StartClientSession;

public record StartClientSessionCommand(SessionId SessionId) : IRequest;