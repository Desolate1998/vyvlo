using FluentValidation;

namespace Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.CreateStoreCommand;

public class CreateStoreCommandValidator : AbstractValidator<CreateStoreCommand>
{
    public CreateStoreCommandValidator()
    {
        RuleFor(x => x.Data.Name).MinimumLength(2).NotNull().WithMessage("Invalid store name").OverridePropertyName("name");
        RuleFor(x => x.Data.Description).MinimumLength(2).NotNull().WithMessage("Invalid store description").OverridePropertyName("description");
        RuleFor(x => x.Data.Image).NotNull().WithMessage("Invalid store image").OverridePropertyName("image");
    }
}
