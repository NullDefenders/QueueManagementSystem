using MediatR;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Application.UseCases.Commands.CompleteClientSession;

public record CompleteClientSessionCommand(SessionId SessionId) : IRequest;