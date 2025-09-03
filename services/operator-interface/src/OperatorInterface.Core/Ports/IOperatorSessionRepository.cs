using CSharpFunctionalExtensions;
using OperatorInterface.Core.Domain.Model;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Ports;

public interface IOperatorSessionRepository
{
    Task<OperatorSession?> GetByIdAsync(SessionId sessionId);
    
    Task AddAsync(OperatorSession session);
    Task UpdateAsync(OperatorSession session);
    
    Task<Maybe<SessionId>> FindActiveSession(OperatorId operatorId);
}