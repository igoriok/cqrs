using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging
{
    public interface IUnitOfWorkFactoryAsync
    {
        Task<IUnitOfWorkAsync> CreateAsync(CancellationToken cancellationToken);
    }
}