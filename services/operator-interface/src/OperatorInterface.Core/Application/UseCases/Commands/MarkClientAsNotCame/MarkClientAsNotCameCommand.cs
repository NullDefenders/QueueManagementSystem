using MediatR;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Application.UseCases.Commands.MarkClientAsNotCame;

public record MarkClientAsNotCameCommand(SessionId SessionId, string Reason = "Client did not come to the window") : IRequest;