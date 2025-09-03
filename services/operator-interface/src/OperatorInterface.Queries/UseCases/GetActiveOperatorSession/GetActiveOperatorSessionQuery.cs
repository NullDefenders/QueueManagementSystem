using MediatR;
using OperatorInterface.Core.Domain.SharedKernel;
using OperatorInterface.Queries.UseCases.Shared;

namespace OperatorInterface.Queries.UseCases.GetActiveOperatorSession;

public record GetActiveOperatorSessionQuery(OperatorId OperatorId) : IRequest<OperatorSessionDto?>;