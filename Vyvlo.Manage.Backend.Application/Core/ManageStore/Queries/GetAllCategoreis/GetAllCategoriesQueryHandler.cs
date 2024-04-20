using ErrorOr;
using MediatR;
using Vyvlo.Manage.Backend.Domain.Database;
using Vyvlo.Manage.Backend.Domain.Repositories;

namespace Vyvlo.Manage.Backend.Application.Core.ManageStore.Queries.GetAllCategoreis;

public class GetAllCategoriesQueryHandler(IManageStoreRepository manageStoreRepository) : IRequestHandler<GetAllCategoriesQuery, ErrorOr<List<StoreCategory>>>
{
    public async Task<ErrorOr<List<StoreCategory>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        if (!await manageStoreRepository.UserAllowedToManageStoreAsync(request.AuthorizationDetails.UserId, request.Data.StoreId))
        {
            return Error.Unauthorized();
        }

        var categories = await manageStoreRepository.GetAllCategoriesAsync(request.Data.StoreId);
        return categories;
    }
}

