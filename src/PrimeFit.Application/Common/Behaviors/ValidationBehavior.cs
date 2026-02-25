using ErrorOr;
using FluentValidation;
using MediatR;

namespace PrimeFit.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    var errors = failures.Select(f => Error.Validation(
                         code: f.ErrorCode,
                         description: f.ErrorMessage,
                         metadata: new Dictionary<string, object>
                         {
                            { "field", f.PropertyName }
                         }
                     )).ToList();

                    return (TResponse)(dynamic)errors;

                }
            }
            return await next(cancellationToken);
        }
    }
}

