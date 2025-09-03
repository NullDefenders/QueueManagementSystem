using MediatR;
using OperatorInterface.Core.Domain.SharedKernel;
using OperatorInterface.Queries.UseCases.Shared;

namespace OperatorInterface.Queries.UseCases.GetOperatorSession;

public record GetOperatorSessionQuery(SessionId SessionId) : IRequest<OperatorSessionDto>;