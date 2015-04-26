using System;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging
{
    public interface IUnitOfWorkAsync : IDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken);

        Task RollbackAsync(CancellationToken cancellationToken);
    }
}