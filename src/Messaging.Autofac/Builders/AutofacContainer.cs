using System;
using Autofac;
using IContainer = Irdaf.Messaging.Container.IContainer;

namespace Irdaf.Messaging.Builders
{
    public class AutofacContainer : IContainer
    {
        private readonly ILifetimeScope _scope;

        public ILifetimeScope Scope
        {
            get { return _scope; }
        }

        public AutofacContainer(ILifetimeScope scope)
        {
            _scope = scope.BeginLifetimeScope();
        }

        public object Build(Type handlerType)
        {
            return _scope.Resolve(handlerType);
        }

        public IContainer CreateChildContainer()
        {
            return new AutofacContainer(_scope);
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}