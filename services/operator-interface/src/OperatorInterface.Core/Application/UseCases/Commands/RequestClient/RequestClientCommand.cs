using MediatR;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Application.UseCases.Commands.RequestClient;

public record RequestClientCommand(SessionId SessionId) : IRequest;