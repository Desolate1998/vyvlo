using FluentValidation;

namespace Vyvlo.Manage.Backend.Application.Core.Authentication.Queries.LoginQuery;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.Data.Email).NotEmpty().WithMessage("Email address is required");
        RuleFor(x => x.Data.Password).NotEmpty().WithMessage("Password is required");
    }
}
