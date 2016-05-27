using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Dispatch.Stages;

namespace Irdaf.Messaging.Dispatch
{
    public class DefaultDispatcher : IDispatcher
    {
        private readonly Func<IMessageContext, IEnumerable<IStage>> _pipeline;

        public DefaultDispatcher(Func<IMessageContext, IEnumerable<IStage>> pipeline)
        {
            _pipeline = pipeline;
        }

        public void Dispatch(IMessageContext context)
        {
            var executor = new MessagePipelineExecutor(context, _pipeline(context).GetEnumerator());

            executor.Execute();
        }

        public async Task DispatchAsync(IMessageContext context, CancellationToken cancellationToken)
        {
            var executor = new MessagePipelineExecutor(context, _pipeline(context).GetEnumerator(), cancellationToken);

            await executor.ExecuteAsync();
        }

        private class MessagePipelineExecutor
        {
            private readonly IMessageContext _context;
            private readonly IEnumerator<IStage> _enumerator;
            private readonly CancellationToken _cancellationToken;

            public MessagePipelineExecutor(IMessageContext context, IEnumerator<IStage> enumerator, CancellationToken cancellationToken = default(CancellationToken))
            {
                _context = context;
                _enumerator = enumerator;
                _cancellationToken = cancellationToken;
            }

            public void Execute()
            {
                if (_enumerator.MoveNext())
                {
                    _enumerator.Current.Execute(_context, Execute);
                }
            }

            public async Task ExecuteAsync()
            {
                _cancellationToken.ThrowIfCancellationRequested();

                if (_enumerator.MoveNext())
                {
                    await _enumerator.Current.ExecuteAsync(_context, ExecuteAsync, _cancellationToken);
                }
            }
        }
    }
}