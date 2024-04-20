using FluentValidation;

namespace Vyvlo.Manage.Backend.Application.Core.Authentication.Commands.RegisterCommand;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Data.FirstName).MinimumLength(2).NotNull().WithMessage("Invalid first name").OverridePropertyName("firstName");
        RuleFor(x => x.Data.LastName).MinimumLength(2).NotNull().WithMessage("Invalid Last Name").OverridePropertyName("lastName");
        RuleFor(x => x.Data.Email).EmailAddress().NotNull().WithMessage("Invalid user email").OverridePropertyName("email");
        RuleFor(x => x.Data.Password).NotNull().MinimumLength(8).WithMessage("Password must be at least 8 characters long").OverridePropertyName("password");
    }
}
