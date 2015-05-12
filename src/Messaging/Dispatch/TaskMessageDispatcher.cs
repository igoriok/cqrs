using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Irdaf.Messaging.Providers;

namespace Irdaf.Messaging.Dispatch
{
    public class TaskMessageDispatcher : IMessageDispatcher
    {
        private readonly bool _isMultithread;

        public TaskMessageDispatcher(bool isMultithread)
        {
            _isMultithread = isMultithread;
        }

        public void Dispatch(IMessageContext context, IEnumerable<IHandlerRegistration> registrations)
        {
            var tasks = new List<Task>();

            foreach (var registration in registrations)
            {
                if (IsMultithread(registration.HandlerType) && IsMultithread(registration.HandlerMethod))
                {
                    tasks.Add(ExecuteAsync(context, registration));
                }
                else
                {
                    Execute(context, registration);
                }
            }

            if (tasks.Count > 0)
            {
                Task.WaitAll(tasks.ToArray());
            }
        }

        private bool IsMultithread(MemberInfo handlerMethod)
        {
            var attribute = handlerMethod.GetCustomAttribute<MessageHandlerAttribute>();

            if (attribute == null)
                return _isMultithread;

            return attribute.IsMultithread;
        }

        private static void Execute(IMessageContext context, IHandlerRegistration registration)
        {
            var handler = registration.CreateHandler();

            context.Execute(handler);
        }

        private static Task ExecuteAsync(IMessageContext context, IHandlerRegistration registration)
        {
            return Task.Run(() => Execute(context, registration));
        }
    }
}