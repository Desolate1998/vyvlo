using FluentValidation;

namespace Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.UpdateCategoryCommand;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Data.CategoryId).NotEmpty();
        RuleFor(x => x.Data.Name).NotEmpty();
        RuleFor(x => x.Data.Description).NotEmpty();
    }
}
