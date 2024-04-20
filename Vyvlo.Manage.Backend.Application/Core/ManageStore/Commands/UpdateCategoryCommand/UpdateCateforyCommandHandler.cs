using ErrorOr;
using MediatR;
using Vyvlo.Manage.Backend.Domain.Repositories;

namespace Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.UpdateCategoryCommand;

public class UpdateCateforyCommandHandler(IManageStoreRepository repository) : IRequestHandler<UpdateCategoryCommand, ErrorOr<bool>>
{
    async Task<ErrorOr<bool>> IRequestHandler<UpdateCategoryCommand, ErrorOr<bool>>.Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (!await repository.UserAllowedToManageStoreAsync(request.AuthorizationDetails.UserId, request.Data.StoreId))
        {
            return Error.Unauthorized(description: "User is not allowed to manage this store");
        }

        var category = await repository.GetCategoryAsync(request.Data.StoreId, request.Data.CategoryId);

        if (category is null)
        {
            return Error.NotFound(description: "Category not found");
        }

        if(category.Name != request.Data.Name && await repository.CheckIfCategoryExistsAsync(request.Data.Name, request.Data.StoreId))
        {
            return Error.NotFound(description: "Category already exists");
        }

        await repository.UpdateCategoryAsync(new()
        {
            Name = request.Data.Name,
            Description = request.Data.Description,
            MetaTags = request.Data.MetaTags,
            StoreId = request.Data.StoreId,
            CategoryId = request.Data.CategoryId
        });

        return true;
    }
}
