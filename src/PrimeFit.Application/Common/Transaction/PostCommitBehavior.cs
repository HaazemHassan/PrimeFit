using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using PrimeFit.Application.Common.Messaging;

namespace PrimeFit.Application.Common.Transaction
{
    public class PostCommitBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IInMemoryEventDispatcher _eventDispatcher;
        private readonly IPublisher _publisher;
        private readonly ILogger<PostCommitBehavior<TRequest, TResponse>> _logger;

        public PostCommitBehavior(
            IInMemoryEventDispatcher eventDispatcher,
            IPublisher publisher,
            ILogger<PostCommitBehavior<TRequest, TResponse>> logger)
        {
            _eventDispatcher = eventDispatcher;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            // Check if response is ErrorOr and has no errors, or if it's not ErrorOr (assuming success)
            bool isSuccess = true;
            if (response is IErrorOr errorOrResponse)
            {
                isSuccess = !errorOrResponse.IsError;
            }

            if (isSuccess)
            {
                var events = _eventDispatcher.DequeueAll();
                
                if (events.Any())
                {
                    _logger.LogInformation("Dispatching {Count} post-commit events for {RequestName}", events.Count, typeof(TRequest).Name);
                    
                    foreach (var @event in events)
                    {
                        try
                        {
                            await _publisher.Publish(@event, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "An error occurred while publishing post-commit event {EventName}", @event.GetType().Name);
                       
                    }
                    }
                }
            }

            return response;
        }
    }
}
