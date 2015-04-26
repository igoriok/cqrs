using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging
{
    public static class UnitOfWorkExtensions
    {
        public static Task CommitAsync(this IUnitOfWorkAsync unitOfWork)
        {
            return unitOfWork.CommitAsync(CancellationToken.None);
        }

        public static Task RollbackAsync(this IUnitOfWorkAsync unitOfWork)
        {
            return unitOfWork.RollbackAsync(CancellationToken.None);
        }

        public static Task<IUnitOfWorkAsync> CreateAsync(this IUnitOfWorkFactoryAsync factory)
        {
            return factory.CreateAsync(CancellationToken.None);
        }
    }
}