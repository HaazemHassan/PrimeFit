using System.Collections.Concurrent;

namespace PrimeFit.Application.Common.Messaging
{
    public interface IInMemoryEventDispatcher
    {
        void EnqueueEvent(IPostCommitEvent @event);
        IReadOnlyCollection<IPostCommitEvent> DequeueAll();
    }

    public class InMemoryEventDispatcher : IInMemoryEventDispatcher
    {
        private readonly ConcurrentQueue<IPostCommitEvent> _events = new();

        public void EnqueueEvent(IPostCommitEvent @event)
        {
            _events.Enqueue(@event);
        }

        public IReadOnlyCollection<IPostCommitEvent> DequeueAll()
        {
            var events = new List<IPostCommitEvent>();
            while (_events.TryDequeue(out var @event))
            {
                events.Add(@event);
            }
            return events;
        }
    }
}
