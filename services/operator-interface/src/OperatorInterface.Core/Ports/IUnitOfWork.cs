namespace OperatorInterface.Core.Ports;

public interface IUnitOfWork
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}
