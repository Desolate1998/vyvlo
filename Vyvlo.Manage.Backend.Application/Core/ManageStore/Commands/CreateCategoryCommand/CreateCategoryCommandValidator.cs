using FluentValidation;

namespace Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.CreateCategoryCommand;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Data.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Data.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Data.StoreId).NotEmpty();
    }
}
