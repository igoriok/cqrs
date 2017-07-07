using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Pipeline.Stages;

namespace Irdaf.Messaging.Pipeline
{
    public class DefaultPipeline : IPipeline
    {
        private readonly IEnumerable<Func<IPipelineContext, IStage>> _pipeline;

        public DefaultPipeline(IEnumerable<Func<IPipelineContext, IStage>> pipeline)
        {
            _pipeline = pipeline;
        }

        public void Execute(IPipelineContext context)
        {
            using (var dispatcher = new Dispatcher(_pipeline, context, CancellationToken.None))
            {
                dispatcher.Dispatch();
            }
        }

        public Task ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            using (var dispatcher = new Dispatcher(_pipeline, context, cancellationToken))
            {
                return dispatcher.DispatchAsync();
            }
        }

        private class Dispatcher : IDisposable
        {
            private readonly IPipelineContext _context;
            private readonly CancellationToken _cancellationToken;
            private readonly IEnumerator<Func<IPipelineContext, IStage>> _enumerator;

            public Dispatcher(IEnumerable<Func<IPipelineContext, IStage>> pipeline, IPipelineContext context, CancellationToken cancellationToken)
            {
                _context = context;
                _cancellationToken = cancellationToken;
                _enumerator = pipeline.GetEnumerator();
            }

            public void Dispatch()
            {
                if (_enumerator.MoveNext())
                {
                    var stage = _enumerator.Current(_context);
                    if (stage != null)
                    {
                        try
                        {
                            stage.Execute(_context, Dispatch);
                        }
                        finally 
                        {
                            (stage as IDisposable)?.Dispose();
                        }
                    }
                }
            }

            public async Task DispatchAsync()
            {
                if (_enumerator.MoveNext())
                {
                    _cancellationToken.ThrowIfCancellationRequested();

                    var stage = _enumerator.Current(_context);
                    if (stage != null)
                    {
                        try
                        {
                            await stage.ExecuteAsync(_context, DispatchAsync, _cancellationToken);
                        }
                        finally
                        {
                            (stage as IDisposable)?.Dispose();
                        }
                    }
                }
            }

            public void Dispose()
            {
                _enumerator.Dispose();
            }
        }
    }
}