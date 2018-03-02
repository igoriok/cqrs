using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Pipeline.Stages;

namespace Irdaf.Messaging.Pipeline
{
    public class DefaultPipeline : IPipeline
    {
        private readonly IEnumerable<Func<IPipelineContext, object>> _pipeline;

        public DefaultPipeline(IEnumerable<Func<IPipelineContext, object>> pipeline)
        {
            _pipeline = pipeline;
        }

        public void Execute(IPipelineContext context)
        {
            using (var dispatcher = new Dispatcher(_pipeline, context))
            {
                dispatcher.Dispatch();
            }
        }

        public Task ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            using (var dispatcher = new DispatcherAsync(_pipeline, context, cancellationToken))
            {
                return dispatcher.DispatchAsync();
            }
        }

        private class Dispatcher : IDisposable
        {
            private readonly IPipelineContext _context;
            private readonly IEnumerator<Func<IPipelineContext, object>> _enumerator;

            public Dispatcher(IEnumerable<Func<IPipelineContext, object>> pipeline, IPipelineContext context)
            {
                _context = context;
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
                            if (stage is IStage syncStage)
                            {
                                syncStage.Execute(_context, Dispatch);
                            }
                            else if (stage is IStageAsync asyncStage)
                            {
                                asyncStage.ExecuteAsync(_context, () => { Dispatch(); return Task.CompletedTask; }, CancellationToken.None).GetAwaiter().GetResult();
                            }
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

        private class DispatcherAsync : IDisposable
        {
            private readonly IPipelineContext _context;
            private readonly CancellationToken _cancellationToken;
            private readonly IEnumerator<Func<IPipelineContext, object>> _enumerator;

            public DispatcherAsync(IEnumerable<Func<IPipelineContext, object>> pipeline, IPipelineContext context, CancellationToken cancellationToken)
            {
                _context = context;
                _cancellationToken = cancellationToken;
                _enumerator = pipeline.GetEnumerator();
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
                            if (stage is IStageAsync asyncStage)
                            {
                                await asyncStage.ExecuteAsync(_context, DispatchAsync, _cancellationToken);
                            }
                            else if (stage is IStage syncStage)
                            {
                                await Task.Run(() => syncStage.Execute(_context, () => Task.Run(DispatchAsync, _cancellationToken)), _cancellationToken);
                            }
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