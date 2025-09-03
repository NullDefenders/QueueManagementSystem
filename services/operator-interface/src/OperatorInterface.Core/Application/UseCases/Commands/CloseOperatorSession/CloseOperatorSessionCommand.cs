using MediatR;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Application.UseCases.Commands.CloseOperatorSession;

public record CloseOperatorSessionCommand(SessionId SessionId) : IRequest;