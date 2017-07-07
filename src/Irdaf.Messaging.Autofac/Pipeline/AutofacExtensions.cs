using System.Reflection;
using Autofac;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Pipeline.Stages;

namespace Irdaf.Messaging.Pipeline
{
    public static class AutofacExtensions
    {
        public static PipelineBuilder UseAutofac(this PipelineBuilder builder, ILifetimeScope container)
        {
            return builder
                .Use(ctx => new AutofacStage(container))
                .From(new AutofacSource());
        }

        public static ContainerBuilder RegisterAssemblyHandlers(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder
                .RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(IQueryHandlerAsync<,>));

            builder
                .RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(ICommandHandlerAsync<>));

            builder
                .RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(IEventHandlerAsync<>));

            return builder;
        }
    }
}