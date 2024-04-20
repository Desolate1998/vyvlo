using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Vyvlo.Manage.Backend.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validator is null)
        {
            return await next();
        }

        ValidationResult validatorResult = await validator.ValidateAsync(request, cancellationToken);

        if (validatorResult.IsValid)
        {
            return await next();
        }
        List<Error> errors = validatorResult.Errors.Select(x => Error.Validation(x.PropertyName, x.ErrorMessage)).ToList();

        return (dynamic)errors;
    }
}
