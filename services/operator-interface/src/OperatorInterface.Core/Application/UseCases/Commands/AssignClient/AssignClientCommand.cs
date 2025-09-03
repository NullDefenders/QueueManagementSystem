using MediatR;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Application.UseCases.Commands.AssignClient;

public record AssignClientCommand(SessionId SessionId, string TicketNumber) : IRequest;