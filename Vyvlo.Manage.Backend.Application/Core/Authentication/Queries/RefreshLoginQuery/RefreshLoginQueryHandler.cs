using Common.JwtTokenGenerator;
using ErrorOr;
using MediatR;
using Vyvlo.Manage.Backend.Domain.Database;
using Vyvlo.Manage.Backend.Domain.Repositories;

namespace Vyvlo.Manage.Backend.Application.Core.Authentication.Queries.RefreshLoginQuery;

public class RefreshLoginQueryHandler(IAuthenticationRepository repository, IJwtTokenGenerator tokenHelper) : IRequestHandler<RefreshLoginQuery, ErrorOr<RefreshLoginQueryResponse>>
{
    /// <summary>
    /// Handles the refresh token login query.
    /// <list type="number">
    /// <item> Check if the refresh token exists</item>
    /// <item> Log the user login </item>
    /// <item>  Get a list of user owned stores</item>
    /// <item> Generate a JWT token.</item>
    /// </list>
    /// </summary>
    async Task<ErrorOr<RefreshLoginQueryResponse>> IRequestHandler<RefreshLoginQuery, ErrorOr<RefreshLoginQueryResponse>>.Handle(RefreshLoginQuery request, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserFromRefreshToken(request.Data.RefreshToken);
        if (user is null)
        {
            return Error.Unauthorized("Invalid refresh token");
        }
        await repository.LogUserLoginAsync(LoginHistory.Create(user.UserId, user.Email, request.Authorization.Ip, "Success"));

        var stores = await repository.GetUserOwnedStoresAsync(user.UserId);
        return new RefreshLoginQueryResponse(user.FirstName, user.LastName, tokenHelper.GenerateToken(user.Email, user.FirstName, user.LastName, user.UserId), stores, request.Data.RefreshToken);
    }
}