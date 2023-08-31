using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var errorDictionary = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage, (properyName, errorMessage) => new
                {
                    Key = properyName,
                    Value = errorMessage.Distinct().ToArray()
                })
                .ToDictionary(x => x.Key, x => x.Value[0]);

            if (errorDictionary.Any())
            {
                var error = errorDictionary.Select(x => new ValidationFailure
                {
                    PropertyName = x.Value,
                    ErrorCode = x.Key
                });
                throw new ValidationException(error);
            }
            return await next();
        }
    }
}
