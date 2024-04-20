using ErrorOr;
using MediatR;
using Vyvlo.Manage.Backend.Domain.Database;
using Vyvlo.Manage.Backend.Domain.Repositories;

namespace Vyvlo.Manage.Backend.Application.Core.ManageStore.Commands.CreateCategoryCommand;

/// <summary>
/// Handles the creation of a new category.
///   <list type="number">
///     <item>Check if the user is allowed to manage the store</item> 
///     <item>Check if category already exists</item> 
///     <item>Create the category</item> 
///   </list>
/// </summary>
public class CreateCategoryCommandHandler(IManageStoreRepository repo) : IRequestHandler<CreateCategoryCommand, ErrorOr<StoreCategory>>
{
    async Task<ErrorOr<StoreCategory>> IRequestHandler<CreateCategoryCommand, ErrorOr<StoreCategory>>.Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (!await repo.UserAllowedToManageStoreAsync(request.AuthorizationDetails.UserId, request.Data.StoreId))
        {
            return Error.Unauthorized(description:"User is not allowed to manage this store");
        }

        if (await repo.CheckIfCategoryExistsAsync(request.Data.Name, request.Data.StoreId))
        {
            return Error.Conflict(description:"Category already exists");
        }

        var category = StoreCategory.CreateCategory(request.Data.StoreId, request.Data.Name, request.Data.Description, request.Data.MetaTags);

        await repo.CreateCategoryAsync(category);

        return category;
    }
}