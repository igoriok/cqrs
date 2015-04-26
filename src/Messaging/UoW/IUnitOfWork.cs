using System;

namespace Irdaf.Messaging
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        void Rollback();
    }
}